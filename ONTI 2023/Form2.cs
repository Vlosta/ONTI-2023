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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public static Form1 frm1;
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Jocuri.mdf;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=true");

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void close(object sender, FormClosedEventArgs e)
        {
            frm1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frm1.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ok = 1;
            if (!(textBox1.Text.Contains("@") & textBox2.Text != "" & textBox3.Text != "" & textBox4.Text != ""))
                ok = 0;
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM UTILIZATORI WHERE EmailUtilizator=@p1", con);
            cmd.Parameters.Add("@p1", textBox1.Text);
            SqlDataReader rdr= cmd.ExecuteReader();
            if(rdr.Read())
                ok = 0;
            rdr.Close();
            if (textBox3.Text != textBox4.Text)
                ok = 0;
            if (ok == 0)
            {
                MessageBox.Show("Date invalide!");
                textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = "";
            }
            else
            {
                SqlCommand cmd1 = new SqlCommand("Insert into Utilizatori values (@p1,@p2,@p3)", con);
                cmd1.Parameters.Add("@p1", textBox1.Text);
                cmd1.Parameters.Add("@p2", textBox2.Text);
                cmd1.Parameters.Add("@p3", textBox3.Text);
                cmd1.ExecuteNonQuery();
                this.Hide();
                frm1.Show();
            }
            con.Close();
            
        }
    }
}
