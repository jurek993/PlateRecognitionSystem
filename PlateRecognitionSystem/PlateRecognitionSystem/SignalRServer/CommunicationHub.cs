using Microsoft.AspNet.SignalR;
using PlateRecognitionSystem.Database;
using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.SignalRServer
{
    public class CommunicationHub : Hub
    {
       private BoardDatabase _boardDatabase = new BoardDatabase();
        public void Heartbeat(System.Security.Principal.IPrincipal user) ////Testowa funkcja
        {
            Console.WriteLine("Hub Heartbeat\n");
           // Clients.All.heartbeat();
        }

        public void NameOfTheTable(string tableName)
        {
            Clients.All.nameOfTheTable(tableName);
            _boardDatabase.AddTableToDatabase(tableName, Context.ConnectionId);
        }


        public void SendHelloObject(HelloModel hello)
        {
            Console.WriteLine("Hub hello {0} {1}\n", hello.Molly, hello.Age);
            Clients.All.sendHelloObject(hello);
        }

        public void EntryGuestVehicleMessage(GuestBoardViewModel model)
        {

        }

        public void ExitGuestVehicleMessage(GuestBoardViewModel model)
        {

        }

        public void EntrySubscriptionVehicleMessage()
        {

        }

        public void ExitSubscriptionVehicleMessage()
        {

        }

        public void NormalMessage()
        {
            //TODO: ilość wolnego miejsca na parkinkgu 
        }

        public override Task OnConnected()
        {
            Console.WriteLine("Hub OnConnected {0}\n", Context.ConnectionId); //TODO: dodać do log4neta
            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine("Hub OnDisconnected {0}\n", Context.ConnectionId); //TODO: dodać do log4neta
            _boardDatabase.DeleteBoardFromDatabase(Context.ConnectionId);
            return (base.OnDisconnected(true));
        }

    }
}
