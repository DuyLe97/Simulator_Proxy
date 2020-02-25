using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server1
{
    class Program
    {
        const int MAX_CONNECTION = 10;// số lượng client connect to Server
        const int PORT_NUMBER = 9999;
        public static string stringData;

        static TcpListener listener;
        public static String str;
        public static String str_1;
        public static HttpListenerRequest strr;
        public static int re;
        public int ip;
        
        public static void Main()
        {

            IPAddress address = IPAddress.Parse("127.0.0.1");

            listener = new TcpListener(address, PORT_NUMBER);
            Console.WriteLine("Waiting for connection...");

            listener.Start();
            for (int i = 0; i < MAX_CONNECTION; i++)
            {

                if (i == 0) new Thread(RevParket).Start(i);
                else
                {                    
                    new Thread(SentParket).Start();
                }
            }
        }
        //SentParket để gửi parket nhận được từ mn cho client
        static void SentParket()
        {
            //Console.WriteLine("zzzzzz " + str);
            byte[] data = new byte[1024];         
            while (true)
            {

                Socket soc = listener.AcceptSocket();

                Console.WriteLine("Connection received from: {0}",
                                  soc.RemoteEndPoint);
                try
                {
                    var stream = new NetworkStream(soc);
                    var reader = new StreamReader(stream);
                    var writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    while (true)
                    {
                        int recv = stream.Read(data, 0, data.Length);
                        stringData = Encoding.ASCII.GetString(data, 0, recv);
                        //Console.WriteLine(stringData);
                        String stringIPdes = stringData.Substring(0, 0);
                        // 3. send
                        Console.WriteLine("Server receive: " + stringData);
                        Thread.Sleep(2000);
                        //writer.WriteLine("Server send to Client: " + stringData);
                        writer.Write("content--------- " + str);
                    }
                    stream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }

                Console.WriteLine("Client disconnected: {0}",
                                  soc.RemoteEndPoint);
                soc.Close();
            }
        }
        //RevParket để nhận parket khi mn gửi tới
        static void RevParket(object i)
        {

            byte[] data = new byte[1024];
            var lineBuffer = new List<byte>();
            Socket soc = listener.AcceptSocket();
            Console.WriteLine("Connection received from: {0}",
                              soc.RemoteEndPoint);
            try
            {
                var stream = new NetworkStream(soc);
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);
                writer.AutoFlush = true;

                while (true)
                {
                    int b = stream.ReadByte();
                    if (b == -1) return;
                    if (b == 10) break;
                    if (b != 13) lineBuffer.Add((byte)b);
                }
                while (true)
                {
                    int recv = stream.Read(data, 0, data.Length);
                    str = Encoding.ASCII.GetString(data, 0, recv);

                    //str_1 = Encoding.ASCII.GetString(lineBuffer.ToArray());
                    //str_1 = str.Replace('\n', ' ');
                    //str = reader.ReadLine();
                   //Console.WriteLine(str + "------------");
                    // 3. send
                    Console.WriteLine("Server receive: " + str);
                    //Console.WriteLine("---------Server receive: " + recv);
                    //re = recv;
                    Thread.Sleep(2000);
                    //writer.WriteLine("Server send to Client: " + str);
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }

            Console.WriteLine("Client disconnected: {0}",
                              soc.RemoteEndPoint);
            soc.Close();
            //  }
        }
    }
}


    
