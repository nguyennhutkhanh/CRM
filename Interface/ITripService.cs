using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using WcfService.Model;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITripService" in both code and config file together.
    [ServiceContract]
    public interface ITripService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "trip_addup")]
        Task<ErrorDBO> trip_addup(Stream stream);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "trip_sel")]
        Task<TripBDO> trip_sel();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "trip_sel_all")]
        Task<List<TripResponseBDO>> trip_sel_all();
    }
}
