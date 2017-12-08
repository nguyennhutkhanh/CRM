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
    public class BusDAO
    {
        string connectionString = ConfigurationManager.AppSettings["lxd_vexe_conn"].ToString();

        public async Task<ErrorDBO> AddUpBus(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            BusBDO b = new BusBDO();
            int last_id = 0;

            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    b = JsonConvert.DeserializeObject<BusBDO>(requestContent);

                    DataTable dt_util = new DataTable();
                    dt_util.Columns.Add("util_1", typeof(byte));
                    dt_util.Columns.Add("util_2", typeof(byte));
                    dt_util.Columns.Add("util_3", typeof(byte));


                    dt_util.Rows.Add(   b.util.util_1
                                        ,b.util.util_2
                                        ,b.util.util_3);
                    
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_bus_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@bus_id", b.bus_id);
                            cmd.Parameters.AddWithValue("@co_id", b.co_id);
                            cmd.Parameters.AddWithValue("@bus_no", b.bus_no);
                            cmd.Parameters.AddWithValue("@driver_id", b.driver_id);
                            cmd.Parameters.AddWithValue("@bus_type", b.bus_type);
                            cmd.Parameters.AddWithValue("@num_seat", b.num_seat);
                            cmd.Parameters.AddWithValue("@img", b.img);
                            cmd.Parameters.AddWithValue("@user_id", b.user_id);

                            cmd.Parameters.AddWithValue("@util", dt_util);
                            cmd.Parameters["@util"].SqlDbType = System.Data.SqlDbType.Structured;

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                last_id = (int)rd["last_id"];

                                if (last_id > 0) { e.status = true; e.message = last_id.ToString(); }
                                else { e.status = false; e.message = "Fail"; }
                            }
                            //if (await cmd.ExecuteNonQueryAsync() != 1)
                            //{
                            //    e.status = true;
                            //    e.message = "Success";
                            //}
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
                 ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - bus_addup" };
                 throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
           
            return e;
        }

        public async Task<BusBDO> GetBus(int id)
        {
            BusBDO b = new BusBDO();

            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_bus_sel";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@bus_id", id);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                if (rd.HasRows)
                                {
                                    await rd.ReadAsync();
                                    b.bus_id = (int)rd["bus_id"];
                                    b.bus_no = (string)rd["bus_no"];
                                    b.driver_id = (int)rd["driver_id"];
                                    b.num_seat = (int)rd["num_seat"];
                                    b.bus_type = (byte)rd["bus_type"];
                                    b.user_id = (int)rd["user_id"];

                                    await rd.NextResultAsync();
                                    b.util = new BusUtilBDO();

                                    if (rd.HasRows)
                                    {
                                        await rd.ReadAsync();
                                        b.util.util_1 = (bool)rd["util_1"]; //(byte)rd["util_1"] == 1 ? true : false;
                                        b.util.util_2 = (bool)rd["util_2"];//(byte)rd["util_2"] == 1 ? true : false; 
                                        b.util.util_3 = (bool)rd["util_3"];//(byte)rd["util_3"]== 1 ? true : false;
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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - bus_sel" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return b;
        }

        public async Task<ErrorDBO> DelBus(int id)
        {
            ErrorDBO e = new ErrorDBO();
            bool code = false;

            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_bus_del";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@bus_id", id);
                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                code = (bool)rd["code"];

                                if (code == true) { e.message = "Success"; e.status = true; }
                                else { e.message = "Fail"; e.status = false; }
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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - bus_del" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
                          
            return e;
        }

        public async Task<List<BusBDO>> GetAllBus(int id)
        {
            List<BusBDO> l_b = new List<BusBDO>();
             // = new BusUtilBDO();

            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_bus_sel_all";
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
                                        l_b.Add(new BusBDO()
                                        {
                                            bus_id = (int)rd["bus_id"]
                                            ,bus_no = (string)rd["bus_no"]
                                            ,co_id = (int)rd["co_id"]
                                            ,driver_id = (int)rd["driver_id"]
                                            ,num_seat = (int)rd["num_seat"]
                                            ,bus_type = (byte)rd["bus_type"]

                                            //
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
               ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - bus_sel_all" };
               throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return l_b;
        }
    }
}
