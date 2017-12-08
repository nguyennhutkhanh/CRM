using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using WcfService.Model;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILocService" in both code and config file together.
    [ServiceContract]
    public interface ILocService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "loc_addup")]
        Task<ErrorDBO> loc_addup(Stream stream);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "loc_sel_by_owner")]
        Task<List<LocDrpPckBDO>> loc_sel_by_owner();
    }
}
