using Microsoft.AspNet.SignalR;
using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.SignalRServer
{
    public class SendDataToBoards
    {
        public void GuestData(GuestBoardViewModel model)
        {
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<CommunicationHub>();
                foreach (var board in model.Boards)
                {
                    hubContext.Clients.Client(board.Token).EntryGuestVehicleMessage(model);
                    Console.WriteLine("Server Sending GuestBoardViewModel to: " + board.FunctionName +" " +board.Token);
                }
                
        }
    }
}
