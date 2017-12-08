using System;
using System.IO;
using System.Collections.Generic;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WcfService.Model;
using WcfService.DAO;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LocService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select LocService.svc or LocService.svc.cs at the Solution Explorer and start debugging.
    public class LocService : ILocService
    {
        public async Task<ErrorDBO> loc_addup(Stream stream)
        {
            LocDAO ld = new LocDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await ld.AddUpLocDrpPck(stream, _id);
        }

        public async Task<List<LocDrpPckBDO>> loc_sel_by_owner()
        {
            List<LocDrpPckBDO> l = new List<LocDrpPckBDO>();
            LocDAO ld = new LocDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await ld.GetAllLoc(_id);
        }
    }
}

    

