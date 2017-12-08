using System;
using System.ServiceModel.Web;
using System.Collections.Generic;

using System.IO;
using System.Threading.Tasks;
using WcfService.Model;
using WcfService.DAO;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OwnerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OwnerService.svc or OwnerService.svc.cs at the Solution Explorer and start debugging.
    public class OwnerService : IOwnerService
    {
        public async Task<ErrorDBO> owner_addup(Stream stream)
        {
            OwnerDAO o = new OwnerDAO();
            return await o.AddUpOwner(stream);
        }
        
        public async Task<OwnerBDO> owner_sel()
        {
            OwnerDAO o = new OwnerDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await o.GetOwner(_id);
        }

        public async Task<List<OwnerBDO>> owner_sel_by_owner()
        {
            OwnerDAO o = new OwnerDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await o.GetAllOwnerByOwner(_id);
        }
    }
}
