using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Services_T3_Ej1_Server
{
    class Program
    {
        public static int serverPort = 2000;

        static void Main(string[] args)
        {
            string mensaje = "";
            IPAddress ip = IPAddress.Loopback;
            IPEndPoint ie = new IPEndPoint(ip, 1200);
            
            while (!testPuerto(ie))
            {
                serverPort++;
                ie = new IPEndPoint(ip, serverPort);
            }

            Socket sServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sServer.Bind(ie);
            
            while (mensaje != "APAGAR")
            {
                sServer.Listen(1);

                Socket sClient = sServer.Accept();

                IPEndPoint ieClient = (IPEndPoint)sClient.RemoteEndPoint;

                NetworkStream ns = new NetworkStream(sClient);

                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);

                mensaje = sr.ReadLine();

                switch (mensaje)
                {
                    case "HORA":
                        sw.WriteLine("Time is: "+DateTime.Now.TimeOfDay);
                        break;
                    case "FECHA":
                        sw.WriteLine("Date is: "+DateTime.Today);
                        break;
                    case "TODO":
                        sw.WriteLine("Time and date is: " + DateTime.Now);
                        break;
                    case "APAGAR":
                    default:
                        sw.WriteLine("¿Cómo has llegado aquí?");
                        break;
                }
                sw.Flush();
                
                sw.Close();
                sr.Close();
                ns.Close();
                sClient.Close();
            }

            sServer.Close();
        }

        public static bool testPuerto(IPEndPoint iep)
        {
            bool puertoLibre = false;
            Socket sTest = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            try
            {
                sTest.Bind(iep);

                puertoLibre = true;
            }
            catch (SocketException se) when (se.ErrorCode == (int) SocketError.AddressAlreadyInUse)
            {
                puertoLibre = false;
            }
            finally
            {
                sTest.Close();
            }

            return puertoLibre;
        }
    }
}