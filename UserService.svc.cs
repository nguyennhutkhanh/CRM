using System;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WcfService.Model;
using WcfService.DAO;
using WcfService.Common;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserService.svc or UserService.svc.cs at the Solution Explorer and start debugging.
    public class UserService : IUserService
    {
        

        public async Task<ErrorDBO> user_addup(Stream stream)
        {
            UserDAO u = new UserDAO();
            return await u.AddUpdateUser(stream);
        }

        //public async Task<UserBDO> user_sel(int id)
        public async Task<UserBDO> user_sel()
        {
            UserDAO u = new UserDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await u.GetUser(_id);
        }

        public async Task<ErrorDBO> user_del()
        {
            UserDAO u = new UserDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await u.DelUser(_id);
        }
    
        public async Task<List<UserBDO>> user_sel_all()
        {
           UserDAO u = new UserDAO();
            //var headers = WebOperationContext.Current.IncomingRequest.Headers;
            //var header_id = headers["id"];
            //int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await u.GetAllUser();//(_id);
           
        }

        public async Task<UserBDO> user_login()
        {
            UserDAO u = new UserDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;

            var header = headers.Get("Authorization");
            var hashAutenticacao = header.Substring(6); //start of "Basic "
            var usernamePwd = Encoding.UTF8.GetString(Convert.FromBase64String(hashAutenticacao));

            var username = usernamePwd.Substring(0, usernamePwd.IndexOf(":"));
            var pwd = usernamePwd.Substring(usernamePwd.IndexOf(":") + 1);

            return await u.LoginUser(username, pwd);
        }
    }
}
