﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Toggl.Phoebe.Analytics;
using Toggl.Phoebe.Data.DataObjects;
using Toggl.Phoebe.Data.Utils;
using Toggl.Phoebe.Data.ViewModels;
using XPlatUtils;

namespace Toggl.Phoebe.Data.ViewModels
{
    public class ClientListViewModel : IDisposable
    {
        ClientListViewModel ()
        {
            ServiceContainer.Resolve<ITracker> ().CurrentScreen = "Select Client";
        }

        public static async Task<ClientListViewModel> Init (Guid workspaceId)
        {
            var vm = new ClientListViewModel ();
            vm.ClientDataCollection = new ObservableRangeCollection<ClientData> ();

            var store = ServiceContainer.Resolve<IDataStore> ();
            var clients = await store.Table<ClientData> ()
                          .Where (r => r.DeletedAt == null && r.WorkspaceId == workspaceId)
                          .ToListAsync();

            vm.Sort (clients);
            vm.ClientDataCollection.AddRange (clients);
            return vm;
        }

        public void Dispose ()
        {
        }

        public ObservableRangeCollection<ClientData> ClientDataCollection { get; set;}

        private void Sort (List<ClientData> clients)
        {
            clients.Sort ((a, b) => String.Compare (
                              a.Name ?? String.Empty,
                              b.Name ?? String.Empty,
                              StringComparison.OrdinalIgnoreCase
                          ));
        }
    }
}
