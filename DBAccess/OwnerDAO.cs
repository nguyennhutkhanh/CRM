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
    public class OwnerDAO
    {
        string connectionString = ConfigurationManager.AppSettings["lxd_vexe_conn"].ToString();

        public async Task<ErrorDBO> AddUpOwner(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            OwnerBDO o = new OwnerBDO();
            int last_id = 0;
     
            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    o = JsonConvert.DeserializeObject<OwnerBDO>(requestContent);

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_owner_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@owner_id", o.co_id);
                            cmd.Parameters.AddWithValue("@co_name", o.co_name);
                            cmd.Parameters.AddWithValue("@co_fax", o.co_fax);
                            cmd.Parameters.AddWithValue("@co_phone", o.co_phone);
                            cmd.Parameters.AddWithValue("@co_address", o.co_address);
                            cmd.Parameters.AddWithValue("@parent", o.parent);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                if (rd.HasRows)
                                {
                                    await rd.ReadAsync();
                                    last_id = (int)rd["last_id"];

                                    if (last_id > 0) { e.status = true; e.message = last_id.ToString(); }
                                    else { e.status = false; e.message = "Fail"; }
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
                ErrorDetail err = new ErrorDetail() { error_info = "Not Allowed", error_detail = "Token is required" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return e;
        }

        public async Task<OwnerBDO> GetOwner(int id)
        {
            OwnerBDO o = new OwnerBDO();
          
                if (await Token.AuthenticatedCheck())
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandText = "ws_owner_sel";
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@owner_id", id);
                                cmd.Connection = conn;
                                await conn.OpenAsync();

                                using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                                {
                                    if (rd.HasRows)
                                    {
                                        await rd.ReadAsync();
                                        o = new OwnerBDO();
                                        o.co_id = (int)rd["co_id"];
                                        o.co_name = (string)rd["co_name"];
                                        o.co_fax = (string)rd["co_fax"];
                                        o.co_phone = (string)rd["co_phone"];
                                        o.co_address = (string)rd["co_address"];
                                        o.parent = (int)rd["parent"];
                                    }
                                    rd.Close();
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
                    ErrorDetail err = new ErrorDetail() { error_info = "Not Allowed", error_detail = "Token is required" };
                    throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
                }
            
            return o;
        }

        public async Task<List<OwnerBDO>> GetAllOwnerByOwner(int id)
        {
            List<OwnerBDO> l_o = new List<OwnerBDO>();

            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_owner_sel_by_owner";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@owner_id", id);
                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                if (rd.HasRows)
                                {
                                    while (await rd.ReadAsync())
                                    {
                                        l_o.Add(new OwnerBDO()
                                        {
                                            co_id = (int)rd["co_id"]
                                            ,co_name = (string)rd["co_name"]
                                            ,co_fax = (string)rd["co_fax"]
                                            ,co_phone = (string)rd["co_phone"]
                                            ,co_address = (string)rd["co_address"]
                                            ,parent = (int)rd["parent"]
                                        });
                                    }
                                }

                                rd.Close();
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
                ErrorDetail err = new ErrorDetail() { error_info = "Not Allowed", error_detail = "Token is required" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
           
            return l_o;
        }

    }
}