using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Threading.Tasks;
using WcfService.Model;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOwnerService" in both code and config file together.
    [ServiceContract]
    public interface IOwnerService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "owner_addup")]
        Task<ErrorDBO> owner_addup(Stream stream);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "owner_sel")]
        Task<OwnerBDO> owner_sel();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "owner_sel_by_owner")]
        Task<List<OwnerBDO>> owner_sel_by_owner();
    }
}
