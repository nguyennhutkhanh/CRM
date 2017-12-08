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
    public class UserGroupDAO
    {
        string connectionString = ConfigurationManager.AppSettings["cm_conn"].ToString();

        public async Task<UserGroupBDO> GetUserGroup(int id)
        {
            UserGroupBDO ug = null;

            try
            {
                //    if (await Token.AuthenticatedCheck())
                //   {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_user_group_sel";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@user_group_id", id);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();
                                ug = new UserGroupBDO();
                                ug.id = id;
                                ug.group_name = (string)reader["group_name"];
                                ug.permission = (string)reader["permission"];
                              
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

            return ug;
        }

        public async Task<ErrorDBO> AddUpdateUserGroup(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            UserGroupBDO ug = new UserGroupBDO();
            int last_id = 0;


            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    ug = JsonConvert.DeserializeObject<UserGroupBDO>(requestContent);
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "s_user_group_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@user_group_id", ug.id);
                            cmd.Parameters.AddWithValue("@group_name", ug.group_name);
                            cmd.Parameters.AddWithValue("@permission", ug.permission);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                last_id = (int)rd["user_group_id"];

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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - user_group_addup" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return e;
        }

        public async Task<ErrorDBO> DelUserGroup(int id)
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
                            cmd.CommandText = "s_user_group_del";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@user_group_id", id);
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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - user_group_del" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }

            return e;
        }

        public async Task<List<UserGroupBDO>> GetAllUserGroup()
        {
            List<UserGroupBDO> l_u = new List<UserGroupBDO>();

            //if (await Token.AuthenticatedCheck())
            //{
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_user_group_sel_all";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                while (await rd.ReadAsync())
                                {
                                    l_u.Add(new UserGroupBDO()
                                    {
                                        id = (int)rd["id"]
                                        ,
                                        group_name = (string)rd["group_name"]
                                        ,
                                        permission = (string)rd["permission"]
                                        
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