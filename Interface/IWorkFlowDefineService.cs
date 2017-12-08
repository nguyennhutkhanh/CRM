using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Threading.Tasks;
using WcfService.Model;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWorkFlowDefineService" in both code and config file together.
    [ServiceContract]
    public interface IWorkFlowDefineService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "workflow_define_addup")]
        Task<ErrorDBO> workflow_define_addup(Stream stream);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "workflow_define_sel")]
        Task<WorkFlowDefineBDO> workflow_define_sel();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "DELETE",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "workflow_define_del")]
        Task<ErrorDBO> workflow_define_del();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "workflow_define_sel_all")]
        Task<List<WorkFlowDefineBDO>> workflow_define_sel_all();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "workflow_define_sel_case")]
        Task<WorkFlowDefineBDO> workflow_define_sel_case();
    }
}
