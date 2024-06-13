using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ONTI_2023
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Jocuri.mdf;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=true");

        string[] cuvinte = new string[5];
        Image[] images = new Image[20];
        Image[] imgDisplay=new Image[5];
        string path = "";
        int width = 150; int height = 150;
        HashSet<int> index = new HashSet<int>();
        string[] fisiere = new string[20];
        char[] litere = new char[100];
        char[] litere1 = new char[100];
        PictureBox[] pb = new PictureBox[20];
        int widthLit = 50,heightLit=50;
        int x, y;
        Image minge;
        int dir=0;
        string cuvantPrelucrat = "";

        int timp=100;

        int ctCorect = 0;
        public static Form3 frm3;

        int ok()
        {
            int ok1 = 3;
            for(int i = 0; i<cuvantPrelucrat.Length; i++)
            {
                if (i < cuvinte[1].Length & i < cuvinte[2].Length)
                {
                    if (cuvantPrelucrat[i] != cuvinte[1][i] & cuvantPrelucrat[i] != cuvinte[2][i])
                        ok1 = -1;
                }
                else if (i >= cuvinte[1].Length)
                {
                    if (cuvantPrelucrat[i] != cuvinte[2][i])
                        ok1 = -1;
                }
                else if(i >= cuvinte[2].Length)
                {
                    if (cuvantPrelucrat[i] != cuvinte[1][i])
                        ok1 = -1;
                }
            }
            if (cuvantPrelucrat == cuvinte[1])
                return 1;
            if (cuvantPrelucrat == cuvinte[2])
                return 2;
            return ok1;
        }
        int ctms = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            ctms += timer1.Interval;
            if (ctms > 1000)
            {
                timp--;
                ctms = 0;
            }
            if(timp<0)
            {
                timer1.Stop(); con.Open();
                SqlCommand cmd = new SqlCommand("Insert into Rezultate values(@p1,@p2,@p3,@p4)", con);
                int a = 1;
                int b = 0;
                cmd.Parameters.Add("@p1", a);
                cmd.Parameters.Add("@p2", Form1.email);
                cmd.Parameters.Add("@p3", b);
                cmd.Parameters.Add("@p4", DateTime.Now);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            label2.Text = "Timp: " + timp;
            Bitmap img = new Bitmap(img3);
            Graphics g = Graphics.FromImage(img);
            if (dir != 3)
            {
                if (dir == 1)
                    x -= 60;
                if (dir == 2)
                    x += 60;
                dir = 0;
            }
            if (dir == 3)
            {
                y -= 10;
                if (y < -50)
                {
                    y = pictureBox3.Height - 50;
                    dir = 0;
                    pb[x / 60].Hide();
                    label1.Text += pb[x / 60].Tag.ToString();
                    cuvantPrelucrat += pb[x / 60].Tag.ToString();
                    if(ok()==-1)
                    {
                        //pierdut
                        label1.Text = "Pierdut!";
                        timer1.Stop();
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Insert into Rezultate values(@p1,@p2,@p3,@p4)", con);
                        int a = 1;
                        int b = 0;
                        cmd.Parameters.Add("@p1", a);
                        cmd.Parameters.Add("@p2", Form1.email);
                        cmd.Parameters.Add("@p3", b);
                        cmd.Parameters.Add("@p4", DateTime.Now);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    if(ok()==1)
                    {
                        //cuv1 corect;
                        ctCorect++;
                        cuvantPrelucrat = "";
                        label1.Text = "Litere: ";
                    }
                    if(ok()==2)
                    {
                        //cuv2 corect;
                        ctCorect++;
                        cuvantPrelucrat = "";
                        label1.Text = "Litere: ";
                    }
                    if(ok()==3)
                    {
                        //continua
                    }
                    if(ctCorect==2)
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Insert into Rezultate values(@p1,@p2,@p3,@p4)", con);
                        int a = 1;
                        cmd.Parameters.Add("@p1", a);
                        cmd.Parameters.Add("@p2", Form1.email);
                        cmd.Parameters.Add("@p3", timp);
                        cmd.Parameters.Add("@p4", DateTime.Now);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        timer1.Stop();
                        //castigat;
                        label1.Text = "Casigat!";
                    }
                }
                
            }
            g.DrawImage(minge, x, y);
            pictureBox3.Image = img;
        }

        private void keyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode.ToString() == "A")
                dir = 1;
            if (e.KeyCode.ToString() == "D")
                dir = 2;
            if (e.KeyValue == 32)
                dir = 3;
            

        }

        private void close(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            frm3.fc();
            frm3.Show();
        }

        Bitmap img3;
        private void Form5_Load(object sender, EventArgs e)
        {
            KeyPreview = true;
            timer1.Start();
            y = pictureBox3.Height-50;
            x = 0;
             path = System.IO.Directory.GetCurrentDirectory();

            path += "\\Resurse\\ONTI_2023_C#_Resurse\\";
            minge = Image.FromFile(path + "ball.png");
            path += "Imagini\\";
            System.IO.Directory.GetFiles(path).CopyTo(fisiere,0);
            for (int i = 0; i < 14; i++)
            {
               
                images[i] = Image.FromFile(fisiere[i]);
                images[i] = new Bitmap(images[i], new Size(width, height));
                fisiere[i] = fisiere[i].Remove(fisiere[i].LastIndexOf('.'));
                int ct = 0;
                for(int j=0; j < fisiere[i].Length; j++)
                {
                    if (fisiere[i][j] == '\\')
                        ct++;

                }
                images[i].Tag = fisiere[i].Split('\\')[ct].TrimEnd();
 

            }
            Random r = new Random();
            
            while(index.Count!=2)
            {
                index.Add(r.Next(0,13));
            }
           
            imgDisplay[0] = images[index.ElementAt(0)];
            imgDisplay[1] = images[index.ElementAt(1)];
            pictureBox1.Size = new Size(width, height);
            pictureBox2.Size = new Size(width, height);
             
            pictureBox1.Image = imgDisplay[0];
            pictureBox2.Image = imgDisplay[1];

            litere = imgDisplay[0].Tag.ToString().ToCharArray();
            litere = litere.Concat(imgDisplay[1].Tag.ToString()).ToArray();

            cuvinte[1] = pictureBox1.Image.Tag.ToString();
            cuvinte[2] = pictureBox2.Image.Tag.ToString();
             



            HashSet<int> aux = new HashSet<int>();
            while(aux.Count!=litere.Length)
            { aux.Add(r.Next(0,litere.Length)); }

          
            for(int i=0; i<aux.Count; i++)
            {
                litere1[i] = litere[aux.ElementAt(i)];
            }

            string str = new string(litere1, 0, aux.Count);
        
             Font fnt = new Font("Arial", 13);
            for(int i=0; i<str.Length; i++)
            {
                pb[i] = new PictureBox();
                pb[i].Size = new Size(widthLit, heightLit);
                pb[i].Tag = litere1[i];
                Bitmap img = new Bitmap(widthLit, heightLit);
                Graphics g = Graphics.FromImage(img);
                g.DrawString(litere1[i].ToString(), fnt, Brushes.Blue, new Point(0, 0));
                pb[i].Image = img;
                pb[i].Location = new Point(100+i*60, 100);
                this.Controls.Add(pb[i]);
            }
            this.Invalidate();
            minge = new Bitmap(minge, new Size(widthLit, heightLit));

            img3 = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            
            




        }
    }
}
