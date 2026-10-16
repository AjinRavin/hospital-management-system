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
    public partial class doctor_admin : Form
    {
        SqlConnection con = Program.con;
        public void LoadTotaldoctors()
        {
            string query = "select COUNT(*)from doctor_details";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int count = (int)cmd.ExecuteScalar();
            label7.Text = count.ToString();
            con.Close();
        }
        private void Loaddoctors()
        {
            string query2 = "select did,uid,doctor_name,contact,email,address from doctor_details";
            SqlDataAdapter dr = new SqlDataAdapter(query2, con);
            DataTable dt = new DataTable();
            dr.Fill(dt);
           
        }
        public doctor_admin()
        {
            InitializeComponent();
            LoadTotaldoctors();
            Loaddoctors();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            admin_add_doctor obj = new admin_add_doctor();
            obj.Show();
        }

        private void doctor_admin_Load(object sender, EventArgs e)
        {
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
            button5.FlatAppearance.BorderSize = 0;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            admin_home obj = new admin_home();
            obj.Show();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(0, 122, 204);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(21, 42, 70);
        }

        private void button2_MouseEnter(object sender, EventArgs
            e)
        {
            button2.BackColor = Color.FromArgb(0, 122, 204);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(21, 42, 70);
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.BackColor = Color.FromArgb(0, 122, 204);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        { button4.BackColor = Color.FromArgb(21, 42, 70);
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            button5.BackColor = Color.FromArgb(0, 122, 204);
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            button5.BackColor = Color.FromArgb(21, 42, 70);
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(0, 122, 204);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(21, 42, 70);
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            admin_view_doctor obj = new admin_view_doctor();
            obj.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            admin_update_doctor obj = new admin_update_doctor();
            obj.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            admin_delete_doctor obj = new admin_delete_doctor();
            obj.Show();
        }

       
    }
}
