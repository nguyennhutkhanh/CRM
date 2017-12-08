using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WcfService.Model;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICaseTransService" in both code and config file together.
    [ServiceContract]
    public interface ICaseTransService
    {
        [OperationContract]
   
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "case_trans_addup")]
        Task<ErrorDBO> case_trans_addup(Stream stream);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "case_trans_sel")]
        //Task<UserBDO> user_sel(int id);
        Task<CaseTransBDO> case_trans_sel();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "DELETE",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "case_trans_del")]
        Task<ErrorDBO> case_trans_del();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "case_trans_sel_all")]
        Task<List<CaseTransBDO>> case_trans_sel_all();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "case_trans_history")]
        Task<List<CaseTransHistoryBDO>> case_trans_history();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "case_trans_detail")]
        Task<List<CaseTransBDO>> case_trans_detail();
    }
}
