using System;
using System.Net;
using System.ServiceModel.Web;

using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using WcfService.Model;

using WcfService.Common;

namespace WcfService.DAO
{
    public class TripDAO
    {
        string connectionString = ConfigurationManager.AppSettings["lxd_vexe_conn"].ToString();

        public async Task<ErrorDBO> AddUpTrip(Stream stream, int id)
        {
            ErrorDBO e = new ErrorDBO();
            TripBDO t = new TripBDO();
            int last_id = 0;

            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    string requestContent = reader.ReadToEnd();
                    requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

                    t = JsonConvert.DeserializeObject<TripBDO>(requestContent);

                    DataTable dt_dis_lock_seat = new DataTable();
                    dt_dis_lock_seat.Columns.Add("seat_id", typeof(int));
                    dt_dis_lock_seat.Columns.Add("co_id", typeof(int));
                    
                    if (t.lock_seat_list.Count > 0)
                    {
                        for (int i = 0; i < t.lock_seat_list.Count; i++)
                        {
                            dt_dis_lock_seat.Rows.Add(  
                                                            t.lock_seat_list[i].seat_id
                                                            ,t.lock_seat_list[i].co_id
                                                     );
                        }
                    }

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_trip_addup_and_generate";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@tr_id", t.tr_id);
                            cmd.Parameters.AddWithValue("@tr_name", t.tr_name);
                            cmd.Parameters.AddWithValue("@tr_desc", t.tr_description);
                            cmd.Parameters.AddWithValue("@tr_cat_id", t.tr_cat_id);
                            cmd.Parameters.AddWithValue("@co_id", id);
                            cmd.Parameters.AddWithValue("@price", t.price);
                            cmd.Parameters.AddWithValue("@departure_hour", t.departure_hour);
                            cmd.Parameters.AddWithValue("@arrival_hours", t.arrival_hour);
                            cmd.Parameters.AddWithValue("@apply_from", t.apply_from);
                            cmd.Parameters.AddWithValue("@apply_to", t.apply_to);
                            cmd.Parameters.AddWithValue("@frequency", t.frequency);
                            cmd.Parameters.AddWithValue("@days_of_week", t.days_of_week);
                            cmd.Parameters.AddWithValue("@bus_id", t.bus_id);
                            cmd.Parameters.AddWithValue("@date_sale_before", t.days_sale_before);
                            cmd.Parameters.AddWithValue("@status", t.status);
                            cmd.Parameters.AddWithValue("@user_id", t.user_id);

                            cmd.Parameters.AddWithValue("@locked_seats_list", dt_dis_lock_seat);
                            cmd.Parameters["@locked_seats_list"].SqlDbType = System.Data.SqlDbType.Structured;

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
                                //    e.message = "Succesful";
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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - trip_addup" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
           }
            
           return e;
        }

        public async Task<TripBDO> GetTrip(int id)
        {
            TripBDO t = new TripBDO();
            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_trip_sel";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@tr_id", id);
                            cmd.Connection = conn;
                            await conn.OpenAsync();

                            using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                            {
                                await rd.ReadAsync();
                                t.tr_id = (int)rd["tr_id"];
                                t.tr_name = (string)rd["name"];
                                t.co_id = (int)rd["co_id"];
                                t.price = (double)rd["price"];
                                t.departure_hour = (TimeSpan)rd["departure_hour"];
                                t.arrival_duration = (short)rd["arrival_hour"];
                                t.apply_from = (string)rd["apply_from"];
                                t.apply_to = (string)rd["apply_to"];
                                t.frequency = (short)rd["frequency"];
                                t.days_of_week = (string)rd["days_of_week"];
                                t.bus_id = (int)rd["bus_id"];
                                t.days_sale_before = (short)rd["days_sale_before"];
                                t.status = (short)rd["status"];
                                t.user_id = (int)rd["user_id"];
                                t.dis_method = (short)rd["dis_method"];
                                t.dis_seat_list = (string)rd["dis_seat_list"];
                                t.dis_before_abort = (short)rd["dis_before_abort"];

                               // await rd.NextResultAsync();
                               
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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - trip_sel" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return t;
        }

        public async Task<List<TripResponseBDO>> GetAllTrip(int id)
        {
            List<TripResponseBDO> l = new List<TripResponseBDO>();

            if (await Token.AuthenticatedCheck())
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "ws_trip_sel_all";
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
                                            l.Add(new TripResponseBDO()
                                            {
                                                tr_id = (int)rd["tr_id"]
                                                ,tr_name = (string)rd["name"]
                                                ,co_id = (int)rd["co_id"]
                                                ,price = (double)rd["price"]
                                                //,departure_hour = rd["departure_hour"].ToString()
                                                //,arrival_duration = (short)rd["arrival_hour"]
                                                //,apply_from = (string)rd["apply_from"]
                                                //,apply_to = (string)rd["apply_to"]
                                                //,frequency = (short)rd["frequency"]
                                                //,days_of_week = (string)rd["days_of_week"]
                                                //,bus_id = (int)rd["bus_id"]
                                                //,days_sale_before = (short)rd["days_sale_before"]
                                                //,status = (short)rd["status"]
                                                //,user_id = (int)rd["user_id"]
                                                //,dis_method = (short)rd["dis_method"]
                                                //,dis_seat_list = (string)rd["dis_seat_list"]
                                                //,dis_before_abort = (short)rd["dis_before_abort"]
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
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - trip_sel_all" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return l;
        }
    }
}
