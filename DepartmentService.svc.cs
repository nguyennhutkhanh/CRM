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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DepartmentService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DepartmentService.svc or DepartmentService.svc.cs at the Solution Explorer and start debugging.
    public class DepartmentService : IDepartmentService
    {
        
        public async Task<ErrorDBO> department_addup(Stream stream)
        {
            DepartmentDAO d = new DepartmentDAO();
            return await d.AddUpdateDepartment(stream);
        }


        public async Task<DepartmentBDO> department_sel()
        {
            DepartmentDAO d = new DepartmentDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await d.GetDepartment(_id);
        }

        public async Task<ErrorDBO> department_del()
        {
            DepartmentDAO d = new DepartmentDAO();

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await d.DelDepartment(_id);
        }

        public async Task<List<DepartmentBDO>> department_sel_all()
        {
            DepartmentDAO d = new DepartmentDAO();
            //var headers = WebOperationContext.Current.IncomingRequest.Headers;
            //var header_id = headers["id"];
            //int _id = header_id == null ? 0 : Convert.ToInt32(header_id);
            return await d.GetAllDepartment();//(_id);

        }
    }
}
