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
    public partial class Client : Form
    {
        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public string recieve;
        public Thread thread;
        public NetworkStream ns;
        public string answer = " ";
        public bool checkTime = true;
        public bool AlreadysendQs = false;
        int index = 0;
        public Client()
        {
            InitializeComponent();
            Thread thread = new Thread(StartClient);
            thread.Start();
        }

        public void OpenFormLogin()
        {

        }

        
        
        void StartClient()
        {
            
            
            client = new TcpClient();
            IPEndPoint IpEnd = new IPEndPoint(IPAddress.Parse(frmLogin.temp.IP), int.Parse(frmLogin.temp.Port));
            try
            {
                client.Connect(IpEnd);
                if (client.Connected)
                {
                    
                    ns = client.GetStream();
                    string name = Info.Name;
                    //Gui nickname MC
                   
                    byte[] buffer = Encoding.ASCII.GetBytes(name);
                    ns.Write(buffer, 0, buffer.Length);

                   // timer1.Start();

                    //Receive Question
                    thread = new Thread(o => ReceiveData((TcpClient)o));
                    thread.Start(client);
                    MessageBox.Show("Successful connection");
                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed", "Attention");
            }
        }

        private void TrueAnswer()
        {
            Info.Score++;

            lblScore.Invoke((MethodInvoker)(()
                   => lblScore.Text = Info.Score.ToString()));
        }
        private void ShowResult()
        {
            string win = "YOU WIN -- CONGRATULATIONS !!!";
            string lose = "YOU LOSE -- ";
            if (Info.Score == 10)
            {
                lblTimer.Invoke((MethodInvoker)(()
                     => lblTimer.Text = win));
            }
            else
            {
                lblTimer.Invoke((MethodInvoker)(()
                     => lblTimer.Text = lose + Info.Score + " point. "));
            }
        }

        public int numQuestion = 0;

        void SetBackToDefault()
        {
            if(answer == "A")
            {
                btnB.Enabled = true;
                btnC.Enabled = true;
            }
            if (answer == "B")
            {
                btnA.Enabled = true;
                btnC.Enabled = true;
            }
            if (answer == "C")
            {
                btnB.Enabled = true;
                btnA.Enabled = true;
            }

            btnA.BackColor = Color.Gainsboro;
            btnB.BackColor = Color.Gainsboro;
            btnC.BackColor = Color.Gainsboro;
            
        }
        void ShowQuestion(string data)
        {
           
            AlreadysendQs = false;

            string[] M = data.Split(new string[] { "@@" }
            , StringSplitOptions.RemoveEmptyEntries);

            if (M.Length > 0)
            {
                checkTime = true;

                var Speak = new SpeechSynthesizer();
                lbQuestion.Invoke((MethodInvoker)(()
                    => lbQuestion.Text = M[0]));
                Speak.SpeakAsync(lbQuestion.Text);
                btnA.Invoke((MethodInvoker)(()
                    => btnA.Text = M[1]));
                Speak.SpeakAsync("A");
                Speak.SpeakAsync(btnA.Text);
                btnB.Invoke((MethodInvoker)(()
                   => btnB.Text = M[2]));
                Speak.SpeakAsync("B");
                Speak.SpeakAsync(btnB.Text);
                btnC.Invoke((MethodInvoker)(()
                   => btnC.Text = M[3]));
                Speak.SpeakAsync("C");
                Speak.SpeakAsync(btnC.Text);
                pic.Invoke((MethodInvoker)(()
                   => pic.ImageLocation = M[4]));
            }

            lblCountQS.Invoke((MethodInvoker)(()
             => lblCountQS.Text = "Question " + countQS));
            lblTimeout.Invoke((MethodInvoker)(()
             => lblTimeout.Text = " "));
        }
        void handleTime()
        {
             //Neu chua tra loi se hien thong bao het thoi gian va gui ket qua rong~
          
                if (AlreadysendQs == false)
                {


                    answer = "didn't answer";
                    byte[] buffer = Encoding.ASCII.GetBytes(answer);
                    ns.Write(buffer, 0, buffer.Length);

                    //  countQSsend++;
                }

                if (AlreadysendQs == true)
                {
                    AlreadysendQs = false;
                }
            
            checkTime = false;
            lblTimeout.Invoke((MethodInvoker)(()
                  => lblTimeout.Text = "Time out"));
        
        }
        
       private void DisableAllButton()
        {
           
           
            if (answer == "A")
            {
                btnB.Invoke((MethodInvoker)(()
                   => btnB.Enabled = false));
                btnC.Invoke((MethodInvoker)(()
                      => btnC.Enabled = false));
            }
            else if (answer == "B")
            {
                btnA.Invoke((MethodInvoker)(()
                  => btnA.Enabled = false));
                btnC.Invoke((MethodInvoker)(()
                    => btnC.Enabled = false));
            }
            else if (answer == "C")
            {
                btnB.Invoke((MethodInvoker)(()
                   => btnB.Enabled = false));
                btnA.Invoke((MethodInvoker)(()
                    => btnA.Enabled =false));
            }

            else
            {
                btnB.Invoke((MethodInvoker)(()
                  => btnB.Enabled = false));
                btnA.Invoke((MethodInvoker)(()
                    => btnA.Enabled = false));
                btnC.Invoke((MethodInvoker)(()
                   => btnC.Enabled = false));
            }
        }
        void setDefaultColorBtn()
        {
            if (answer == "A")
            {
                btnB.Invoke((MethodInvoker)(()
                    => btnB.Enabled = true));
                btnC.Invoke((MethodInvoker)(()
                    => btnC.Enabled = true));
            }
            else if (answer == "B")
            {
                btnA.Invoke((MethodInvoker)(()
                   => btnA.Enabled = true));
                btnC.Invoke((MethodInvoker)(()
                    => btnC.Enabled = true));
            }
            else  if (answer == "C")
            {
                btnB.Invoke((MethodInvoker)(()
                   => btnB.Enabled = true));
                btnA.Invoke((MethodInvoker)(()
                    => btnA.Enabled = true));
            }

            else
            {
                btnB.Invoke((MethodInvoker)(()
                  => btnB.Enabled = true));
                btnA.Invoke((MethodInvoker)(()
                    => btnA.Enabled = true));
                btnC.Invoke((MethodInvoker)(()
                   => btnC.Enabled = true));
            }
           
            btnA.BackColor = Color.Gainsboro;
            btnB.BackColor = Color.Gainsboro;
            btnC.BackColor = Color.Gainsboro;
        }
        int countQS = 0;
        void ReceiveData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[1024];
            int byte_count;

            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) >0)
            {
                string data = Encoding.ASCII.GetString(receivedBytes, 0, byte_count);
        
                switch(data)
                {
                        //Truong hop gui signal het thoi gian
                    case "You're running out of time\r\n":
                        
                       
                        handleTime();
                        DisableAllButton();
                        break;

                        //Gui dap an dung cua cau truoc de so sanh dap an
                    case "A\r\n":
                         btnA.BackColor = Color.YellowGreen;
                         if (answer == "A")
                         {
                             TrueAnswer();
                         }
                            if (answer == "B")
                                btnB.BackColor = Color.Brown;
                            if(answer =="C")
                                btnC.BackColor = Color.Brown;
                            break;
                    case "B\r\n":
                            btnB.BackColor = Color.YellowGreen;
                            if (answer == "B")
                            {
                                TrueAnswer();
                            }
                        if (answer == "A")
                            btnA.BackColor = Color.Brown;
                        if (answer == "C")
                            btnC.BackColor = Color.Brown;
                            break;
                    case "C\r\n":
                            btnC.BackColor = Color.YellowGreen;
                            if (answer == "C")
                            {
                                TrueAnswer();
                            }
                            if (answer == "A")
                                btnA.BackColor = Color.Brown;
                            if (answer == "B")
                                btnB.BackColor = Color.Brown;
                            break;

                        //Thong bao len nguoi choi thang hay thua
                    case "Show Result\r\n":
                            ShowResult();
                            break;

                        //Truong hop con lai la nhan cau hoi tu Server
                    default:
                            //if (countQS < 10)
                            {
                                countQS++;
                                setDefaultColorBtn();
                                ShowQuestion(data);
                            }
                            break;




                }
            }
        }
       
        private void infomationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Thông tin về game
            MessageBox.Show("Đây là Trò chơi Confetti được viết theo đề tài môn Lập trình Windows."
            + "\nGiảng viên hướng dẫn: Trần Văn Quý.\nĐề tài : Thực hiện project game Confetti\nNhóm thực hiện:\n1. Trần Đỗ Thanh Hải - 1760051\n"
            + "2. Nguyễn Minh Hoàng - 1760070\n3. Nguyễn Đình Nam - 1760110\nKhoa Công nghệ Thông tin\n"
            + "Trường Đại học Khoa học Tự nhiên Thành phố Hồ Chí Minh.\n01-06-2019");
        }
        private void mnuGuide_Click(object sender, EventArgs e)
        {
            //Hướng dẫn chơi
            MessageBox.Show("                               Hướng dẫn chơi game\n"
        + "  1. Để bắt đầu trò chơi hãy nhập đúng IP và Port của Server\n"
        + "  2.Sau đó bấm connect để chơi\n","Guide",MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnA_Click(object sender, EventArgs e)
        {
           
            answer = "A";
            btnA.BackColor = Color.DimGray;
          
            btnB.Enabled = false;
            btnC.Enabled = false;
          
            SendYourAnswer(answer);

            
        }

        private void btnB_Click(object sender, EventArgs e)
        {
            answer = "B";
            btnB.BackColor = Color.DimGray;
            btnA.Enabled = false;
            btnC.Enabled = false;
            SendYourAnswer(answer);
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            answer = "C";
            btnC.BackColor = Color.DimGray;
            btnA.Enabled = false;
            btnB.Enabled = false;
            SendYourAnswer(answer);
            
        }
       
        private void SendYourAnswer(string answer)
        {

            
            if (checkTime == true && AlreadysendQs == false )
            {
                byte[] buffer = Encoding.ASCII.GetBytes(answer);
                ns.Write(buffer, 0, buffer.Length);
                
                AlreadysendQs = true;
             
            }
            if (checkTime == false)
            {

                MessageBox.Show("You can't answer this question");
                AlreadysendQs = false;
            }
            
        }

        private void lblTimer_Click(object sender, EventArgs e)
        {

        }

        private void Client_Load(object sender, EventArgs e)
        {
            this.Text = Info.Name;
        }

        private void lblScore_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        

        private void lblTimer1_Click(object sender, EventArgs e)
        {


        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

     

    }
}
