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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Media;
namespace Client
{

    public partial class frmLogin : Form
    {

        
       // Info info = new Info();
        
        
        public frmLogin()
        {
            InitializeComponent();
        }
        public class temp
        {
            
            static public string IP;
            static public string Port;
        }
        public void btnOk_Click(object sender, System.EventArgs e)
        {
            
            temp.IP = txtIP.Text;
            temp.Port = txtPort.Text;
            Info.Name = txtName.Text;
            Client client = new Client();
            
            
            client.Show();
            this.Hide();


        }
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        private void frmLogin_Load(object sender, System.EventArgs e)
        {
        //    MessageBox.Show(" CHÀO MỪNG BẠN ĐÃ ĐẾN VỚI GAME CONFETTI VIET NAM\n"
        //+ "Để chơi bạn phải nhập IP, Port từ MC và nhập tên", "Guide", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtPort.Text = "5000";
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {

        }


        
    }
}
