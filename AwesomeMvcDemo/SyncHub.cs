using Microsoft.AspNet.SignalR;

namespace AwesomeMvcDemo
{
    public class SyncHub : Hub
    {
        public void Send(string cid, string gridId, string key, string act, string group)
        {
            Clients.All.broadcastMessage(cid, gridId, key, act, group);
        }
    }
}