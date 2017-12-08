using System;
using System.IO;
using System.Collections.Generic;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WcfService.Model;
using WcfService.DAO;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TripCatService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TripCatService.svc or TripCatService.svc.cs at the Solution Explorer and start debugging.
    public class TripCatService : ITripCatService
    {
        public async Task<ErrorDBO> trip_cat_addup(Stream stream)
        {
            ErrorDBO e = new ErrorDBO();
            TripCatDAO t_c = new TripCatDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await t_c.AddUpTrip_Cat(stream, _id);
        }

        public async Task<TripCatBDO> trip_cat_sel()
        {
            TripCatDAO t_c = new TripCatDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await t_c.GetTrip_cat(_id);
        }

        public async Task<List<TripCatBDO>> trip_cat_sel_all()
        {
            TripCatDAO t_c = new TripCatDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await t_c.GetAllTrip_Cat(_id);
        }
    }
}
