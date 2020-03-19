using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Services_Ej1_Cliente
{
    public partial class Form1 : Form
    {
        private IPAddress ip = IPAddress.Loopback; // IP del server
        public IPAddress Ip
        {
            set
            {
                ip = IPAddress.Parse(value.ToString());
                lblIP.Text = value.ToString();
            }
            get
            {
                return ip;
            }
        }

        private int port = 1200;
        public int Port
        {
            set{
                port = value;
                lblPort.Text = value.ToString();
            }
            get{
                return port;
            }
        }

        public Form1()
        {
            InitializeComponent(); 
            lblIP.Text = ip.ToString();
            lblPort.Text = port.ToString();
        }

        private void Conexion(string mensaje)
        {
            string response = "";
            IPEndPoint ie = new IPEndPoint(Ip, Port);
            Socket sServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sServer.Connect(ie);
            }
            catch (SocketException se)
            {
                Console.WriteLine("Error connection: {0}\nError code: {1}({2})",se.Message, (SocketError)se.ErrorCode, se.ErrorCode);
                MessageBox.Show("Error en la conexión");
                return;
            }

            NetworkStream ns = new NetworkStream(sServer);
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);

            try
            {
                sw.WriteLine(mensaje);
                sw.Flush();
                response=sr.ReadLine();
            }
            catch (IOException)
            {
                MessageBox.Show("El servidor es inaccesible.");
            }

            this.textBox1.Text += response+"\r\n";

            sw.Close();
            sr.Close();
            ns.Close();
            sServer.Close();

            if (mensaje == "APAGAR")
            {
                this.Close();
            }
        }

        private void btnHora_Click(object sender, EventArgs e)
        {
            this.Conexion(((Button)sender).Text);
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            IPChange iPC = new IPChange(this);
            iPC.ShowDialog();
        }
    }
}