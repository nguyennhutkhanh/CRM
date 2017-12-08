using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WcfService.Model;
using WcfService.DAO;
using WcfService.Common;
using System;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CustomerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CustomerService.svc or CustomerService.svc.cs at the Solution Explorer and start debugging.
    public class CustomerService : ICustomerService
    {
        public async Task<ErrorDBO> customer_addup(Stream stream)
        {
            CustomerDAO c = new CustomerDAO();
            return await c.AddUpdateCustomer(stream);
        }

        //public async Task<UserBDO> user_sel(int id)
        public async Task<CustomerDBO> customer_sel()
        {
            CustomerDAO c = new CustomerDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await c.GetCustomer(_id);
        }

        public async Task<CustomerDBO> customer_sel_mobile()
        {
            CustomerDAO c = new CustomerDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["mobile"];
            string mobile = header_id;
            return await c.GetCustomerMobile(mobile);
        }

        public async Task<ErrorDBO> customer_del()
        {
            CustomerDAO c = new CustomerDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await c.DelCustomer(_id);
        }

        public async Task<List<CustomerDBO>> customer_sel_all()
        {
            CustomerDAO c = new CustomerDAO();
            //var headers = WebOperationContext.Current.IncomingRequest.Headers;
            //var header_id = headers["id"];
            //int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await c.GetAllCustomer();//(_id);

        }

        public async Task<CustomerHistories> customer_histories_sel()
        {
            CustomerDAO c = new CustomerDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["mobile"];
            string mobile = header_id;
            return await c.GetCustomerHistories(mobile);
        }
    }
}
