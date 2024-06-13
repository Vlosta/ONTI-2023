using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Server;
using System.Runtime.Serialization;
using System.Globalization;
using MessagingToolkit.QRCode.Codec.Data;


namespace ONTI_2023
{
    public partial class Form1 : Form
    {
        string path = "";
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Jocuri.mdf;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=true");
        public Form1()
        {
            InitializeComponent();
        }
        int ok = 0;
        public static string email = ""; public static string nume = "";
        
        private void Form1_Load(object sender, EventArgs e)
        {
            con.Open();
            path = System.IO.Directory.GetCurrentDirectory();
            path += "\\Resurse\\ONTI_2023_C#_Resurse\\";
            StreamReader sr = new StreamReader(path + "Utilizatori.txt");
            SqlCommand cmd1 = new SqlCommand("Truncate table Utilizatori",con);
            //cmd1.ExecuteNonQuery();
            string line = "";
            while((line=sr.ReadLine())!=null)
            {
                string email = line.Split(';')[0];
                string nume = line.Split(';')[1];
                string parola = line.Split(';')[2];
                SqlCommand cmd = new SqlCommand("Insert into Utilizatori values(@p1,@p2,@p3)", con);
                cmd.Parameters.Add("@p1", email);
                cmd.Parameters.Add("@p2", nume);
                cmd.Parameters.Add("@p3", parola);
                //cmd.ExecuteNonQuery();
            }
            sr = new StreamReader(path + "Rezultate.txt");
            SqlCommand cmd2 = new SqlCommand("Truncate table Rezultate", con);
            //cmd2.ExecuteNonQuery();
            while((line=sr.ReadLine())!=null)
            {
                int tip = Convert.ToInt32(line.Split(';')[0]);
                string email = line.Split(';')[1];
                int pct = Convert.ToInt32(line.Split(';')[2]);
                DateTime dt = Convert.ToDateTime(DateTime.ParseExact(line.Split(';')[3], "dd.M.yyyy", CultureInfo.InvariantCulture));  
                SqlCommand cmd = new SqlCommand("Insert into Rezultate values(@p1,@p2,@p3,@p4)", con);
                cmd.Parameters.Add("@p1", tip);
                cmd.Parameters.Add("@p2", email);
                cmd.Parameters.Add("@p3", pct);
                cmd.Parameters.Add("@p4", dt);
                //cmd.ExecuteNonQuery();

            }
            sr.Close();
            con.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = path+"QRCode";
            ofd.ShowDialog();
            string str = ofd.SafeFileName;

            pictureBox1.Image = Image.FromFile(path + "QRCode\\" + str);

         }

        private void button1_Click(object sender, EventArgs e)
        {
            MessagingToolkit.QRCode.Codec.QRCodeDecoder objDecodare = new MessagingToolkit.QRCode.Codec.QRCodeDecoder();
            string sirCodare = objDecodare.decode(new
            MessagingToolkit.QRCode.Codec.Data.QRCodeBitmapImage(pictureBox1.Image as Bitmap));
            textBox1.Text = sirCodare.Split('\n')[1].ToString();
            textBox2.Text = sirCodare.Split('\n')[2].ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * from Utilizatori where EmailUtilizator=@p1 and Parola=@p2", con);
            cmd.Parameters.Add("@p1", textBox1.Text);
            cmd.Parameters.Add("@p2", textBox2.Text);
            SqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read())
            {
                ok = 1;
                email = rdr[0].ToString();
                nume = rdr[1].ToString();
                Form3 frm3 = new Form3();
                Form3.frm1 = this;
                frm3.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Email sau parola gresite! Incearca din nou.");
                textBox2.Text = "";
                textBox1.Text = "";
            }
            rdr.Close();
            con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2.frm1 = this;
            Form2 frm2 = new Form2();
            frm2.Show();
            this.Hide();
        }

        private void close(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
