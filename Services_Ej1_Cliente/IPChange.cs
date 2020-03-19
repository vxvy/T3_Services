using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Services_Ej1_Cliente
{
    public partial class IPChange : Form
    {
        public Form1 form1;

        public IPAddress IpChanged
        {
            set; get;
        }

        public int PortChanged
        {
            set; get;
        }

        public IPChange(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;

            IpChanged = form1.Ip;
            txbIP.Text = IpChanged.ToString();

            PortChanged = form1.Port;
            txbPort.Text = PortChanged.ToString();
        }
        public bool ChangeConnection(IPEndPoint iep)
        {
            Socket sServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sServer.Bind(iep); //comprueba si está libre el puerto
                sServer.Connect(iep); // se intenta conectar al puerto
                return true;
            }
            catch (SocketException)
            {

                return false;
            }

        }
        private void btnIP_Click(object sender, EventArgs e)
        {
            bool portOk;
            bool ipOk;

            string checkPuerto = (portOk = parsePort()) ?
                "El puerto  " + PortChanged + " está libre." 
                : "El puerto " + txbPort.Text + " no es válido.";
            
            string checkIp = (ipOk = parseIp()) ? 
                "La ip " + IpChanged + " es válida." 
                : "La ip " + txbIP.Text + " no es válida.";
            

            if (portOk && ipOk)
            {
                //if (this.ChangeConnection(new IPEndPoint(IpChanged, PortChanged)))
                //{
                MessageBox.Show("Datos cambiados con éxito."
                    + "\r\n Nueva ip: " + IpChanged
                    + "\r\n Nuevo puerto: " + PortChanged);

                form1.lblIP.Text = IpChanged.ToString();
                form1.lblPort.Text = PortChanged.ToString();

                form1.Ip = IpChanged;
                form1.Port = PortChanged;

                form1.textBox1.Text = "Se ha cambiado el destino de la conexión.\r\n";
                //}
                //else{
                //    MessageBox.Show("A pesar de que los datos proporciondos son válidos, " +
                //        "no no es posible conectarse al servidor original en: " +
                //        "\r\nIP: " + IpChanged +
                //        "\r\nPort: " + PortChanged
                //        );
                //}
            }
            else
            {
                MessageBox.Show("No se puede conectar porque alguno de los datos no es válido."
                    + "\r\n" + checkPuerto
                    + "\r\n" +checkIp);
            }

            //this.Close();
        }

        private bool parsePort()
        {
            int newPort = 0;
            if (Int32.TryParse(txbPort.Text, out newPort))
            {
                if (newPort >= 0 && newPort<= 65535)
                {
                    lblPortChange.Visible = false;
                    PortChanged = newPort;
                    return true; // esta es la buena
                }
                else
                {
                    lblPortChange.Visible = true;
                    return false;
                }
            }
            else
            {
                lblPortChange.Visible = true;
                return false;
            }
        }
        
        private bool parseIp()
        {
            try
            {
                if (txbIP.Text.Contains("."))
                {
                    string[] a = txbIP.Text.Split('.');
                    if (a.Length == 4)
                    {
                        for (int i = 0; i < a.Length; i++)
                        {
                            if (!(Int32.Parse(a[i])<256))
                            {
                                throw new FormatException();
                            }
                            //else
                            //{
                            //    MessageBox.Show("Int32.Parse(a[i]) < 256 " + (Int32.Parse(a[i]) < 256));
                            
                            //}
                        }
                    }
                    else
                    {
                        //MessageBox.Show("a.Length == 4 " + (a.Length == 4) + " current length " + a.Length);
                        throw new FormatException();
                    }
                }
                else
                {
                    //MessageBox.Show("txbIP.Text.Contains(\".\") " + txbIP.Text.Contains("."));
                    throw new FormatException();
                }

                IpChanged = IPAddress.Parse(txbIP.Text);
                //MessageBox.Show(""+IpChanged);
                lblIPChange.Visible = false;
                return true;
            }
            catch (FormatException)
            {
                lblIPChange.Visible = true;
                return false;
            }
        }
    }
}