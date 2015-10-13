using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FATExplorer
{
    public partial class Editor : Form
    {
        public Editor(string output)
        {
            InitializeComponent();
            richTextBox1.Text = output;
        }
    }
}
