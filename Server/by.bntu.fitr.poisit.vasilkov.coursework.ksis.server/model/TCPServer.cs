using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Server.@by.bntu.fitr.poisit.vasilkov.coursework.ksis.server.view;

namespace Server.by.bntu.fitr.poisit.vasilkov.coursework.ksis.server.model
{
    public class TSPServer
    {
        private static string IPaddress { get; set; }
        private static int port { get; set; }
        private static TcpListener Listener;

        public void Create(string adress, int ip_port)
        {
            IPaddress = adress;
            port = ip_port;
        }

        public void ShowInfoServer()
        {
            Printer.Print("IP-address server: " + IPaddress);
            Printer.Print("Port server: " + port + "\n");
        }

        public void StartServer()
        {
            try
            {
                Listener = new TcpListener(IPAddress.Parse(IPaddress), port);
                Listener.Start();
                Printer.Print("Waiting for connecting...");

                while (true)
                {
                    TcpClient client = Listener.AcceptTcpClient();
                    //Client object
                    ClientObject clientObject = new ClientObject(client);
                    Thread clientThread = new Thread(clientObject.Process);
                    clientThread.Start();
                }
            }
            catch (Exception e)
            {
                Printer.Print(e.Message);
            }
            finally
            {
                if (Listener != null) Listener.Stop();
            }
        }
    }
}