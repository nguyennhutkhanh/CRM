using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Threading.Tasks;
using WcfService.Model;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserService" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "user_addup")]
        Task<ErrorDBO> user_addup(Stream stream);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "user_sel")]
        Task<UserBDO> user_sel();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "DELETE",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "user_del")]
        Task<ErrorDBO> user_del();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "user_sel_all")]
        Task<List<UserBDO>> user_sel_all();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "user_login")]
        Task<UserBDO> user_login();
    }
}
