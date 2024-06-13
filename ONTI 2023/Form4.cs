using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ONTI_2023
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        int timp = 100;
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Jocuri.mdf;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=true");
        public static Form3 frm3;
        int f(int n)
        {
            if (n <= 2)
                return 1;
            else
                return f(n - 1) + f(n - 2);
        }
        int nivel = 3;
        PictureBox[] pb = new PictureBox[20];
        Image[] img = new Image[20];
        string path;
        HashSet<int> set = new HashSet<int>();
        int[][] perechi = new int[10][];
        Image orange;
        PictureBox ecran = new PictureBox();
        int prev = -100;
        int corecte = 0;
        int ctClick = 0;
        int tick = 0;
        string[] arr1 = new string[35];


        private void Form4_Load(object sender, EventArgs e)
        {
            ecran.Location = new Point(0, 0);
            ecran.Size=new Size(this.Width,this.Height);
            this.Controls.Add(ecran);
            this.Invalidate();
            orange = new Bitmap(75, 75);
             

            Graphics g =Graphics.FromImage(orange);
            g.Clear(Color.Orange);

            path = System.IO.Directory.GetCurrentDirectory();
            path += "\\Resurse\\ONTI_2023_C#_Resurse\\Imagini\\";
            string[] arr = new string[20];
            System.IO.Directory.GetFiles(path).CopyTo(arr, 0);
 
            for (int i = 0; i < 14; i++) {
                 img[i] = Image.FromFile(arr[i]);
                img[i] = new Bitmap(img[i], 75,75);
                    }
            load_pb(nivel);
             
        }
       
        void win()
        {
            this.Controls.Clear();
            path = System.IO.Directory.GetCurrentDirectory();
            path += "\\Resurse\\ONTI_2023_C#_Resurse\\Artificii\\";
            System.IO.Directory.GetFiles(path).CopyTo(arr1, 0);
            SqlCommand cmd = new SqlCommand("Insert into Rezultate values(@p1,@p2,@p3,@p4)", con);
            int zero = 0;
            con.Open();
            cmd.Parameters.Add("@p1", zero);
            cmd.Parameters.Add("@p2", Form1.email);
            cmd.Parameters.Add("@p3", timp);
            cmd.Parameters.Add("@p4", DateTime.Now);
            cmd.ExecuteNonQuery();
            con.Close();
            this.Controls.Add(ecran);
            timer1.Stop();

            timer2.Start();
        }
        
         private void click(object sender, EventArgs e)
        {
            ctClick++;
            PictureBox pbox = (PictureBox)sender;
            int index = Convert.ToInt32(pbox.Tag);
            label1.Text= index.ToString();

            //15 - 0; 14 - 1[
            //2*f(nivel)-15-1

            if (index >= f(nivel))
            {
                pb[index].Image = img[set.ElementAt(2 * f(nivel) - 1 - index)];
 
            }
            else
            { pb[index].Image = img[set.ElementAt(index)];
             }
            pb[index].Refresh();

            if (index==2*f(nivel)-prev-1)
            {
                corecte++;
                if(corecte==f(nivel))
                {
                    nivel++;
                    if(nivel!=7)
                    load_pb(nivel);
                    if (nivel == 7)
                        win();
                    corecte = 0;
                    
                }
            }
            else if(prev!=-100)
            {
              
                Thread.Sleep(1000);
                pb[index].Image = orange;
                pb[prev].Image = orange;
            }
            if(ctClick==1)
                 prev = index;
            if (ctClick == 2)
            {
                prev = -100;
                ctClick = 0;
            }
            
        }
        void btn_click(object sender, EventArgs e)
        {
            timer1.Start();
        }
        void load_pb(int n)
        {
            Random r = new Random();
             this.Controls.Clear(); 
            this.Invalidate();
            if(n==3)
            {
                Button btn = new Button();
                btn.Location = new Point(400, 200);
                btn.Text = "START";
                btn.Click += new EventHandler(btn_click);
                this.Controls.Add(btn);
                this.Invalidate();
            }
            while (set.Count!=f(n))
            { set.Add(r.Next(0,13)); }
            for(int i=0; i<f(n); i++)
            {
                pb[i] = new PictureBox();
                pb[i].Size = new Size(75, 75);
                pb[i].Location = new Point(15+80*(i%f(n)),15+80*(i/f(n)));
                pb[i].Image = orange;
                //pb[i].Image = img[set.ElementAt(i)];
                //pb[i].BackColor = Color.Orange;
                pb[i].Tag = i;
                pb[i].Click += new EventHandler(click);
                this.Controls.Add(pb[i]);
            }
            for (int i = f(n); i <2* f(n); i++)
            {
                pb[i] = new PictureBox();
                pb[i].Size = new Size(75, 75);
                pb[i].Location = new Point(15 + 80 * (i % f(n)), 15 + 80 * (i / f(n)));
                pb[i].Image = orange;
                //pb[i].Image = img[set.ElementAt(f(n)-i%f(n)-1)];
                //pb[i].BackColor=Color.Orange;
                pb[i].Tag = i;
                pb[i].Click += new EventHandler(click);
                this.Controls.Add(pb[i]);
            }
           

            this.Invalidate();
        }
       
        private void timer2_Tick(object sender, EventArgs e)
        {
            Bitmap img= new Bitmap(arr1[tick]);
            img = new Bitmap(img, ecran.Size);
            ecran.Image = img;
            tick++;
            label2.Text = tick.ToString();
            if (tick == 33)
                timer2.Stop();
            this.Invalidate();

        }
        int ctms = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            ctms += timer1.Interval;
            if (ctms >= 1000)
            {
                timp--;
                this.Text = "Joc memorie - Timp ramas: " + timp + " secunde";
                ctms = 0;
            }
        }

        private void close(object sender, FormClosedEventArgs e)
        {
            frm3.fc();
            frm3.Show();
        }
    }
}
