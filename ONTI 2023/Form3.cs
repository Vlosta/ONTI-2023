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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        string email="ion@oti.ro", nume = "";
        public static Form1 frm1;
        Form4 frm4;
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Jocuri.mdf;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=true");
        int ctReload = 0;

        private void close(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            frm1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frm4 = new Form4();
            Form4.frm3 = this; 
            frm4.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form5 frm5 = new Form5();
            frm5.Show();
            this.Hide();
            Form5.frm3 = this;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form6 frm6 = new Form6();
            frm6.Show();
            this.Hide();
            Form6.frm3 = this;
        }

        public void fc()
        {
            chart1.Invalidate(new Rectangle(0, 0, chart1.Width, chart1.Height));
            con.Open();
            email = Form1.email; nume = Form1.nume;
            label1.Text = "Bine ai venit, " + nume + "! (" + email + ")";
            SqlCommand cmd = new SqlCommand("Select PunctajJoc,Data from Rezultate where TipJoc='0' and EmailUtilizator=@p1 ORDER BY Data", con);
            cmd.Parameters.Add("@p1", email);
            SqlDataReader reader = cmd.ExecuteReader();
            DateTime data1, data;
            data = DateTime.Now.AddDays(7);
            while (reader.Read())
            {
                data1 = Convert.ToDateTime(reader[1]);
                if (data != data1)
                {
                 
                        chart1.Series[0].Points.AddXY(data1, reader[0]);
                }
                 data = data1;
            }
            reader.Close();
            SqlCommand cmd1 = new SqlCommand("Select PunctajJoc,Data from Rezultate where TipJoc='1' and EmailUtilizator=@p1 ORDER BY Data", con);
            cmd1.Parameters.Add("@p1", email);
            reader = cmd1.ExecuteReader();
            data = DateTime.Now.AddDays(7);
            while (reader.Read())
            {
                data1 = Convert.ToDateTime(reader[1]);
                if (data != data1)
                {
                   
                    
                        chart1.Series[1].Points.AddXY(data1, reader[0]);
                    
                }
                data = data1;
            }
            reader.Close();


            con.Close();
            ctReload++;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            fc();
        }
    }
}
