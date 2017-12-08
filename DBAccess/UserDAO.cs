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
    public class UserDAO
    {
        string connectionString = ConfigurationManager.AppSettings["cm_conn"].ToString();

        public async Task<UserBDO> GetUser(int id)
        {
            UserBDO u = null;

            try
            {
                //    if (await Token.AuthenticatedCheck())
                //   {
                using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "s_user_sel";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@user_id", id);
                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                            {
                                if (reader.HasRows)
                                {
                                    await reader.ReadAsync();
                                    u = new UserBDO();
                                    u.id = id;
                                    u.user_name = (string)reader["user_name"];
                                    u.api_key = (string)reader["api_key"];
                                    u.mobile = (string)reader["mobile"];
                                    u.email = (string)reader["email"];
                                    u.user_group_id = (int)reader["user_group_id"];
                                    u.group_name = (string)reader["group_name"];
                                    u.department_id = (int)reader["department_id"];
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

            return u;
        }

        public async Task<ErrorDBO> AddUpdateUser(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            UserBDO u = new UserBDO();
            int last_id = 0;
            

            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    u = JsonConvert.DeserializeObject<UserBDO>(requestContent);
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "s_user_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@user_id", u.id);
                            cmd.Parameters.AddWithValue("@user_name", u.user_name);
                            cmd.Parameters.AddWithValue("@hash_pwd", u.hash_pwd);
                            cmd.Parameters.AddWithValue("@email", u.email);
                            cmd.Parameters.AddWithValue("@mobile", u.mobile);
                            cmd.Parameters.AddWithValue("@user_group_id", u.user_group_id);
                            cmd.Parameters.AddWithValue("@department_id", u.department_id);
                            //cmd.Parameters.AddWithValue("@co_id", u.co_id);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                last_id = (int)rd["user_id"];

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

        public async Task<UserBDO> LoginUser(string user_name, string pwd)
        {
            ErrorDBO e = new ErrorDBO();
            UserBDO u = new UserBDO();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_user_login";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        if (user_name.Contains("@"))
                            cmd.Parameters.AddWithValue("@email", user_name);
                        //else
                        //    cmd.Parameters.AddWithValue("@mobile", user_name);
                        cmd.Parameters.AddWithValue("@hash_pwd", pwd);

                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                await rd.ReadAsync();
                                u.id = (int)rd["id"];
                                u.user_name = (string)rd["user_name"];
                                u.email = (string)rd["email"];
                                //u.hash_pwd
                                u.mobile = (string)rd["mobile"];
                                u.api_key = (string)rd["api_key"];
                                u.group_name = (string)rd["group_name"];
                            }

                            await rd.NextResultAsync();
                            u.module_list = new List<UserModuleBDO>();
                            while (await rd.ReadAsync())
                            {
                                u.module_list.Add(new UserModuleBDO()
                                {
                                    mobile_id = (int)rd["module_id"]
                                    ,module_name = (string)rd["module_name"]
                                    //,role_id = (int)rd["role_id"]
                                    ,allow = (bool)rd["allow"]
                                });
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
            
            return u;
        }

        public async Task<ErrorDBO> DelUser(int id)
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
                            cmd.CommandText = "s_user_del";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@user_id", id);
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

        public async Task<List<UserBDO>> GetAllUser()
        {
            List<UserBDO> l_u = new List<UserBDO>();
          
            //if (await Token.AuthenticatedCheck())
            //{
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "s_user_sel_all";
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
                                        l_u.Add(new UserBDO()
                                        {
                                            id = (int)rd["id"]
                                            ,user_name = (string)rd["user_name"]
                                            ,email = (string)rd["email"]
                                            ,mobile = (string)rd["mobile"]
                                            ,user_group_id = (int)rd["user_group_id"]
                                            ,group_name = (string)rd["group_name"]
                                            ,api_key = (string)rd["api_key"]
                                            //,hash_pwd = (string)rd["hash_pwd"]
                                            ,department_id = (int)rd["department_id"]

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
           
            return l_u;
        }

    }
}
