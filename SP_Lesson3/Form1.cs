using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP_Lesson3
{
    public partial class Form1 : Form
    {
        Mutex mutex;
        public Form1()
        {
            bool IsCreate = true;
            mutex = new Mutex(false, "MyMutex", out IsCreate);
            if(!IsCreate)
                mutex.WaitOne();

            InitializeComponent();

            //if (!IsCreate)
            //{
            //    MessageBox.Show("Программа уже запущена");
            //    Close();
            //}
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            mutex.ReleaseMutex();
        }
    }
}
