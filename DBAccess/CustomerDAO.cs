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
    public class CustomerDAO
    {
        string connectionString = ConfigurationManager.AppSettings["cm_conn"].ToString();

        public async Task<CustomerDBO> GetCustomer(int id)
        {
            CustomerDBO c = null;

            try
            {
                //    if (await Token.AuthenticatedCheck())
                //   {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_customer_sel";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cust_id", id);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();
                                c = new CustomerDBO();
                                c.id = id;
                                c.cust_first_name = (string)reader["cust_first_name"];
                                c.cust_last_name = (string)reader["cust_last_name"];
                                c.birth_date = (DateTime)reader["birth_date"];
                                c.mobile_1 = (string)reader["mobile_1"];
                                c.mobile_2 = (string)reader["mobile_2"];
                                c.mobile_3 = (string)reader["mobile_3"];
                                c.email = (string)reader["email"];
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

        public async Task<CustomerDBO> GetCustomerMobile(string mobile)
        {
            CustomerDBO c = null;

            try
            {
                //    if (await Token.AuthenticatedCheck())
                //   {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_customer_sel_mobile";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@mobile", mobile);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();
                                c = new CustomerDBO();
                                c.id = (int) reader["id"];
                                c.cust_first_name = (string)reader["cust_first_name"];
                                c.cust_last_name = (string)reader["cust_last_name"];
                                c.birth_date = (DateTime)reader["birth_date"];
                                c.mobile_1 = (string)reader["mobile_1"];
                                c.mobile_2 = (string)reader["mobile_2"];
                                c.mobile_3 = (string)reader["mobile_3"];
                                c.email = (string)reader["email"];
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

        public async Task<ErrorDBO> AddUpdateCustomer(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            CustomerDBO c = new CustomerDBO();
            int last_id = 0;


            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    c = JsonConvert.DeserializeObject<CustomerDBO>(requestContent);
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "s_customer_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@cust_id", c.id);
                            cmd.Parameters.AddWithValue("@first_name", c.cust_first_name);
                            cmd.Parameters.AddWithValue("@last_name", c.cust_last_name);
                            cmd.Parameters.AddWithValue("@birth_date", c.birth_date);
                            cmd.Parameters.AddWithValue("@mobile_1", c.mobile_1);
                            cmd.Parameters.AddWithValue("@mobile_2", c.mobile_2);
                            cmd.Parameters.AddWithValue("@mobile_3", c.mobile_3);
                            cmd.Parameters.AddWithValue("@email", c.email);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                last_id = (int)rd["cust_id"];

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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - cust_addup" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return e;
        }

        public async Task<ErrorDBO> DelCustomer(int id)
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
                            cmd.CommandText = "s_customer_del";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@cust_id", id);
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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - cust_del" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }

            return e;
        }

        public async Task<List<CustomerDBO>> GetAllCustomer()
        {
            List<CustomerDBO> l_c = new List<CustomerDBO>();

            //if (await Token.AuthenticatedCheck())
            //{
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_customer_sel_all";
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
                                    l_c.Add(new CustomerDBO()
                                    {
                                        id = (int)rd["id"]
                                        ,cust_first_name = (string)rd["cust_first_name"]
                                        ,cust_last_name = (string)rd["cust_last_name"]
                                        ,birth_date = (DateTime)rd["birth_date"]
                                        ,mobile_1 = (string)rd["mobile_1"]
                                        ,mobile_2 = (string)rd["mobile_2"]
                                        ,mobile_3 = (string)rd["mobile_3"]
                                        ,email = (string)rd["email"]
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

        public async Task<CustomerHistories> GetCustomerHistories(string mobile)
        {
            CustomerHistories c = null;

            try
            {
                //    if (await Token.AuthenticatedCheck())
                //   {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_customer_history_sel";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@mobile", mobile);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();
                                c = new CustomerHistories();
                                c.id = (int)reader["id"];
                                c.cust_first_name = (string)reader["cust_first_name"];
                                c.cust_last_name = (string)reader["cust_last_name"];
                                c.birth_date = (DateTime)reader["birth_date"];
                                c.mobile_1 = (string)reader["mobile_1"];
                                c.mobile_2 = (string)reader["mobile_2"];
                                c.mobile_3 = (string)reader["mobile_3"];
                                c.email = (string)reader["email"];
                            }
                            await reader.NextResultAsync();
                            c.cust_histories = new List<CallBDO>();
                            while (await reader.ReadAsync())
                            {
                                c.cust_histories.Add(new CallBDO()
                                {
                                    call_id = (int)reader["id"]
                                    ,call_no = (string) reader["call_no"]
                                    ,start_time = (DateTime) reader["start_time"]
                                    ,end_time = (DateTime) reader["end_time"]
                                    ,content = (string) reader["content"]
                                    ,note = (string) reader["note"]
                                    ,url_rec = (string) reader["url_rec"]
                                    ,status = (byte) reader["status"]                                
                                }
                               );
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
    }
}