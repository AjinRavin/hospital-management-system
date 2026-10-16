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
    public partial class admin_delete_receptionist : Form
    {
        SqlConnection con = Program.con;
        public admin_delete_receptionist()
        {
            InitializeComponent();
            fillcombo();
        }
        public void fillcombo()
        {
            //comboBox1.Items.Clear();
            string query = "select rid from receptionist_details";
            con.Open();
            SqlCommand cmd= new SqlCommand(query,con);
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
            SqlCommand cmd= new SqlCommand(query,con);
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.Read())
            {
              textBox4.Text=dr[2].ToString();
                textBox6.Text=dr[4].ToString();
                textBox5.Text=dr[5].ToString();
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
            string query = "delete from receptionist_details where rid='" + comboBox1.Text + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            if (cmd.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("receptionist details is deleted");
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                comboBox1.Text = "";
                

            }
            else
            {
                MessageBox.Show("errorwhile deleting receptionist data");
            }
                con.Close();
                fillcombo();
        }

       
    }

        

        

        


      
    }

