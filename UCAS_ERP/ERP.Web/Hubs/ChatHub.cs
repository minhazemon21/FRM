using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace IMS.Web.Hubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        public void Send(string firstMessage, object secondMessage)
        {
            Clients.All.sendMessage(firstMessage, secondMessage);
        }
    }
}
