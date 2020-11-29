namespace Nemezida.Rationalizator.Web.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    using Nemezida.Rationalizator.Web.DataAccess;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly SystemDbContext _dbContext;

        public ChatHub(SystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SendMessage(string message, long to)
        {

        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
