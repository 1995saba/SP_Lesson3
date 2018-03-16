using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Practice
{
    public partial class Form1 : Form
    {
        private int Port;
        private Socket srvSock;
        private Thread srvThread;
        public Form1()
        {
            InitializeComponent();
            button.Tag = false;
            button.Text = "Start";
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if((bool)button.Tag == true)
            {
                // Отключение сервера
                if (StopServer()) button.Text = "Start";
            }
            else
            {
                // Необходимо запустить сервер
                if (StartServer()) button.Text = "Stop"; 
            }
            button.Tag = !(bool)button.Tag;
        }

        

        private bool StartServer()
        {
            Port = 1234;
            if (srvSock != null) return false;

            srvSock = new Socket(AddressFamily.InterNetwork, 
                                 SocketType.Stream, 
                                 ProtocolType.Tcp);
            IPEndPoint srvEndPoint = new IPEndPoint(0,Port);
            srvSock.Bind(srvEndPoint);
            srvSock.Listen(100);
            ThreadPool.SetMaxThreads(16,32);
            srvThread = new Thread(SrvRoutine);
            srvThread.Start();
            return true;
        }

        private void SrvRoutine()
        {
            Socket clientSocket;
            while (true)
            {
                clientSocket=srvSock.Accept();
                if (clientSocket == null) { break; }
                ThreadPool.QueueUserWorkItem(ClientThread, clientSocket);
            }
        }
        private void ClientThread(object state)
        {
            Socket clientSocket = (Socket)state;
            textBox1.Text += "Client connected\n";
            textBox1.Text += clientSocket.RemoteEndPoint.ToString()+"\n";
            string str = "Hello!";
            byte[] bstr = Encoding.ASCII.GetBytes(str);
            clientSocket.Send(bstr);
            bstr = new byte[1024];
            int size=clientSocket.Receive(bstr);
            str=Encoding.ASCII.GetString(bstr,0,size);
            textBox1.Text += str;
            // clientSocket.Shutdown(SocketShutdown.Both);
        }
        private bool StopServer()
        {
            if (srvSock != null) return false;
            srvSock.Shutdown(SocketShutdown.Both);
            return true;
        }
    }
}
