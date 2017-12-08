using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using Newtonsoft.Json;

using WcfService.Model;

using WcfService.Common;

namespace WcfService.DBAccess
{
    public class ModuleDAO
    {
        string connectionString = ConfigurationManager.AppSettings["lxd_vexe_conn"].ToString();

        public async Task<ErrorDBO> AddUpModule(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            ModuleBDO m = new ModuleBDO();
            int last_id = 0;
            try
            {
                if (await Token.AuthenticatedCheck())
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    m = JsonConvert.DeserializeObject<ModuleBDO>(requestContent);

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_module_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id", m.id);
                            cmd.Parameters.AddWithValue("@module_name", m.module_name);
                            cmd.Parameters.AddWithValue("@role_id", m.role_id);
                            cmd.Parameters.AddWithValue("@allow", m.allow);
                            cmd.Parameters.AddWithValue("@user_id", m.user_id);
                            cmd.Parameters.AddWithValue("@is_admin", m.is_admin);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                last_id = (int)rd["last_id"];

                                if (last_id > 0) { e.status = true; e.message = last_id.ToString(); }
                                else { e.status = false; e.message = "Fail"; }
                            }
                        }
                    }
                }
                else
                {
                    ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - module_addup" };
                    throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
                }
            }
            catch (Exception ex)
            {
                Logs.writeToLogFile(ex.ToString());
            }
                
            return e;
        }
    }
}