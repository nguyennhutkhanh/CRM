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
    public class CaseDAO
    {
        string connectionString = ConfigurationManager.AppSettings["cm_conn"].ToString();

        public async Task<CaseBDO> GetCase(int id)
        {
            CaseBDO c = null;

            try
            {
                //    if (await Token.AuthenticatedCheck())
                //   {
                using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "s_case_sel";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@case_id", id);
                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                            {
                                if (reader.HasRows)
                                {
                                    await reader.ReadAsync();
                                    c = new CaseBDO();
                                    c.case_id = id;
                                    c.case_desc = (string)reader["case_desc"];
                                    c.case_fb_id = (int)reader["case_fb_id"];
									c.user_create_id = (int)reader["user_create_id"];
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

        public async Task<ErrorDBO> AddUpdateCase(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            CaseBDO c = new CaseBDO();
            int last_id = 0;
            

            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    c = JsonConvert.DeserializeObject<CaseBDO>(requestContent);
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "s_case_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@case_id", c.case_id);

                            cmd.Parameters.AddWithValue("@case_desc", c.case_desc);
                            cmd.Parameters.AddWithValue("@case_fb_id", c.case_fb_id);
                            cmd.Parameters.AddWithValue("@user_create_id", c.user_create_id);
                         

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                last_id = (int)rd["case_id"];

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

        public async Task<ErrorDBO> DelCase(int id)
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
                            cmd.CommandText = "s_case_del";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@case_id", id);
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

        public async Task<List<CaseBDO>> GetAllCase()
        {
            List<CaseBDO> l_c = new List<CaseBDO>();
          
            //if (await Token.AuthenticatedCheck())
            //{
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "s_case_sel_all";
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
                                        l_c.Add(new CaseBDO()
                                        {
                                            case_id = (int)rd["id"]
                                            ,case_desc = (string)rd["case_desc"]
                                            ,case_fb_id = (int)rd["case_fb_id"]
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
            //    ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - user_sel_all" };
            //    throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            //}
           
            return l_c;
        }

    }
}
