using System;
using System.Net;
using System.ServiceModel.Web;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;


using Newtonsoft.Json;

using WcfService.Model;
using WcfService.Common;

namespace WcfService.DAO
{ 
    public class LocDAO
    {
        string connectionString = ConfigurationManager.AppSettings["lxd_vexe_conn"].ToString();

        public async Task<ErrorDBO> AddUpLocDrpPck(Stream stream, int id)
        {
            ErrorDBO e = new ErrorDBO();
            LocDrpPckBDO l = new LocDrpPckBDO();
            int last_id = 0;

            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    l = JsonConvert.DeserializeObject<LocDrpPckBDO>(requestContent);

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_location_pick_drop_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@loc_id", l.location_id);
                            cmd.Parameters.AddWithValue("@name", l.name);
                            cmd.Parameters.AddWithValue("@address", l.address);
                            cmd.Parameters.AddWithValue("@co_id", id);
                            cmd.Parameters.AddWithValue("@user_id", l.user_id);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                last_id = (int)rd["last_id"];
                                if (last_id > 0) { e.status = true; e.message = last_id.ToString(); }
                                else { e.status = false; e.message = "Fail"; }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logs.writeToLogFile(ex.ToString());
                }

            }
            else
            {
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - loc_addup" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
          

            return e;
            
        }

        public async Task<List<LocDrpPckBDO>> GetAllLoc(int id)
        {
            List<LocDrpPckBDO> l = new List<LocDrpPckBDO>();

            if (await Token.AuthenticatedCheck())
            {
                try
                {

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_location_pick_drop_sel_by_owner";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@co_id", id);
                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                if (rd.HasRows)
                                {
                                    while (await rd.ReadAsync())
                                    {
                                        l.Add(new LocDrpPckBDO()
                                        {
                                            location_id = (int)rd["location_id"]
                                            ,name = (string)rd["name"]
                                            ,address = (string)rd["address"]
                                            ,co_id = (int)rd["co_id"]
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logs.writeToLogFile(ex.ToString());
                }
            }
            else
            {
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - loc_sel_all" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
           
            
            return l;
        }
    }
}