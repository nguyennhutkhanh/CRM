using System;
using System.Net;
using System.ServiceModel.Web;

using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using WcfService.Model;

using WcfService.Common;

namespace WcfService.DAO
{
    public class TicketDAO
    {
        string connectionString = ConfigurationManager.AppSettings["lxd_vexe_conn"].ToString();

        public async Task<ErrorDBO> AddUpTicket(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            TicketBDO t = new TicketBDO();
            int _last_id = 0;

            try
            {
                StreamReader reader = new StreamReader(stream);
                string requestContent = reader.ReadToEnd();
                requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                t = JsonConvert.DeserializeObject<TicketBDO>(requestContent);

                if (await Token.AuthenticatedCheck())
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_ticket_registry";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ticket_id", t.ticket_id);
                            cmd.Parameters.AddWithValue("@ticket_no", t.ticket_no);
                            cmd.Parameters.AddWithValue("@bus_id", t.bus_id);
                            cmd.Parameters.AddWithValue("@s_b_id", t.s_b_id);
                            cmd.Parameters.AddWithValue("@client_id", t.client_id);
                            cmd.Parameters.AddWithValue("@price", t.price);
                            cmd.Parameters.AddWithValue("@trip_id", t.trip_id);
                            cmd.Parameters.AddWithValue("@trip_category_id", t.trip_category_id);
                            cmd.Parameters.AddWithValue("@user_id", t.user_id);                

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                _last_id = (int)rd["last_id"];

                                if (_last_id > 0) { e.status = true; e.message = _last_id.ToString(); }
                                else { e.status = false; e.message = "Fail"; }
                            }
                        }
                    }
                }
                else
                {
                    ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - ticket_registry_addup" };
                    throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
                }
            }
            catch (Exception ex)
            {
                Logs.writeToLogFile(ex.ToString());
            }

            return e;
        }

        public async Task<TicketResponseBDO> GetTicket(int id)
        {
            TicketResponseBDO t = new TicketResponseBDO();

            try
            {
                if (await Token.AuthenticatedCheck())
                {

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_ticket_sel";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ticket_id", id);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                t.ticket_id = (int)rd["ticket_id"];
                                t.ticlet_no = (string)rd["ticket_no"];
                                t.s_b_id = (int)rd["s_b_id"];
                                t.s_b_no = (string)rd["s_b_no"];
                                t.client_iid = (int)rd["client_id"];
                                t.client_name = (string)rd["client_name"];
                                t.status_id = (byte)rd["status_id"];
                                t.note = (string)rd["note"];
                             
                                rd.Close();
                            }

                        }
                    }
                }
                else
                {
                    ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - ticket_sel" };
                    throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
                }
            }
            catch (Exception ex)
            {
                Logs.writeToLogFile(ex.ToString());
            }

            return t;
        }
    }
}