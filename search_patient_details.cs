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
    public partial class search_patient_details : UserControl
    {
        SqlConnection con = Program.con;
        public search_patient_details()
        {
            InitializeComponent();

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;

            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;
            textBox3.BackColor = Color.White;
            textBox4.BackColor = Color.White;
            textBox5.BackColor = Color.White;

            LoadPatientIDs();
        }
        private void LoadPatientIDs()
        {
            try
            {
                DataTable dt = new DataTable();

                con.Open();

                string query = "SELECT pid FROM patient_details ORDER BY pid";

                SqlCommand cmd = new SqlCommand(query, con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                con.Close();

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "pid";
                comboBox1.ValueMember = "pid";

                comboBox1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a patient.");
                return;
            }

            try
            {
                con.Open();

                string query = @"
            SELECT
                patient_name,
                gender,
                age,
                address,
                phone
            FROM patient_details
            WHERE pid = @pid";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@pid",
                    Convert.ToInt32(comboBox1.SelectedValue)
                );

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    textBox1.Text = dr["patient_name"].ToString();
                    textBox2.Text = dr["gender"].ToString();
                    textBox3.Text = dr["age"].ToString();
                    textBox4.Text = dr["address"].ToString();
                    textBox5.Text = dr["phone"].ToString();
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
}
