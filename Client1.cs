using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client1
{
    class Program
    {
        private const int BUFFER_SIZE = 1024;
        private const int PORT_NUMBER = 8080;
        public static string str_server = "";
        public static string rev_IN = "";
        public static List<string> listRevServer = new List<string>(); 
        static ASCIIEncoding encoding = new ASCIIEncoding();

        static void Main(string[] args)
        {
            
            new Thread(toServer).Start();
            if (listRevServer.Count > 0)
            new Thread(toIN).Start();
            Console.Read();
        }
        public static void toIN()
        {
            try
            {
                //TcpClient client = new TcpClient();
                TcpClient client = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1000));
                // 1. connect
                client.Connect("127.0.0.1", PORT_NUMBER);
                Stream stream = client.GetStream();
                byte[] data = new byte[1024];
                Console.WriteLine("Connected to IN");
                while (true)
                {
                    Console.WriteLine("-----------------------------------------------------");
                    string str2 = "ban tin nhan duoc tu Server";
                    var reader = new StreamReader(stream);
                    var writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    // 2. send
                    if (listRevServer.Count > 0)
                    {
                        Console.WriteLine("str_server: " + listRevServer[0]);
                        writer.Write(Convert.ToByte(listRevServer[0]));
                    }
                    //writer.Write(Convert.ToByte(str2));
                    // 3. receive ban tin tu IN
                    int recv = stream.Read(data, 0, data.Length);
                    rev_IN = Encoding.ASCII.GetString(data, 0, recv);
                    Console.WriteLine(rev_IN);

                    if (str2.ToUpper() == "BYE")
                        break;
                    // 4. close    
                    
                }
                stream.Close();
                client.Close(); 
                    
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
        public static void toServer()
        {
            try
            {
                //TcpClient client = new TcpClient();
                TcpClient client = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1001));
                // 1. connect
                client.Connect("127.0.0.1", 9999);
                Stream stream = client.GetStream();
                byte[] data = new byte[1024];
                Console.WriteLine("Connected to Server.");
                while (true)
                {
                    string str = "Ban tin tra loi cua IN";
                    var reader = new StreamReader(stream);
                    var writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    // 2. send ban tin response nhan duoc tu IN
                    //if (rev_IN != null)
                    writer.Write(str);

                    // 3. receive ban tin moi tu server
                    int recv = stream.Read(data, 0, data.Length);
                    str_server = Encoding.ASCII.GetString(data, 0, recv);
                    Console.WriteLine(str_server);
                    listRevServer.Add(str_server);
                    //Console.WriteLine("So luong ban tin Server gui cho Client la: " + listRevServer.Count);
                    if (str_server.ToUpper() == "BYE")
                        break;
                    // 4. close                      

                }
                stream.Close();
                client.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
    }
}
