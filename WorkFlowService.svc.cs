using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WcfService.DAO;
using WcfService.Model;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WorkFlowService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WorkFlowService.svc or WorkFlowService.svc.cs at the Solution Explorer and start debugging.
    public class WorkFlowService : IWorkFlowService
    {
        public async Task<ErrorDBO> workflow_addup(Stream stream)
        {
            WorkFlowDAO wf = new WorkFlowDAO();
            return await wf.AddUpdateWorkFlow(stream);
        }


        public async Task<WorkFlowBDO> workflow_sel()
        {
            WorkFlowDAO wf= new WorkFlowDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await wf.GetWorkFlow(_id);
        }

        public async Task<ErrorDBO> workflow_del()
        {
            WorkFlowDAO wf = new WorkFlowDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await wf.DelWorkFlow(_id);
        }

        public async Task<List<WorkFlowBDO>> workflow_sel_all()
        {
            WorkFlowDAO wf = new WorkFlowDAO();
            //var headers = WebOperationContext.Current.IncomingRequest.Headers;
            //var header_id = headers["id"];
            //int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await wf.GetAllWorkFlow();//(_id);

        }
    }
}
