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

namespace Server
{
    public partial class Server : Form
    {
        public TcpClient Client;
        public StreamReader STR;
        public StreamWriter STW;
        public string recieve;
        public Thread thread;
        public NetworkStream ns ;
        static readonly object _lock = new object();
        static readonly Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();
        public int slClient = 0;
        public int second = 10;
        public bool check = true;
        public Server()
        {

            InitializeComponent();
            //Doc cau hoi tu file
            readQuestion();

            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            //show IP local
                foreach (IPAddress address in localIP)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        txtLocalIP.Text = address.ToString();
                    }
                }

        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            Thread thread = new Thread(StartServer);
            thread.Start();
            panel1.Visible = false;
            
            
        }
        List<InfoPlayer> listPlayer = new List<InfoPlayer>();
       
        void StartServer()
        {
            string data=" ";
            int count =1;
            TcpListener ServerSocket = new TcpListener(IPAddress.Any, 5000);
            ServerSocket.Start();

            while (true)
            {
                TcpClient client = ServerSocket.AcceptTcpClient();
                lock (_lock) list_clients.Add(count, client);



                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int byte_count = stream.Read(buffer, 0, buffer.Length);

                if (byte_count == 0)
                {
                    break;
                }

                data = Encoding.ASCII.GetString(buffer, 0, byte_count);




                //Receive answer from client 
                Thread t = new Thread(handle);
                t.Start(count);
                count++;

                //Luu nick name vao list Player
                CreateNULLListPlayer(slClient, data);
                slClient++;

            }
        }

       
        void CreateNULLListPlayer(int n,string name)
        {
            
            InfoPlayer info = new InfoPlayer();
            info.Score = 0;
            info.Name = name;
            listPlayer.Add(info);
        }
        public int index = 0;
        public  void handle(object o)
        {
            
            int id = (int)o;
            TcpClient client;

            lock (_lock) client = list_clients[id];
            //int cauhoi = 0;
            while (true)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int byte_count = stream.Read(buffer, 0, buffer.Length);

                if (byte_count == 0)
                {
                    break;
                }

                string data = Encoding.ASCII.GetString(buffer, 0, byte_count);

               if(data == lsQuestion[index-1].CorrectAnswer)
               {
                  listPlayer[id - 1].Score++;
                 
               }
                
              
            }
               // cauhoi++;
            lock (_lock) list_clients.Remove(id);
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        public static void broadcast(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data + Environment.NewLine);

            lock (_lock)
            {
                foreach (TcpClient c in list_clients.Values)
                {
                    NetworkStream stream = c.GetStream();

                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }
        static void ReceiveData(TcpClient Client)
        {
            NetworkStream ns = Client.GetStream();
            byte[] receivedBytes = new byte[1024];
            int byte_count;

            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                Console.Write(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));
            }
        }
       
      //public int index = 0;
        List<Question> lsQuestion;

        private void btnNext_Click(object sender, EventArgs e)
        {
            
            if (index > 9)
            {
                MessageBox.Show("Endgame");
            }
            else
            {
                Question question = lsQuestion[index];
                rtbQuestion.Text = question.Content;
                txtA.Text = question.ListAnswers[0].Content;
                txtB.Text = question.ListAnswers[1].Content;
                txtC.Text = question.ListAnswers[2].Content;

                picBox.ImageLocation = question.ImageLink;
    
                index++;

            }
            btnNext.Enabled = false;
            btnSend.Enabled = true;

            lblCountQS.Text = "Question " + index.ToString();
           
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
      
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void đóngKếtNốiToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        void readQuestion()
        {
            lsQuestion = new List<Question>();
            Question question = null;
            string path = @"D:\DA.txt";
            string[] lines = File.ReadAllLines(path);

          
            
            foreach (string line in lines)
            {
                if (line.StartsWith("@@"))//Question
                {
                    question = new Question();
                    question.Content = line.Substring(2);
                }
                if (line.StartsWith("--"))//Image
                {
                    question.ImageLink = line.Substring(2);
                }
                if (line.StartsWith("$$"))//Answer
                {
                    Answer answer = new Answer();
                    string[] M = line.Substring(2).Split(new char[] { '.' });
                    answer.Id = M[0];
                    answer.Content = M[1];

                    question.ListAnswers.Add(answer);
                }

                if (line.StartsWith("%%"))
                {
                    question.CorrectAnswer = line.Substring(2);
                    lsQuestion.Add(question);
                }
            }
        }
      
        private void btnSend_Click(object sender, EventArgs e)
        {
          //Sau khi send se khoi dong timer

            if (index > 10)
            {
                MessageBox.Show("EndGame");
            }
            else
            {
                string question = rtbQuestion.Text;
                string a = txtA.Text;
                string b = txtB.Text;
                string c = txtC.Text;
                string Pic = picBox.ImageLocation;
                string data = string.Format("{0}@@{1}@@{2}@@{3}@@{4}", question, a, b, c, Pic);
                byte[] buffer = Encoding.ASCII.GetBytes(data);

                broadcast(data);
            }
            timer1.Start();
            btnSend.Enabled = false;
          
        }

        private void mnuInfo_Click(object sender, EventArgs e)
        {
             
        }
        private void SendResult()
        {
            
        }
        private void btnResult_Click(object sender, EventArgs e)
        {
            int numberWin= 0;
            Winner win = new Winner() ;
            string listwin = "";
            for(int i=1;i<=slClient;i++)
            {
                if(listPlayer[i-1].Score == 10)
                {
                    win.ListWinner.Add(listPlayer[i - 1]);
                    listwin += "\n";
                    listwin += listPlayer[i - 1].Name;
                    numberWin++;
                }
            }
          
            //Luu ket qua tro choi vao file test
            String filepath = "D:\\result.txt";
            FileStream fs = new FileStream(filepath, FileMode.Create);
            StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);

            sWriter.WriteLine("Ket qua cua game:\n");
            
            for(int i=0; i< slClient;i++)
            {
                sWriter.Write("Player " + listPlayer[i].Name + ": " + listPlayer[i].Score + " diem \n");
            }

            sWriter.WriteLine("\n\n___Danh sach nguoi choi thang cuoc___ ");
            for (int i = 0; i < win.ListWinner.Count;i++ )
            {
                sWriter.Write("\n" + win.ListWinner[i].Name);
            }
            if(numberWin == 0)
            {
                sWriter.Write("\n Khong co nguoi thang cuoc ");
            }

                //Dong file;
                sWriter.Flush();
                fs.Close();


            //Thong bao ket qua game len MessageBox
            string path = @"D:\result.txt";
            string result = File.ReadAllText(path);

            MessageBox.Show(result);
            if(index == 10)
            {
                string temp = "Show Result";
                btnNext.Enabled = false;
                btnSend.Enabled = false;
                broadcast(temp);
            }
          
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void mnuGuide_Click(object sender, EventArgs e)
        {
          //  exitToolStripMenuItem_Click(sender, e);
        }

        string endgame = "You're running out of time";
      

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            lblTimer.Text = second.ToString();
            second--;
            if (second == -1)
            {

                timer1.Stop();

                lblTimer.Text = "Time out";

                second = 10;

                //Gui signal het thoi gian
                broadcast(endgame);
                broadcast(lsQuestion[index-1].CorrectAnswer);

                btnNext.Enabled = true;

                //Khi da gui het cau hoi se gui tin hieu het game cho client
                //if (index == 10)
                //{
                //    string temp = "Show Result";
                //    btnNext.Enabled = false;
                //    btnSend.Enabled = false;
                //    broadcast(temp);
                //}
            }

        }

        private void Server_Load(object sender, EventArgs e)
        {
            txtPort.Text = "5000";
            btnSend.Enabled = false;
        }
    }
}
