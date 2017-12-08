using System;
using System.Collections.Generic;
using System.ServiceModel.Web;
using System.IO;
using System.Threading.Tasks;
using WcfService.Model;
using WcfService.DAO;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TripService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TripService.svc or TripService.svc.cs at the Solution Explorer and start debugging.
    public class TripService : ITripService
    {
        public async Task<ErrorDBO> trip_addup(Stream stream)
        {
            TripDAO t = new TripDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            //var header_user_id = headers["user_id"];
            //int _user_id = header_user_id == null ? 0 : Convert.ToInt32(header_user_id);

            var header_co_id = headers["id"];
            int _co_id = header_co_id == null ? 0 : Convert.ToInt32(header_co_id);

            return await t.AddUpTrip(stream, _co_id);
        }

        public async Task<TripBDO> trip_sel()
        {
            TripDAO t_d = new TripDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await t_d.GetTrip(_id);
        }

        public async Task<List<TripResponseBDO>> trip_sel_all()
        {
            TripDAO t = new TripDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await t.GetAllTrip(_id);
        }
    }
}
