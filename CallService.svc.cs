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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CallService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CallService.svc or CallService.svc.cs at the Solution Explorer and start debugging.
    public class CallService : ICallService
    {
        public async Task<ErrorDBO> call_addup(Stream stream)
        {
            CallDAO c = new CallDAO();
            return await c.AddUpdateCall(stream);
        }

        public async Task<CallTotalStatusBBO> call_status_department()
        {
            CallDAO c = new CallDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await c.GetCallStatusDeparment(_id);
        }

        public async Task<List<CallBDO>> call_history()
        {
            CallDAO c = new CallDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["phone"];
            string _phone = header_id;

            return await c.GetCallHistoryTop5(_phone);
        }
        
        public async Task<List<CallBDO>> call_status()
        {
            CallDAO c = new CallDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["status"];
            int _id = header_id == null ? 1 : Convert.ToByte(header_id);

            return await c.GetCallStatus(_id);
        }

        public async Task<CallBDO> call_sel()
        {
            CallDAO c = new CallDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["status"];
            int _id = header_id == null ? 1 : Convert.ToByte(header_id);

            return await c.GetCall(_id);
        }

    }
}
