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
    public class WorkFlowDAO
    {
        string connectionString = ConfigurationManager.AppSettings["cm_conn"].ToString();

        public async Task<WorkFlowBDO> GetWorkFlow(int id)
        {
            WorkFlowBDO wf = null;

            try
            {
                //    if (await Token.AuthenticatedCheck())
                //   {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_workflow_sel";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@wf_id", id);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();
                                wf = new WorkFlowBDO();
                                wf.id = id;
                                wf.wf_define_id = (int)reader["wf_define_id"];
                                wf.wf_desc = (string)reader["wf_desc"];
                            
                                //wf.call_id = (int)reader["call_id"];
                                wf.next = (int)reader["next"];
                                wf.department_id = (int)reader["department_id"];
                                //wf.status = (byte)reader["status"];
                                //wf.user_create_id = (int)reader["user_create_id"];

                            }
                            reader.Close();
                        }
                    }
                }
                //}
                //else
                //{
                //    ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - wf_sel" };
                //    throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
                //}
            }
            catch (Exception ex)
            {
                Logs.writeToLogFile(ex.ToString());
            }

            return wf;
        }

        public async Task<ErrorDBO> AddUpdateWorkFlow(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            WorkFlowBDO wf = new WorkFlowBDO();
            int last_id = 0;


            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    wf = JsonConvert.DeserializeObject<WorkFlowBDO>(requestContent);
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "s_workflow_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@wf_id", wf.id);
                            cmd.Parameters.AddWithValue("@wf_desc", wf.wf_desc);
                      
                            //cmd.Parameters.AddWithValue("@call_id", wf.call_id);
                            cmd.Parameters.AddWithValue("@wf_define_id", wf.wf_define_id);
                            cmd.Parameters.AddWithValue("@next", wf.next);
                            cmd.Parameters.AddWithValue("@department_id", wf.department_id);
                            //cmd.Parameters.AddWithValue("@status", wf.status);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                last_id = (int)rd["wf_id"];

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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - wf_addup" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return e;
        }

        public async Task<ErrorDBO> DelWorkFlow(int id)
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
                            cmd.CommandText = "s_workflow_del";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@wf_id", id);
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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - wf_del" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }

            return e;
        }

        public async Task<List<WorkFlowBDO>> GetAllWorkFlow()
        {
            List<WorkFlowBDO> l_wf = new List<WorkFlowBDO>();

            //if (await Token.AuthenticatedCheck())
            //{
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_workflow_sel_all";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                while (await rd.ReadAsync())
                                {
                                    l_wf.Add(new WorkFlowBDO()
                                    {
                                        id = (int)rd["id"]
                                        ,wf_define_id = (int)rd["wf_define_id"]
                                        ,wf_desc = (string)rd["wf_desc"]
                                
                                        //,call_id = (int)rd["call_id"]
                                        ,next = (int)rd["next"]
                                        ,department_id = (int)rd["department_id"]
                                       // ,status = (byte)rd["status"]

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
            //    ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - wf_sel_all" };
            //    throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            //}

            return l_wf;
        }
    }
}