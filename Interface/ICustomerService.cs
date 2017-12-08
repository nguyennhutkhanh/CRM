
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WcfService.Model;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICustomerService" in both code and config file together.
    [ServiceContract]
    public interface ICustomerService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "customer_addup")]
        Task<ErrorDBO> customer_addup(Stream stream);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "customer_sel")]        
        Task<CustomerDBO> customer_sel();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "customer_sel_mobile")]
        Task<CustomerDBO> customer_sel_mobile();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "DELETE",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "customer_del")]
        Task<ErrorDBO> customer_del();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "customer_sel_all")]
        Task<List<CustomerDBO>> customer_sel_all();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "customer_histories_sel")]
        Task<CustomerHistories> customer_histories_sel();
    }
}
