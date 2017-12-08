using System;
using System.IO;
using System.Threading.Tasks;
using System.ServiceModel.Web;

using WcfService.Model;
using WcfService.DAO;


namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TicketService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TicketService.svc or TicketService.svc.cs at the Solution Explorer and start debugging.
    public class TicketService : ITicketService
    {
        public async Task<ErrorDBO> ticket_registry(Stream stream)
        {
            TicketDAO t = new TicketDAO();
            return await t.AddUpTicket(stream);
        }

        public async Task<TicketResponseBDO> ticket_sel()
        {
            TicketDAO t = new TicketDAO();
            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header_id = headers["id"];
            int _id = header_id == null ? 0 : Convert.ToInt32(header_id);

            return await t.GetTicket(_id);
        }
    }
}
