using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CounterPortal_2D
{
    public partial class GameMessage : Form
    {
        public string Message
        {
            set { message.Text = value; }
        }
        
        public GameMessage()
        {
            InitializeComponent();
        }
    }
}
