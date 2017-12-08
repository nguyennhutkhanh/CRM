using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using WcfService.Model;

namespace WcfService.DAO
{
    public class ClientDAO
    {
        string connectionString = ConfigurationManager.AppSettings["lxd_vexe_conn"].ToString();

        public async Task<ErrorDBO> AddUpClient(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            ClientDBO  c = new ClientDBO();
            int code = 0;

            StreamReader reader = new StreamReader(stream);
            string requestContent = reader.ReadToEnd();
            requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

            c = JsonConvert.DeserializeObject<ClientDBO>(requestContent);

            //DataTable dt_util = new DataTable();
            //dt_util.Columns.Add("util_1", typeof(byte));
            //dt_util.Columns.Add("util_2", typeof(byte));
            //dt_util.Columns.Add("uti_3", typeof(byte));

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "ws_client_addup";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@client_id", c.client_id);
                    cmd.Parameters.AddWithValue("@full_name", c.full_name);
                    cmd.Parameters.AddWithValue("@mobile", c.mobile);
                    cmd.Parameters.AddWithValue("@email", c.email);
                    cmd.Parameters.AddWithValue("@address", c.address);
                    cmd.Parameters.AddWithValue("@gender", c.gender);
                    cmd.Parameters.AddWithValue("@DOB", c.DOB);
                    cmd.Parameters.AddWithValue("@co_id", c.co_id);

                    cmd.Connection = conn;
                    await conn.OpenAsync();

                    using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                    {
                        await rd.ReadAsync();
                        code = (int)rd["client_id"];
                    }
                    //if (await cmd.ExecuteNonQueryAsync() != 1)
                    //{
                    if (code > 0) { e.status = true; e.message = code.ToString(); }
                    else { e.status = false; e.message = "Fail"; }
                    //}
                }
                return e;
            }

        }

        public async Task<ClientDBO> GetClient(int id)
        {
            ClientDBO c = new ClientDBO();
            ClientTransDBO c_t = new ClientTransDBO();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "ws_client_sel";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@client_id", id);

                    cmd.Connection = conn;
                    await conn.OpenAsync();

                    using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                    {
                        await rd.ReadAsync();
                        c.client_id = (int)rd["client_id"];
                        c.full_name = (string)rd["full_name"];
                        c.mobile = (string)rd["mobile"];
                        c.email = (string)rd["email"];

                        await rd.NextResultAsync();
                        c.client_trans = new List<ClientTransDBO>();
                        if (rd.HasRows)
                        {
                            while (await rd.ReadAsync())
                            {
                                c.client_trans.Add(new ClientTransDBO()
                                    {
                                         datetime = (string)rd["create_date"]
                                         ,status = (bool)rd["status"]
                                         ,status_name = (string)rd["satus_name"]
                                         ,ticket_no = (string)rd["ticket_no"]
                                         ,ticket_price = (float)rd["ticket_price"]
                                });
                            }
                        }

                        rd.Close();
                    }

                }
            }
            return c;
        }

        public async Task<List<ClientDBO>> GetAllClient(int id)
        {
            List<ClientDBO> l_c = new List<ClientDBO>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "ws_client_sel_by_owner";
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
                                l_c.Add(new ClientDBO()
                                {
                                    client_id = (int)rd["client_id"]
                                    ,full_name = (string)rd["full_name"]
                                });
                            }
                            
                        }

                        rd.Close();
                    }
                }
            }
            return l_c;
        }
    }
}
