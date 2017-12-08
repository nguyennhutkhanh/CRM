using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Threading.Tasks;

using WcfService.Model;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUtilService" in both code and config file together.
    [ServiceContract]
    public interface IUtilService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "/product_uploadfile")]
        Task<notify_image_upload> ProductUploadFile(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "/user_uploadfile")]
        Task<notify_image_upload> UserUploadFile(Stream stream);
    }
}
