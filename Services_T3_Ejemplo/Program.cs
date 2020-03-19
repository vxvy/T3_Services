using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Services_T3_Ejemplo
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint ie = new IPEndPoint(IPAddress.Any, 31416);
            //Creacion del Socket
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Enlace de socket al puerto (y en cualquier interfaz de red)
            //Salta excepción si el puerto está ocupado
            s.Bind(ie);
            //Esperando una conexión y estableciendo cola de clientes pendientes
            s.Listen(10);
            //Esperamos y aceptamos la conexion del cliente (socket bloqueante)
            Socket sClient = s.Accept();
            //Obtenemos la info del cliente
            //El casting es necesario ya que RemoteEndPoint es del tipo EndPoint
            //mas genérico
            IPEndPoint ieClient = (IPEndPoint)sClient.RemoteEndPoint;
            Console.WriteLine("Client connected:{0} at port {1}", ieClient.Address, ieClient.Port);
            sClient.Close(); // Se puede usar using con Socket y nos ahorramos los close.
            s.Close();
            //Preparando End Point del servidor
            //Creación del Stream de Red. Nuevamente puede hacerse con using.
            NetworkStream ns = new NetworkStream(sClient);
            //StreamReader y StreamWriter aceptan un Stream
            //como parámetro en el constructor
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            string welcome = "Welcome to The Echo-Logic, Odd, Desiderable, " +
            "Incredible, and Javaless Echo Server (T.E.L.O.D.I.J.E Server)";
            //El envío por red se convierte en un WriteLine
            sw.WriteLine(welcome);
            //Con flush se fuerza el envío de los datos sin esperar al cierre
            sw.Flush();

            string msg="";
            while (msg!="#bro")
            {
                try
                {
                    //Leemos el mensaje del cliente
                    msg = sr.ReadLine();
                    //if (msg == "#bro")
                    //{
                    //    break;
                    //}
                    // Si se cierra el cierra el cliente mientras se espera
                    // en el ReadLine, este devuelve null.
                    Console.WriteLine(msg != null ? msg : "Client disconnected");
                    //Mandamos nuevamente el mensaje al cliente
                    sw.WriteLine(msg);
                    sw.Flush();
                }
                // Si se cierra el cliente, salta excepción
                // Al siguiente readline
                catch (IOException e)
                {
                    break;
                }
            }
            Console.WriteLine("Connection closed");

            //El código del protocolo debe ir antes de esta línea
            //Siempre cerramos los Streams y sockets si no lo hemos hecho con using.
            sw.Close();
            sr.Close();
            ns.Close();
            sClient.Close();
            s.Close();
        }
    }
}
