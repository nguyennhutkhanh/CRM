using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Hosting;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using System.Net.Mail;
using System.Threading;
using Newtonsoft.Json;

using WcfService.Common;
using WcfService.Model;
using WcfService.DAO;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UtilService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UtilService.svc or UtilService.svc.cs at the Solution Explorer and start debugging.
    public class UtilService : IUtilService
    {

        public async Task<notify_image_upload> ProductUploadFile(Stream stream)
        {
            UtilDAO ud = new UtilDAO();
            return await ud.ProductUploadFile(stream);
        }

        public async Task<notify_image_upload> UserUploadFile(Stream stream)
        {
            UtilDAO ud = new UtilDAO();
            return await ud.UserUploadFile(stream);
        }

    }
}
