using System;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WcfService.Model;
using WcfService.DAO;
using WcfService.Common;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DriverService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DriverService.svc or DriverService.svc.cs at the Solution Explorer and start debugging.
    public class DriverService : IDriverService
    {
        public async Task<ErrorDBO> driver_addup(Stream stream)
        {
            DriverDAO d = new DriverDAO();
            return await d.AddUpDriver(stream);
        }

        public async Task<DriverBDO> driver_sel()
        {
            DriverDAO d = new DriverDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await d.GetDriver(_id);
        }

        public async Task<List<DriverBDO>> driver_sel_all()
        {
            DriverDAO d = new DriverDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await d.GetAllDriver(_id);
        }

        public async Task<ErrorDBO> driver_del()
        {
            DriverDAO d = new DriverDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await d.DelDriver(_id);
        }
    }
}
