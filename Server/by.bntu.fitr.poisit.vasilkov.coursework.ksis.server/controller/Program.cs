using Server.@by.bntu.fitr.poisit.vasilkov.coursework.ksis.server.model;

namespace Server
{
    internal class Program
    {
        const int port = 8001;
        const string adrress = "0.0.0.0";

        static void Main(string[] args)
        {
            TSPServer server = new TSPServer();
            server.Create(adrress, port);
            server.ShowInfoServer();
            server.StartServer();
        }
    }
}