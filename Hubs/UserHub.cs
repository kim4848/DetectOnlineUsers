using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DetectUsers.Hubs {
    public class UserHub : Hub {

        static List<string> users = new List<string> ();
        public UserHub () {
            Debug.WriteLine ("test");
        }

        public async Task SendMessage (string user, string message) {

            await Clients.All.SendAsync ("ReceiveMessage", user, message);
        }

        public async Task PushUserList () {

            await Clients.All.SendAsync ("UserList", users.ToArray ());
        }

        public override async Task OnDisconnectedAsync (Exception exception) {

            string name = Context.User.Identity.Name;
            await SendMessage (name, "Disconnected");
            await base.OnDisconnectedAsync (exception);
        }
        public override async Task OnConnectedAsync () {

            string name = Context.User.Identity.Name;

            if (!users.Any (x => x == name))
                users.Add (name);
            await SendMessage (name, "Connected");
            await PushUserList ();
            await base.OnConnectedAsync ();
        }
    }
}