using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CounterPortal_2D
{
    public partial class Connect : Form
    {
        public string name;
        public string IP;
        public int Port;
        
        public Connect()
        {
            InitializeComponent();
            ExitBtn.DialogResult = DialogResult.Cancel;
            PlayBtn.DialogResult = DialogResult.OK;
        }

        private void PlayBtn_Click(object sender, EventArgs e)
        {
            try
            {
                name = NameTB.Text;
                IP = IPTB.Text;
                Port = int.Parse(PortTB.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid IP or Port. Exiting.");
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
