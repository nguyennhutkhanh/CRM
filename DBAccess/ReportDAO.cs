using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WcfService.Common;
using WcfService.Model;


namespace WcfService.DAO
{
    public class ReportDAO
    {
        string connectionString = ConfigurationManager.AppSettings["cm_conn"].ToString();

       
        public async Task<ReportBDO> GetReport(int id)
        {
            ReportBDO r = new ReportBDO();

            //if (await Token.AuthenticatedCheck())
            //{
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_report_sel";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Connection = conn;
                        await conn.OpenAsync();

                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                while (await rd.ReadAsync())
                                {
                                    r.id = (int)rd["id"];
                                    r.report_name = (string)rd["report_name"];
                                }
                            }
                            await rd.NextResultAsync();
                            r.report_list = new List<Report_detail>();
                            while (await rd.ReadAsync())
                            {
                                r.report_list.Add(new Report_detail()
                                {
                                    id = (int)rd["id"]
                                    ,name = (string)rd["name"]
                                    
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
            //}
            //else
            //{
            //    ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - user_sel_all" };
            //    throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            //}

            return r;
        }

        public async Task<List<ReportBDO>> GetAllReports()
        {
            List<ReportBDO> l_r = new List<ReportBDO>();

            //if (await Token.AuthenticatedCheck())
            //{
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "s_report_sel_all";
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
                                    l_r.Add(new ReportBDO()
                                    {
                                        id = (int)rd["id"]
                                        ,report_name = (string)rd["report_name"]
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

            return l_r;
        }
    }
}