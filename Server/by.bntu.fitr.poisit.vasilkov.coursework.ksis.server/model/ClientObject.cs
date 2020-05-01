using System;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Server.@by.bntu.fitr.poisit.vasilkov.coursework.ksis.server.util;
using Server.@by.bntu.fitr.poisit.vasilkov.coursework.ksis.server.view;

namespace Server.by.bntu.fitr.poisit.vasilkov.coursework.ksis.server.model
{
    public class ClientObject
    {
        private const int SIZE_DATA = 64;
        
        public TcpClient сlient;
        private Configuration _configuration = new Configuration();

        public ClientObject(TcpClient client)
        {
            сlient = client;
        }

        public void Process()
        {
            Thread thread = null;
            NetworkStream stream = null;
            try
            {
                thread = Thread.CurrentThread;
                stream = сlient.GetStream();
                byte[] data = new byte[SIZE_DATA];

                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (stream.DataAvailable);

                string msg = builder.ToString();
                Printer.Print("Request from customer: " + msg);
                
                msg = _configuration.IPConfig(msg);
                data = Encoding.Unicode.GetBytes(msg);
                stream.Write(data, 0, data.Length);
                
            }
            catch (Exception e)
            {
                Printer.Print(e.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (сlient != null)
                    сlient.Close();

                thread.Abort();
            }
        }
    }
}