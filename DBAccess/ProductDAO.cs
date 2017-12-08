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
using System.Data;

namespace WcfService.DAO
{
    public class ProductDAO
    {
        string connectionString = ConfigurationManager.AppSettings["cm_conn"].ToString();

        public async Task<ProductBDO> GetProduct(int id)
        {
            ProductBDO p = new ProductBDO();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "s_product_sel";
                        cmd.Parameters.AddWithValue("@product_id", id);
                        cmd.Connection = conn;
                        await conn.OpenAsync();
                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                while (await rd.ReadAsync())
                                {
                                    p.product_id = (int)rd["product_id"];
                                    p.name = rd["name"].ToString();
                                    p.desc = rd["desc"].ToString();
                                    p.category_id = (int)rd["category_id"];
                                    p.product_code = rd["product_code"].ToString();
                                    p.barcode = rd["barcode"].ToString();
                                    p.price = (double)rd["price"];
                                    p.in_store = (bool)rd["in_store"];
                                    p.is_discount = (bool)rd["is_discount"];
                                    p.photo = rd["photo"].ToString();
                                    
                                    //p.unit_id = Convert.ToInt32(rd["unit_id"]);
                                    //p.min_stock = Convert.ToInt32(rd["min_stock"]);
                                    //p.begin_date = rd["begin_time"].ToString();
                                    //p.end_date = rd["end_time"].ToString();
                                    //p.active = Convert.ToBoolean(rd["active"]);
                                }
                                //await rd.NextResultAsync();
                                //p.photos = new List<P0056>();
                                //while (await rd.ReadAsync())
                                //{
                                //    p.photos.Add(new P0056()
                                //    {
                                //        id = Convert.ToInt32(rd["id"])              ,
                                //        url = rd["url"].ToString()
                                //    });
                                //}
                                ////get price
                                //await rd.NextResultAsync();
                                //p.prices = new List<P0055>();
                                //while (await rd.ReadAsync())
                                //{
                                //    p.prices.Add(new P0055()
                                //    {
                                //        product_price_code = rd["product_price_code"].ToString()
                                //                        //,to_date = rd["to_date"].ToString()
                                //                        //,from_date = rd["from_date"].ToString()
                                //                        ,
                                //        product_price_name = rd["product_price_name"].ToString()
                                //                        ,
                                //        discount = rd["discount"] == null ? 0 : Convert.ToDouble(rd["discount"])
                                //                        ,
                                //        price = rd["price"] == null ? 0 : Convert.ToDouble(rd["price"])
                                //                        ,
                                //        status = Convert.ToBoolean(rd["status"])
                                //                        ,
                                //        product_price_id = Convert.ToInt32(rd["product_price_id"])
                                //    }
                                //                );
                                //}
                                rd.Close();
                                rd.Dispose();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logs.writeToLogFile(ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
            return p;
        }

        public async Task<ErrorDBO> AddUpdateProduct(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();

            //List<error> myList = new List<error>();
            ProductBDO p = new ProductBDO();

            StreamReader reader = new StreamReader(stream);
            string requestContent = reader.ReadToEnd();
            requestContent = Format.Stream_JSON.StreamToJSON(requestContent);

            p = JsonConvert.DeserializeObject<ProductBDO>(requestContent);

            //DataTable _dt = new DataTable();
            //_dt.Columns.Add("product_id", typeof(int));
            //_dt.Columns.Add("product_price_code", typeof(string));
            //_dt.Columns.Add("product_price_name", typeof(string));
            //_dt.Columns.Add("price", typeof(float));
            //_dt.Columns.Add("discount", typeof(float));
            ////_dt.Columns.Add("from_date", typeof(DateTime));
            ////_dt.Columns.Add("to_date", typeof(DateTime));
            //_dt.Columns.Add("status", typeof(bool));

            //if (p.prices.Count > 0)
            //{
            //    for (int i = 0; i < p.prices.Count; i++)
            //    {
            //        _dt.Rows.Add(p.prices[i].product_id
            //                        , p.prices[i].product_price_code
            //                        , p.prices[i].product_price_name
            //                        , p.prices[i].price
            //                        , p.prices[i].discount
            //                        //,p.prices[i].from_date
            //                        //,p.prices[i].to_date
            //                        , p.prices[i].status
            //                     );
            //    }
            //}

            //string list_outlet_id = string.Empty;

            //if (p.product_applying.Count > 0)
            //{
            //    for (int i = 0; i < p.product_applying.Count; i++)
            //        list_outlet_id += p.product_applying[i].outlet_id + ";";
            //    list_outlet_id = list_outlet_id.Substring(0, list_outlet_id.Length - 1);
            //}

            if (await Token.AuthenticatedCheck())
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "s_product_addup";
                            cmd.Parameters.AddWithValue("@product_id", p.product_id);
                            //cmd.Parameters.AddWithValue("@outlet_id", p.outlet_id);
                            cmd.Parameters.AddWithValue("@product_code", p.product_code);
                            cmd.Parameters.AddWithValue("@name", p.name);
                            cmd.Parameters.AddWithValue("@desc", p.desc);
                            cmd.Parameters.AddWithValue("@category_id", p.category_id);
                            cmd.Parameters.AddWithValue("@in_store", p.in_store);
                            cmd.Parameters.AddWithValue("@is_discount", p.is_discount);

                            cmd.Parameters.AddWithValue("@barcode", p.barcode);
                            //cmd.Parameters.AddWithValue("@hot_pro", p.hot_pro);
                            //cmd.Parameters.AddWithValue("@unit_id", p.unit_id);
                            //cmd.Parameters.AddWithValue("@min_stock", p.min_stock);
                            //cmd.Parameters.AddWithValue("@scale", p.scale);
                            //cmd.Parameters.AddWithValue("@ask_price", p.ask_price);
                            //cmd.Parameters.AddWithValue("@active", p.active);
                            //cmd.Parameters.AddWithValue("@old_price", p.old_price);
                            cmd.Parameters.AddWithValue("@price", p.price);
                            cmd.Parameters.AddWithValue("@photo", p.photo);
                            //cmd.Parameters.AddWithValue("@begin_date", Convert.ToDateTime(p.begin_date)); //recheck when variable is string
                            //cmd.Parameters.AddWithValue("@end_date", Convert.ToDateTime(p.end_date));
                            //cmd.Parameters.AddWithValue("@prices", _dt);
                            //cmd.Parameters["@prices"].SqlDbType = SqlDbType.Structured;

                            //cmd.Parameters.AddWithValue("@list_outlet_id", list_outlet_id);
                            cmd.Parameters.AddWithValue("@last_product_id", SqlDbType.Int);
                            cmd.Parameters["@last_product_id"].Direction = ParameterDirection.Output;

                            cmd.Connection = conn;

                            await conn.OpenAsync();

                            await cmd.ExecuteNonQueryAsync();

                            e.status = true;
                            e.message = cmd.Parameters["@last_product_id"].Value.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logs.writeToLogFile(ex.ToString());
                        e.status = false;
                        e.message = ex.ToString();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return e;
        } 

        public async Task<ErrorDBO> DelProduct(int id)
        {
            ErrorDBO e = new ErrorDBO();         

            if (await Token.AuthenticatedCheck())
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "s_product_del";
                            cmd.Parameters.AddWithValue("@product_id", id);
                            cmd.Connection = conn;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            e.status = true;
                            e.message = "Successfull";
                        }

                    }
                    catch (Exception ex)
                    {
                        Logs.writeToLogFile(ex.ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return e;
        }

        public async Task<List<ProductBDO>> GetAllProduct()
        {
            List<ProductBDO> p = new List<ProductBDO>();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
          
            var header_page_index = headers["page_index"];
            int _page_index = header_page_index == null ? 0 : Convert.ToInt32(header_page_index);


            var header_page_size = headers["page_size"];
            int _page_size = header_page_size == null ? 0 : Convert.ToInt32(header_page_size);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "s_product_sel_all_pagewise";                        
                        cmd.Parameters.AddWithValue("@page_index", _page_index);
                        cmd.Parameters.AddWithValue("@page_size", _page_size);
                        cmd.Connection = conn;
                        await conn.OpenAsync();
                        using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                while (await rd.ReadAsync())
                                {
                                    p.Add(new ProductBDO()
                                    {
                                        //row_id = Convert.ToInt64(rd["RowNumber"])
                                        //,
                                        product_id = Convert.ToInt32(rd["product_id"])
                                        ,name = rd["name"].ToString()
                                        ,photo = rd["photo"].ToString()
                                        ,price = (double)rd["price"]
                                        ,category_id = Convert.ToInt32(rd["category_id"])
                                        //,category_name = rd["category_name"].ToString()
                                        ,in_store = Convert.ToBoolean(rd["in_store"])
                                        ,is_discount = Convert.ToBoolean(rd["is_discount"])
                                        ,desc = rd["desc"].ToString()
                                    });
                                }
                            }
                            rd.Close();
                            rd.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logs.writeToLogFile(ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }

            return p;
        }

    }
}