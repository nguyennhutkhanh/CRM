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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CaseTransService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CaseTransService.svc or CaseTransService.svc.cs at the Solution Explorer and start debugging.
    public class CaseTransService : ICaseTransService
    {
        public async Task<ErrorDBO> case_trans_addup(Stream stream)
        {
            CaseTransDAO c = new CaseTransDAO();
            return await c.AddUpdateCaseTrans(stream);
        }

        //public async Task<UserBDO> user_sel(int id)
        public async Task<CaseTransBDO> case_trans_sel()
        {
            CaseTransDAO c = new CaseTransDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await c.GetCaseTrans(_id);
        }

        public async Task<ErrorDBO> case_trans_del()
        {
            CaseTransDAO c = new CaseTransDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await c.DelCaseTrans(_id);
        }

        public async Task<List<CaseTransBDO>> case_trans_sel_all()
        {
            CaseTransDAO c = new CaseTransDAO();
            //var headers = WebOperationContext.Current.IncomingRequest.Headers;
            //var header_id = headers["id"];
            //int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await c.GetAllCaseTrans();//(_id);

        }

        public async Task<List<CaseTransHistoryBDO>> case_trans_history()
        {
            CaseTransDAO c = new CaseTransDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_phone = headers["phone"];
            return await c.GetCaseHistory(header_phone);
        }

        public async Task<List<CaseTransBDO>> case_trans_detail()
        {
            CaseTransDAO c = new CaseTransDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_phone = headers["phone"];
            var case_id = headers["case_id"];
            return await c.GetCaseDetail(header_phone, Convert.ToInt32(case_id));
        }
    }
}
