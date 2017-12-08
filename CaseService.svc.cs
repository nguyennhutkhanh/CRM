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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CaseService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CaseService.svc or CaseService.svc.cs at the Solution Explorer and start debugging.
    public class CaseService : ICaseService
    {
        public async Task<ErrorDBO> case_addup(Stream stream)
        {
            CaseDAO c = new CaseDAO();
            return await c.AddUpdateCase(stream);
        }

        //public async Task<UserBDO> user_sel(int id)
        public async Task<CaseBDO> case_sel()
        {
            CaseDAO c = new CaseDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await c.GetCase(_id);
        }

        public async Task<ErrorDBO> case_del()
        {
            CaseDAO c = new CaseDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await c.DelCase(_id);
        }

        public async Task<List<CaseBDO>> case_sel_all()
        {
            CaseDAO c = new CaseDAO();
            //var headers = WebOperationContext.Current.IncomingRequest.Headers;
            //var header_id = headers["id"];
            //int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await c.GetAllCase();//(_id);

        }
    }
}
