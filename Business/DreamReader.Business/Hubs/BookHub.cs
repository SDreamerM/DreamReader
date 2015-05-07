using Microsoft.AspNet.SignalR;

namespace DreamReader.Business.Hubs
{
    public class BookHub : Hub
    {
        public static void BookSectionRowProcessed(string message, decimal processedPercentage)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<BookHub>();
            if (hubContext != null) hubContext.Clients.All.bookSectionRowProcessed(message, processedPercentage);
        }

        public static void BookSectionProcessed(string message, decimal processedPercentage)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<BookHub>();
            if (hubContext != null) hubContext.Clients.All.bookSectionProcessed(message, processedPercentage);
        }
    }
}