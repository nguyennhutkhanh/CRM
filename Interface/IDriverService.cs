using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using WcfService.Model;


namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDriverService" in both code and config file together.
    [ServiceContract]
    public interface IDriverService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "driver_addup")]
        Task<ErrorDBO> driver_addup(Stream stream);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "driver_sel")]
        Task<DriverBDO> driver_sel();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "driver_sel_all")]
        Task<List<DriverBDO>> driver_sel_all();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "DELETE",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "driver_del")]
        Task<ErrorDBO> driver_del();
    }
}
