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


namespace hms
{
    public partial class admin_add_receptionist : Form
    {
        SqlConnection con = Program.con;
        public admin_add_receptionist()
        {
            InitializeComponent();
            this.TransparencyKey = Color.Empty;
        }

        public void get_userid()
        {
            con.Open();
            string query = "select isnull(max(uid),100)+1 from user_details";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox2.Text = dr[0].ToString();
            }
            con.Close();
        }

        private void admin_add_receptionist_Load(object sender, EventArgs e)
        {
            get_userid();
            get_receptionistid();
            button1.FlatAppearance.BorderSize = 0;
        }
        public void get_receptionistid()
        {
            con.Open();
            string query = "select isnull(max(rid),5000)+1 from receptionist_details";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox1.Text = dr[0].ToString();
            }
            con.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter Receptionist ID");
                textBox1.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter User ID");
                textBox2.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please enter Receptionist Name");
                textBox4.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter Contact Number");
                textBox3.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please enter Email");
                textBox6.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please enter Address");
                textBox5.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("Please enter Username");
                textBox7.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox8.Text))
            {
                MessageBox.Show("Please enter Password");
                textBox8.Focus();
                return;
            }
            con.Open();
            string query = "insert into user_details values(" + textBox2.Text + ",'" + textBox7.Text + "','" + textBox8.Text + "','receptionist')";
            string query2 = "insert into receptionist_details values("+textBox1.Text+","+textBox2.Text+",'"+textBox4.Text+"','"+textBox3.Text+"','"+textBox6.Text+"','"+textBox5.Text+"')";
            SqlCommand Cmd1 = new SqlCommand(query, con);
            SqlCommand Cmd2 = new SqlCommand(query2, con);
            if (Cmd1.ExecuteNonQuery() > 0)
            {
                if (Cmd2.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("registration success");
                }
                else
                {
                    MessageBox.Show("registration failed");
                }
            }
            else
            {
                MessageBox.Show("registration failed");
            }
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();  // Receptionist ID
            textBox2.Clear();  // User ID
            textBox3.Clear();  // Contact
            textBox4.Clear();  // Receptionist Name
            textBox5.Clear();  // Address
            textBox6.Clear();  // Email
            textBox7.Clear();  // Username
            textBox8.Clear();  // Password

            textBox1.Focus();

        }

       
    }
}
