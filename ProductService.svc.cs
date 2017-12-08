using System;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WcfService.Model;
using WcfService.DAO;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProductService.svc or ProductService.svc.cs at the Solution Explorer and start debugging.
    public class ProductService : IProductService
    {
        public async Task<ProductBDO> product_sel()
        {
            ProductDAO p = new ProductDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await p.GetProduct(_id);
        }

        public async Task<ErrorDBO> product_addup(Stream stream)
        {
            ProductDAO c = new ProductDAO();
            return await c.AddUpdateProduct(stream);
        }

        public async Task<ErrorDBO> product_del()
        {
            ProductDAO p = new ProductDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await p.DelProduct(_id);
        }

        public async Task<List<ProductBDO>> product_sel_all()
        {
            ProductDAO p = new ProductDAO();          
            return await p.GetAllProduct();

        }
    }
}
