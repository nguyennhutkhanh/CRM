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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserGroupService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserGroupService.svc or UserGroupService.svc.cs at the Solution Explorer and start debugging.
    public class UserGroupService : IUserGroupService
    {
        public async Task<ErrorDBO> user_group_addup(Stream stream)
        {
            UserGroupDAO ug = new UserGroupDAO();
            return await ug.AddUpdateUserGroup(stream);
        }


        public async Task<UserGroupBDO> user_group_sel()
        {
            UserGroupDAO ug = new UserGroupDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await ug.GetUserGroup(_id);
        }

        public async Task<ErrorDBO> user_group_del()
        {
            UserGroupDAO ug = new UserGroupDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await ug.DelUserGroup(_id);
        }

        public async Task<List<UserGroupBDO>> user_group_sel_all()
        {
            UserGroupDAO u = new UserGroupDAO();
            //var headers = WebOperationContext.Current.IncomingRequest.Headers;
            //var header_id = headers["id"];
            //int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await u.GetAllUserGroup();//(_id);

        }
    }
}
