using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TTG_Tools
{
    public partial class EncryptionVariants : Form
    {
        public EncryptionVariants()
        {
            InitializeComponent();
        }

        public bool customKey
        {
            get
            {
                return checkBox1.Checked;
            }
        }

        public string key
        {
            get
            {
                return customKeyText.Text;
            }
        }

        public int keyEnc
        {
            get
            {
                return comboBox1.SelectedIndex;
            }
        }

        public bool fullEnc
        {
            get
            {
                if (rbEncFullFile.Checked) return true;
                else return false;
            }
        }

        public bool OldMode
        {
            get
            {
                if (rbOld.Checked) return true;
                else return false;
            }
        }

        private void EncryptionVariants_Load(object sender, EventArgs e)
        {
            if(comboBox1.Items.Count > 0) comboBox1.Items.Clear(); //Чистить, если комбобокс не пустой.

            for (int i = 0; i < MainMenu.gamelist.Count; i++)
            {
                comboBox1.Items.Add(i.ToString() + " " + MainMenu.gamelist[i].gamename);
            }
            rbOld.Checked = true;
            rbEncTex.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EncryptionVariants_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
