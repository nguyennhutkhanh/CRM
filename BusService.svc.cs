using System;
using System.ServiceModel.Web;
using System.Collections.Generic;

using System.IO;
using System.Threading.Tasks;
using WcfService.Model;
using WcfService.DAO;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BusService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BusService.svc or BusService.svc.cs at the Solution Explorer and start debugging.
    public class BusService : IBusService
    {
        public async Task<ErrorDBO> bus_addup(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            BusDAO b = new BusDAO();

            //var headers = WebOperationContext.Current.IncomingRequest.Headers;
            //var header_user_id = headers["user_id"];
            //int _user_id = header_user_id == null ? 0 : Convert.ToInt32(header_user_id);

            //var header_co_id = headers["co_id"];
            //int _co_id = header_co_id == null ? 0 : Convert.ToInt32(header_co_id);

            return await b.AddUpBus(stream);
        }

        public async Task<BusBDO> bus_sel()
        {
            BusDAO b = new BusDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await b.GetBus(_id);
        }

        public async Task<ErrorDBO> bus_del()
        {
            BusDAO b = new BusDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await b.DelBus(_id);
        }

        public async Task<List<BusBDO>> bus_sel_all()
        //public int bus_sel_all()
        {
            BusDAO b = new BusDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await b.GetAllBus(_id);
            //return  _id;
        }

        //public async Task<List<BusBDO>> bus_sel_all()
        //{
        //    List<BusBDO> l_b = new List<BusBDO>();

        //    var headers = WebOperationContext.Current.IncomingRequest.Headers;
        //    var header_id = headers["id"];
        //    int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                cmd.CommandText = "ws_bus_sel_all";
        //                cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@co_id", _id);

        //                cmd.Connection = conn;
        //                await conn.OpenAsync();

        //                using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
        //                {
        //                    while (rd.HasRows)
        //                    {
        //                        l_b.Add(new BusBDO()
        //                        {
        //                            bus_id = Convert.ToInt32(rd["bus_id"])
        //                            ,bus_no = rd["bus_no"].ToString()
        //                            ,co_id = Convert.ToInt32(rd["co_id"])
        //                            ,driver_id = Convert.ToInt32(rd["driver_id"])
        //                            ,num_seat = Convert.ToInt32(rd["num_seat"])
        //                            ,bus_type = Convert.ToInt16(rd["bus_type"])
        //                        });
        //                    }
        //                    rd.Close();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logs.writeToLogFile(ex.ToString());
        //    }
        //    return l_b;
        //}
    }
}
