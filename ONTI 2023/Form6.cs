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
using MessagingToolkit.QRCode.Codec.Data; 

namespace ONTI_2023
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        public static Form3 frm3;

        int[] v = new int[100];
        int ct = 0;
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Jocuri.mdf;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=true");
        int distMax = 0,scorFinal=0;
        string mail = "";
        int nrPrim = 2;
        Bitmap bmp;
        int prim1(int x)
        {
            if (x == 0 | x == 1)
                return 0;
            if (x == 2)
                return 1;
            if (x % 2 == 0)
                return 0;
            for (int i = 3; i <= x / 2; i += 2)
                if (x % i == 0)
                    return 0;
            return 1;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Select EmailUtilizator,PunctajJoc from Rezultate order by EmailUtilizator", con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                string email = rdr[0].ToString();
                int scor = Convert.ToInt32(rdr[1]);
                int i = 0;
                while (scor < v[i])
                    i++;
                if (v[i] - scor > distMax)
                {
                    distMax = v[i] - scor;
                    scorFinal = scor;
                    mail = email;
                    nrPrim = v[i];
                }
            }
            rdr.Close();
            con.Close();
            MessagingToolkit.QRCode.Codec.QRCodeEncoder encoder = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
            //encoder.QRCodeScale = 8;
            string txtEnconde="aa";
            txtEnconde += mail; txtEnconde += ";";
            txtEnconde += scorFinal.ToString(); txtEnconde += ";";
            txtEnconde += nrPrim.ToString();

             
            bmp= encoder.Encode(txtEnconde);

            bmp = new Bitmap(bmp,pictureBox2.Width,pictureBox2.Height);

            string path = System.IO.Directory.GetCurrentDirectory();
            path += "\\Resurse\\ONTI_2023_C#_Resurse\\Prim";
            Bitmap img = new Bitmap(path + "\\Logo_C#.png");

            img = new Bitmap(img,40,40);


            Graphics g =Graphics.FromImage(bmp);
            g.DrawImage(img, 180, 180);

            pictureBox2.Image = bmp;



        }

        private void close(object sender, FormClosedEventArgs e)
        {
            frm3.Show();

        }

        private void Form6_Load(object sender, EventArgs e)
        {
            for (int i = 0; i <= 110; i++)
            {
                if (prim1(i) == 1)
                    v[ct++] = i;  
            }
          
        }
    }
}
