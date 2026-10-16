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
    public partial class receptionist_add_appointment : Form
    {
        SqlConnection con = Program.con;
        public receptionist_add_appointment()
        {
            InitializeComponent();
            LoadPatientIds();
            LoadDoctorIds();
        }
        private void LoadPatientIds()
        {
            try
            {
                string query = "SELECT pid FROM patient_details";

                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                comboBox1.Items.Clear();

                while (dr.Read())
                {
                    comboBox1.Items.Add(dr["pid"].ToString());
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        private void LoadDoctorIds()
        {
            try
            {
                string query = "SELECT did FROM doctor_details";

                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                comboBox2.Items.Clear();

                while (dr.Read())
                {
                    comboBox2.Items.Add(dr["did"].ToString());
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Please select Patient ID and Doctor ID");
                return;
            }

            string query = @"INSERT INTO appointments
                     (patient_id, doctor_id, appointment_date, appointment_time)
                     VALUES
                     (@patient_id, @did, @appointment_date, @appointment_time)";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@patient_id", comboBox1.Text);
            cmd.Parameters.AddWithValue("@did", comboBox2.Text);
            cmd.Parameters.AddWithValue("@appointment_date", dateTimePicker1.Value.Date);
            cmd.Parameters.AddWithValue("@appointment_time", dateTimePicker2.Value.ToString("HH:mm:ss"));

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Appointment booked successfully");

                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                textBox1.Clear();
                textBox2.Clear();

                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        
    }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                return;

            string query = "SELECT patient_name FROM patient_details WHERE pid = @pid";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@pid", comboBox1.Text);

            try
            {
                con.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    textBox1.Text = result.ToString();
                }
                else
                {
                    textBox1.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox2.SelectedIndex == -1)
                return;

            string query = "SELECT doctor_name FROM doctor_details WHERE did = @did";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@did", comboBox2.Text);

            try
            {
                con.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    textBox2.Text = result.ToString();
                }
                else
                {
                    textBox2.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            receptionist_home obj = new receptionist_home();
            obj.Show();
        }
    }
}
