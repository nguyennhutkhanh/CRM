using System;
using System.Net;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;
using System.ServiceModel.Web;

using Newtonsoft.Json;

using WcfService.Format;
using WcfService.Model;
using WcfService.Common;

namespace WcfService.DAO
{
    public class DriverDAO
    {
        string connectionString = ConfigurationManager.AppSettings["lxd_vexe_conn"].ToString();

        public async Task<ErrorDBO> AddUpDriver(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            DriverBDO d = new DriverBDO();
            int last_id = 0;

            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Stream_JSON.StreamToJSON(requestContent);

                    d = JsonConvert.DeserializeObject<DriverBDO>(requestContent);
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_driver_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@driver_id", d.driver_id);
                            cmd.Parameters.AddWithValue("@full_name", d.full_name);
                            cmd.Parameters.AddWithValue("@mobile", d.mobile);
                            cmd.Parameters.AddWithValue("@co_id", d.co_id);
                            cmd.Parameters.AddWithValue("@user_id", d.user_id);


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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - driver_addup" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
           
            return e;
        }

        public async Task<DriverBDO> GetDriver(int id)
        {
            DriverBDO d = new DriverBDO();

            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_driver_sel";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@driver_id", id);
                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                d.driver_id = (int)rd["driver_id"];
                                d.full_name = (string)rd["full_name"];
                                d.mobile = (string)rd["mobile"];
                                d.co_id = (int)rd["co_id"];
                                d.user_id = (int)rd["user_id"];
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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - driver_get" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return d;
        }

        public async Task<List<DriverBDO>> GetAllDriver(int id)
        {
            List<DriverBDO> l_d = new List<DriverBDO>();

            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_driver_sel_all";
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
                                        l_d.Add(new DriverBDO()
                                        {
                                            driver_id = (int)rd["driver_id"]
                                            ,full_name = (string)rd["full_name"]
                                            ,mobile = (string)rd["mobile"]
                                            ,co_id = (int)rd["co_id"]
                                            ,user_id = (int)rd["user_id"]
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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - driver_get_all" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
           
            return l_d;
        }

        public async Task<ErrorDBO> DelDriver(int id)
        {
            ErrorDBO e = new ErrorDBO();
            int code = 0;

           
            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_driver_del";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@driver_id", id);
                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                code = (int)rd["code"];
                                if (code > 0) { e.status = true; e.message = "Successful"; }
                                else { e.status = false; e.message = "fail"; }
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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - driver_get_del" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
           
            return e;
        }

    }
}
