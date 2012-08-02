using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRS.Core;
using SignalR;
using SignalR.Hubs;

namespace Loveboat.Hubs
{
    public class ViewModelEventDispatcher : IViewModelEventDispatcher
    {
        private readonly IConnectionManager _connectionManager;

        public ViewModelEventDispatcher(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void  Change<TDto>(TDto dto, ViewModelUpdateType viewModelUpdateType)
        {
            _connectionManager.GetHubContext<ViewModelHub>().Clients.viewModelChange(typeof(TDto).Name, dto, viewModelUpdateType);
        }
    }

    public class ViewModelHub : Hub
    {
        public void Join(string[] viewModelTypes)
        {
            if (viewModelTypes != null)
                foreach (var type in viewModelTypes)
                    Groups.Add(Context.ConnectionId, type);

        }
    }
}