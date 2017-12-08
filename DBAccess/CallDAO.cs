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
    public class CallDAO
    {
        string connectionString = ConfigurationManager.AppSettings["cm_conn"].ToString();

        public async Task<ErrorDBO> AddUpdateCall(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            CallBDO c = new CallBDO();
            int last_id = 0;


            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    c = JsonConvert.DeserializeObject<CallBDO>(requestContent);
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "s_call_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@call_id", c.call_id);
                            cmd.Parameters.AddWithValue("@call_no", c.call_no);
                            cmd.Parameters.AddWithValue("@start_time", c.start_time);
                            cmd.Parameters.AddWithValue("@end_time", c.end_time);
                            cmd.Parameters.AddWithValue("@content", c.content);
                            cmd.Parameters.AddWithValue("@note", c.note);
                            cmd.Parameters.AddWithValue("@url_rec", c.url_rec);
                            cmd.Parameters.AddWithValue("@status", c.status);
                            cmd.Parameters.AddWithValue("@user_id", c.user_id);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                last_id = (int)rd["call_id"];

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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - call_addup" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return e;
        }

        public async Task<CallTotalStatusBBO> GetCallStatusDeparment(int deparment_id)
        {
            CallTotalStatusBBO c = new CallTotalStatusBBO();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_call_count_status_department";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@department_id", deparment_id);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                while (await rd.ReadAsync())
                                {
                                    c.total_waiting = (int)rd[0];
                                    c.total_processing = (int)rd[1];
                                    c.total_rejected = (int)rd[2];
                                    c.total_confirm_waiting = (int)rd[3];
                                    c.total_finish = (int)rd[4];
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
            return c;
        }

        public async Task<List<CallBDO>> GetCallHistoryTop5(string phone)
        {
            List<CallBDO> c = new List<CallBDO>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_call_sel_history_last_top_5";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@call_no", phone);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                while (await rd.ReadAsync())
                                {
                                    c.Add(new CallBDO()
                                    {
                                        call_id = (int)rd["id"]
                                        ,call_no = (string)rd["call_no"]
                                        ,start_time = (DateTime)rd["start_time"]
                                        ,end_time = (DateTime)rd["end_time"]
                                        ,content = (string)rd["content"]
                                        ,note = (string)rd["note"]
                                        ,url_rec = (string)rd["url_rec"]
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
            return c;
        }

        public async Task<List<CallBDO>> GetCallStatus(int status)
        {
            List<CallBDO> lc = new List<CallBDO>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_call_sel_all_status";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                while (await rd.ReadAsync())
                                {
                                    lc.Add(new CallBDO()
                                    {
                                        call_id = (int)rd["id"]
                                        ,call_no = (string)rd["call_no"]
                                        ,start_time = (DateTime)rd["start_time"]
                                        ,end_time = (DateTime)rd["end_time"]
                                        ,status = (byte)rd["status"]
                                        ,content = (string)rd["content"]
                                        ,note = (string)rd["note"]
                                        ,url_rec = (string)rd["url_rec"]
                                        ,case_trans_id = (int)rd["case_trans_id"]
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
            return lc;
        }

        public async Task<CallBDO> GetCall(int call_id)
        {
            CallBDO c = new CallBDO();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_call_sel";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", call_id);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                while (await rd.ReadAsync())
                                {
                                    c.call_id = (int)rd["id"];
                                    c.call_no = (string)rd["call_no"];
                                    c.start_time = (DateTime)rd["start_time"];
                                    c.end_time = (DateTime)rd["end_time"];
                                    c.status = (byte)rd["status"];
                                    c.content = (string)rd["content"];
                                    c.note = (string)rd["note"];
                                    c.url_rec = (string)rd["url_rec"];
                                    
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
            return c;
        }
    }
}