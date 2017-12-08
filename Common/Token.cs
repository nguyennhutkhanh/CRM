using System;
using System.Configuration;
using System.ServiceModel.Web;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WcfService.Common
{
    public class Token
    {
        static string connectionString = ConfigurationManager.AppSettings["cm_conn"].ToString();

        static public async Task<bool> AuthenticatedCheck()
        {
            bool v_result = false;

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_token = headers["token"];
            var header_outlet_id = headers["co_id"];
            int _outlet_id = header_outlet_id == null ? 0 : Convert.ToInt32(header_outlet_id);

            if (header_token != null)
            {
                //using sp_WS_valid_api_key
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "s_valid_api_key";

                        cmd.Parameters.AddWithValue("@api_key", header_token.ToString());
                        cmd.Connection = conn;

                        await conn.OpenAsync();

                        object _object = await cmd.ExecuteScalarAsync();

                        if (_object != null)
                        {
                            int v_outlet_id = Convert.ToInt32(_object.ToString());
                            //v_result = v_outlet_id == _outlet_id;
                            v_result = true;
                        }
                    }

                    conn.Close();
                }

            }
            else v_result = false;

            return v_result;
        }
    }
}
