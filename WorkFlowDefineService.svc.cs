using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WcfService.DAO;
using WcfService.Model;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WorkFlowDefineService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WorkFlowDefineService.svc or WorkFlowDefineService.svc.cs at the Solution Explorer and start debugging.
    public class WorkFlowDefineService : IWorkFlowDefineService
    {
        public async Task<ErrorDBO> workflow_define_addup(Stream stream)
        {
            WorkFlowDefineDAO wfd = new WorkFlowDefineDAO();
            return await wfd.AddUpdateWorkFlowDefine(stream);
        }


        public async Task<WorkFlowDefineBDO> workflow_define_sel()
        {
            WorkFlowDefineDAO wf = new WorkFlowDefineDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await wf.GetWorkFlowDefine(_id);
        }

        public async Task<ErrorDBO> workflow_define_del()
        {
            WorkFlowDefineDAO wf = new WorkFlowDefineDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await wf.DelWorkFlowDefine(_id);
        }

        public async Task<List<WorkFlowDefineBDO>> workflow_define_sel_all()
        {
            WorkFlowDefineDAO wfd = new WorkFlowDefineDAO();
            //var headers = WebOperationContext.Current.IncomingRequest.Headers;
            //var header_id = headers["id"];
            //int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await wfd.GetAllWorkFlowDefine();//(_id);

        }

        public async Task<WorkFlowDefineBDO> workflow_define_sel_case()
        {
            WorkFlowDefineDAO wf = new WorkFlowDefineDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["case_id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await wf.GetWorkFlowDefineCase(_id);
        }
    }
}
