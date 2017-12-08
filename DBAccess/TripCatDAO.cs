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
    public class TripCatDAO
    {
        string connectionString = ConfigurationManager.AppSettings["lxd_vexe_conn"].ToString();

        public async Task<ErrorDBO> AddUpTrip_Cat(Stream stream, int id)
        {
            ErrorDBO e = new ErrorDBO();
            TripCatBDO t_c = new TripCatBDO();
            int last_id = 0;
          
            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    t_c = JsonConvert.DeserializeObject<TripCatBDO>(requestContent);

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_trip_category_addup";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@tr_cat_id", t_c.tr_cat_id);
                            cmd.Parameters.AddWithValue("@tr_cat_name", t_c.name);
                            cmd.Parameters.AddWithValue("@departure", t_c.departure);
                            cmd.Parameters.AddWithValue("@arrival", t_c.arrival);
                            cmd.Parameters.AddWithValue("@location_pickup", t_c.location_pickup);
                            cmd.Parameters.AddWithValue("@location_dropoff", t_c.location_dropoff);
                            cmd.Parameters.AddWithValue("@user_id", t_c.user_id);
                            cmd.Parameters.AddWithValue("@co_id", id);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                last_id = (int)rd["last_id"];
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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - trip_cat_addup" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
         
            return e;
        }

        public async Task<List<TripCatBDO>> GetAllTrip_Cat(int id)
        {
            List<TripCatBDO> t_c = new List<TripCatBDO>();
          
            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_trip_category_sel_all";
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
                                        t_c.Add(new TripCatBDO()
                                        {
                                            tr_cat_id = (int)rd["tr_cat_id"]
                                            ,name = (string)rd["name"]
                                            ,departure = (int)rd["departure"]
                                            ,arrival = (int)rd["arrival"]
                                            ,location_dropoff = (int)rd["location_dropoff"]
                                            ,location_pickup = (int)rd["location_pickup"]
                                            ,co_id = (int)rd["co_id"]
                                            ,user_id = (int)rd["create_user_id"]
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
            }
            else
            {
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - trip_cat_sel_all" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
          
            return t_c;
        }

        public async Task<TripCatBDO> GetTrip_cat(int id)
        {
            TripCatBDO t_c = new TripCatBDO();
           
            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_trip_category_sel";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@tr_cat_id", id);

                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                if (rd.HasRows)
                                {
                                    await rd.ReadAsync();

                                    t_c.tr_cat_id = (int)rd["tr_cat_id"];
                                    t_c.name = (string)rd["name"];
                                    t_c.departure = (int)rd["departure"];
                                    t_c.arrival = (int)rd["arrival"];
                                    t_c.location_dropoff = (int)rd["location_dropoff"];
                                    t_c.location_pickup = (int)rd["location_pickup"];
                                    t_c.co_id = (int)rd["co_id"];
                                    t_c.user_id = (int)rd["create_user_id"];
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
            }
            else
            {
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - trip_cat_sel_all" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            
            return t_c;
        }
    }
}
