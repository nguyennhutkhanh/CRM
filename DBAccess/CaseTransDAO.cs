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
    public class CaseTransDAO
    {
        string connectionString = ConfigurationManager.AppSettings["cm_conn"].ToString();

        public async Task<CaseTransBDO> GetCaseTrans(int id)
        {
            CaseTransBDO c = null;

            try
            {
                //    if (await Token.AuthenticatedCheck())
                //   {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_case_trans_sel";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@case_trans_id", id);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();
                                c = new CaseTransBDO();
                                c.id = id;
                                c.wf_define_id = (int)reader["wf_define_id"];
                                c.case_id = (int)reader["case_id"];
                                c.call_id = (int)reader["call_id"];
                                c.assign_agent_id = (int)reader["assign_agent_id"];
                                c.content = (string)reader["content"];
                                c.note = (string)reader["note"];
                                c.reason = (string)reader["reason"];
                                c.status = (byte)reader["status"];
                            }
                            reader.Close();
                        }
                    }
                }
                //}
                //else
                //{
                //    ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - user_sel" };
                //    throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
                //}
            }
            catch (Exception ex)
            {
                Logs.writeToLogFile(ex.ToString());
            }

            return c;
        }

        public async Task<ErrorDBO> AddUpdateCaseTrans(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            CaseTransBDO c = new CaseTransBDO();
            int last_id = 0;


            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    c = JsonConvert.DeserializeObject<CaseTransBDO>(requestContent);
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "s_case_trans_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@case_trans_id", c.id);
                            cmd.Parameters.AddWithValue("@wf_define_id", c.wf_define_id);
                            cmd.Parameters.AddWithValue("@case_id", c.case_id);
                            cmd.Parameters.AddWithValue("@call_id", c.call_id);
                            cmd.Parameters.AddWithValue("@assign_agent_id", c.assign_agent_id);
                            cmd.Parameters.AddWithValue("@content", c.content);
                            cmd.Parameters.AddWithValue("@note", c.note);
                            cmd.Parameters.AddWithValue("@reason", c.reason);
                            cmd.Parameters.AddWithValue("@status", c.status);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                last_id = (int)rd["case_trans_id"];

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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - user_addup" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return e;
        }

        public async Task<ErrorDBO> DelCaseTrans(int id)
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
                            cmd.CommandText = "s_case_trans_del";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@case_trans_id", id);
                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                code = (int)rd["code"];

                                if (code > 0) { e.status = true; e.message = "Successful"; }
                                else { e.status = false; e.message = "Fail"; }

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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - user_del" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }

            return e;
        }

        public async Task<List<CaseTransBDO>> GetAllCaseTrans()
        {
            List<CaseTransBDO> l_c = new List<CaseTransBDO>();

          
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_case_trans_sel_all";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@co_id", id);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                while (await rd.ReadAsync())
                                {
                                    l_c.Add(new CaseTransBDO()
                                    {
                                        id = (int)rd["id"]
                                        ,wf_define_id = (int)rd["wf_define_id"]
                                        ,case_id = (int)rd["case_id"]
                                        ,call_id = (int)rd["call_id"]
                                        ,assign_agent_id = (int)rd["assign_agent_id"]
                                        ,content = (string)rd["content"]
                                        ,note = (string)rd["note"]
                                        ,reason = (string)rd["reason"]
                                        ,status = (byte)rd["status"]
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
            //}
            //else
            //{
            //    ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - user_sel_all" };
            //    throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            //}

            return l_c;
        }

        public async Task<List<CaseTransHistoryBDO>> GetCaseHistory(string phone)
        {
            List<CaseTransHistoryBDO> l_c = new List<CaseTransHistoryBDO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_case_trans_history";
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
                                    l_c.Add(new CaseTransHistoryBDO()
                                    {
                                        create_date = (DateTime)(rd["create_date"])
                                        ,case_desc = (string)rd["case_desc"]
                                        ,wf_define_desc = (string)rd["wf_define_desc"]
                                        ,status = (byte)rd["status"]
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
            //}
            //else
            //{
            //    ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - user_sel_all" };
            //    throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            //}

            return l_c;
        }

        public async Task<List<CaseTransBDO>> GetCaseDetail(string phone, int case_id)
        {
            List<CaseTransBDO> l_c = new List<CaseTransBDO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_case_trans_detail";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@call_no", phone);
                        cmd.Parameters.AddWithValue("@case_id", case_id);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                while (await rd.ReadAsync())
                                {
                                    l_c.Add(new CaseTransBDO()
                                    {
                                        id = (int)rd["case_id"]
                                        ,case_desc = (string)rd["case_desc"]
                                        ,assign_agent_id = (int)rd["assign_agent_id"]
                                        ,content = (string)rd["content"]
                                        ,note = (string)rd["note"]
                                        ,reason = (string)rd["reason"]
                                        ,status = (byte)rd["status"]
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
            //}
            //else
            //{
            //    ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - user_sel_all" };
            //    throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            //}

            return l_c;
        }
    }
}