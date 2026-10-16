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
    public partial class admin_update_receptionist : Form
    {
        SqlConnection con = Program.con;
        public admin_update_receptionist()
        {
            InitializeComponent();
            fillcombo();
        }
        public void fillcombo()
        {
            string query = "select rid from receptionist_details";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr[0].ToString());
            }
            con.Close();
        }
        public void getdetails()
        {
            con.Open();
            string query = "select * from receptionist_details where rid='" + comboBox1.Text + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox4.Text = dr[2].ToString();
                textBox3.Text = dr[3].ToString();
                textBox6.Text = dr[4].ToString();
                textBox5.Text = dr[5].ToString();
            }

            con.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdetails();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             con.Open();
             string query = "UPDATE receptionist_details SET receptionist_name='" + textBox4.Text + "',contact='" + textBox3.Text + "',email='" + textBox6.Text + "',address='" + textBox5.Text + "'where rid=" + comboBox1.Text + "";



            SqlCommand cmd = new SqlCommand(query, con);
            if (cmd.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("receptionist details is updated");
                

            }
            else
            {
                MessageBox.Show("error while updating receptionist data");
            }
                con.Close();
                fillcombo();
        }

        private void admin_update_receptionist_Load(object sender, EventArgs e)
        {
            button1.FlatAppearance.BorderSize = 0;
            
        }

        
    }
}
