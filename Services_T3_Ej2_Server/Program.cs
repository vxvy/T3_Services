using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Services_T3_Ej2_Server
{
        /**
         * Haz un chatroom. El servidor permite conectarse a cuantos clientes deseen y lo que recibe de un
    cliente lo reenvía al resto de los clientes indicando la IP. Cuando un cliente se conecte debe indicar un
    usuario de forma que cuando envíe un mensaje al resto de los usuarios indicará en el formato
    usuario@IP quién lo envía antes del mensaje en sí mismo. Además el servidor informará al resto de
    clientes de quién se conecta y quién se desconecta. Un usuario se podrá conectar y desconectar
    cuando lo desee escribiendo el comando #salir. También podrá ver la lista de usuarios conectados
    escribiendo #lista. Por supuesto debe ser robusto ante salidas abruptas de un cliente.*/


    class Program
    {

        public static int testport = 22333;
        public static IPAddress localserver = IPAddress.Loopback;
        public static bool serverOn = true;
        public static int maxClients = 10;
        public static object l = new object();
        public static List<IPEndPoint> listIPEndPoints = new List<IPEndPoint>();
        public static List<StreamWriter> listStreamWriters = new List<StreamWriter>(); //estamos en el servidor, 
                                                                                       //así que para escribirles a todos necesitamos sus SW

        static void Main(string[] args)
        {
            IPEndPoint ie = new IPEndPoint(localserver, TestPort(testport));
            Socket sServer = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            try
            {
                sServer.Bind(ie);
                sServer.Listen(maxClients);
                while (serverOn)
                {
                    Socket sClient = sServer.Accept();
                    Thread th = new Thread(()=>Client(sClient));
                    th.Start();
                }
            }
            catch(SocketException se){
                Console.WriteLine(se.StackTrace);
            }

        }

        //whike lock if
        public static void Client(Socket sCliente)
        {
            bool clientConnected = true;
            IPEndPoint iep =(IPEndPoint)sCliente.RemoteEndPoint;
            string strUserMessage = "";
            string strWelcome = String.Format("USER {0}@{1} HAS JOINED THE CHAT ROOM.", iep.Address, iep.Port);

            using (NetworkStream ns = new NetworkStream(sCliente))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {
                lock (l)
                {
                    listIPEndPoints.Add(iep);   //Al lanzar el hilo se añade el cliente a la lista
                    listStreamWriters.Add(sw);  //Necesitamos una col. de SW para poder escribirles

                    foreach(StreamWriter clientsStreamWriters in listStreamWriters) //se va a mandar el mensaje a sí mismo, soy consciente de ello
                    {
                        //Console.WriteLine("La lista no está vacía");
                        clientsStreamWriters.WriteLine(strWelcome);
                        clientsStreamWriters.Flush();
                    }
                    Console.WriteLine(strWelcome);
                }

                while (clientConnected)
                {
                    if (strUserMessage != null && strUserMessage.ToUpper().Trim() != "#SALIR")
                    {
                        try
                        {
                            strUserMessage = sr.ReadLine();
                            if(strUserMessage != null)
                            {
                                string strResponse = ""; //Añadimos las cosas a este string para imprimir una sola vez
                                    
                                if (strUserMessage.ToUpper().Trim() == "#LISTA")
                                {
                                    lock (l)
                                    {
                                        foreach (IPEndPoint ipendpoint in listIPEndPoints)
                                        {
                                            strResponse += String.Format(
                                                "\r\n{0}@{1}",
                                                ipendpoint.Address,ipendpoint.Port);
                                        }
                                    }
                                }
                                else if (strUserMessage.ToUpper().Trim() == "#SALIR")
                                {
                                    clientConnected = false;
                                    listStreamWriters.Remove(sw); //Al terminar la conexión se saca el writer de la lista de clientes
                                    listIPEndPoints.Remove(iep); //Al terminar la conexión se saca al cliente de la lista de clientes
                                    strResponse = String.Format("USER {0}@{1} SE HA DESCONECTADO.",iep.Address,iep.Port);
                                }
                                else if(strUserMessage != null)
                                {
                                    strResponse = String.Format("{0}@{1}: {2}", iep.Address, iep.Port, strUserMessage);
                                }

                                if (strUserMessage.ToUpper().Trim() != "#LISTA") {
                                    lock (l)
                                    {
                                        foreach (StreamWriter clientStreamWriter in listStreamWriters) //Por la estructura del código no se lo envía a sí mismo
                                        {
                                            clientStreamWriter.WriteLine(strResponse);
                                            clientStreamWriter.Flush();
                                        }
                                    }
                                }
                                else
                                {
                                    sw.WriteLine(strResponse);
                                    sw.Flush();
                                }

                                Console.WriteLine(strResponse);
                            }
                        }                           //Código repe se ejecuta cuando se dispone de un cliente cualquiera
                        catch (IOException ioe)     //Trata de acceder al socket y no está permitido
                        {                           // Cuando llega aquí es porque se ha cerrado abruptamente el cliente
                            clientConnected = false;
                            listStreamWriters.Remove(sw); //Al terminar la conexión se saca el writer de la lista de clientes
                            listIPEndPoints.Remove(iep);  //Al terminar la conexión se saca al cliente de la lista de clientes
                            sCliente.Close();

                            lock (l)
                            {
                                int cont = 0;
                                foreach (StreamWriter clientStreamWriter in listStreamWriters)
                                {
                                    clientStreamWriter.WriteLine("USER {0}@{1} SE HA DESCONECTADO.", iep.Address, iep.Port);
                                    clientStreamWriter.Flush();
                                    cont++;
                                }
                                //Console.WriteLine("Se ha intentado acceder a un socket del que se dispuso. " + cont + "\r\n"
                                //    +ioe.Message);
                                Console.WriteLine("catch USER {0}@{1} SE HA DESCONECTADO. colección: {2}", iep.Address, iep.Port,cont);
                            }
                        }//termina el try-catch
                    }//termina el if de clientConnected
                    else   //Código repe, se ejecuta  cuando se dispone del último
                    {
                        clientConnected = false;
                        listStreamWriters.Remove(sw); //Al terminar la conexión se saca el writer de la lista de clientes
                        listIPEndPoints.Remove(iep);  //Al terminar la conexión se saca al cliente de la lista de clientes
                        sCliente.Close();

                        lock (l)
                        {
                            int cont = 0;
                            foreach (StreamWriter clientStreamWriter in listStreamWriters)
                            {
                                clientStreamWriter.WriteLine("USER {0}@{1} SE HA DESCONECTADO.", iep.Address, iep.Port);
                                clientStreamWriter.Flush();
                                cont++;
                            }

                            Console.WriteLine("else USER {0}@{1} SE HA DESCONECTADO. colección: {2}", iep.Address, iep.Port, cont);
                        }
                    }//termina else de clientConnected
                } // termina el while
            }
        }

        public static int TestPort(int testport)
        {
            bool puertoLibre = false;
            int portInUse = 0;

            while (!puertoLibre) {
                IPEndPoint iep = new IPEndPoint(localserver,testport);
                Socket sTest = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                try
                {
                    sTest.Bind(iep);
                    puertoLibre = true;
                }
                catch (SocketException se) when (se.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    testport++;
                    Console.WriteLine("Imposible conectar en puerto: "+iep.Port);
                }
                catch (Exception e){
                    Console.WriteLine(e.StackTrace);
                }
                finally
                {
                    sTest.Close();
                    portInUse = testport;
                }
            }

            return testport;
        }
    }

}