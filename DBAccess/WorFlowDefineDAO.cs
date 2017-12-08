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
    public class WorkFlowDefineDAO        
    {
        string connectionString = ConfigurationManager.AppSettings["cm_conn"].ToString();

        public async Task<WorkFlowDefineBDO> GetWorkFlowDefine(int id)
        {
            WorkFlowDefineBDO wfd = null;

            try
            {
                //    if (await Token.AuthenticatedCheck())
                //   {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_workflow_define_sel";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@wf_define_id", id);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();
                                wfd = new WorkFlowDefineBDO();
                                wfd.wf_define_id = id;
                                wfd.wf_define_desc = (string)reader["wf_define_desc"];
                                //wf.case_id = (int)reader["case_id"];
                                wfd.case_id = (int)reader["case_id"];
                                wfd.user_create_id = (int)reader["user_create_id"];

                            }
                            await reader.NextResultAsync();
                            wfd.WorkFlowList = new List<WorkFlowBDO>();
                            while(await reader.ReadAsync())
                            {
                                wfd.WorkFlowList.Add(new WorkFlowBDO()
                                {
                                    id = (int)reader["id"]
                                    ,wf_define_id = (int)reader["wf_define_id"]
                                    ,wf_desc = (string)reader["wf_desc"]
                                    ,next = (int)reader["next"]
                                    ,department_id = (int)reader["department_id"]
                                    ,status = (byte)reader["status"]
                                });
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

            return wfd;
        }

        public async Task<ErrorDBO> AddUpdateWorkFlowDefine(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            WorkFlowDefineBDO wfd = new WorkFlowDefineBDO();
            int last_id = 0;


            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    wfd = JsonConvert.DeserializeObject<WorkFlowDefineBDO>(requestContent);
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "s_workflow_define_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@wf_define_id", wfd.wf_define_id);
                            cmd.Parameters.AddWithValue("@wf_define_desc", wfd.wf_define_desc);
                            cmd.Parameters.AddWithValue("@case_id", wfd.case_id);
                            cmd.Parameters.AddWithValue("@user_create_id", wfd.user_create_id);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                last_id = (int)rd["wf_define_id"];

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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - wfd_addup" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return e;
        }

        public async Task<ErrorDBO> DelWorkFlowDefine(int id)
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
                            cmd.CommandText = "s_workflow_define_del";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@wf_define_id", id);
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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - wfd_del" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }

            return e;
        }

        public async Task<List<WorkFlowDefineBDO>> GetAllWorkFlowDefine()
        {
            List<WorkFlowDefineBDO> l_wfd = new List<WorkFlowDefineBDO>();

            //if (await Token.AuthenticatedCheck())
            //{
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_workflow_define_sel_all";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                while (await rd.ReadAsync())
                                {
                                    l_wfd.Add(new WorkFlowDefineBDO()
                                    {
                                        wf_define_id = (int)rd["wf_define_id"]
                                        ,wf_define_desc = (string)rd["wf_define_desc"]
                                        ,case_id = (int)rd["case_id"]
                                        ,user_create_id = (int)rd["user_create_id"]

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
            //    ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - wfd_sel_all" };
            //    throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            //}

            return l_wfd;
        }

        public async Task<WorkFlowDefineBDO> GetWorkFlowDefineCase(int case_id)
        {
            WorkFlowDefineBDO wfd = null;

            try
            {
                //    if (await Token.AuthenticatedCheck())
                //   {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_workflow_define_sel_case";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@case_id", case_id);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();
                                wfd = new WorkFlowDefineBDO();
                                wfd.wf_define_id = (int)reader["wf_define_id"];
                                wfd.wf_define_desc = (string)reader["wf_define_desc"];
                                
                                wfd.case_id = (int)reader["case_id"];
                                wfd.user_create_id = (int)reader["user_create_id"];

                            }
                            await reader.NextResultAsync();
                            wfd.WorkFlowList = new List<WorkFlowBDO>();
                            while (await reader.ReadAsync())
                            {
                                wfd.WorkFlowList.Add(new WorkFlowBDO()
                                {
                                    id = (int)reader["id"]
                                    ,wf_define_id = (int)reader["wf_define_id"]
                                    ,wf_desc = (string)reader["wf_desc"]
                                    ,next = (int)reader["next"]
                                    ,department_id = (int)reader["department_id"]
                                    ,status = (byte)reader["status"]
                                });
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

            return wfd;
        }
    }
}