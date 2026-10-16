using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hms
{
    public partial class add_patients : UserControl
    {
        SqlConnection con = Program.con;
        public add_patients()
        {
            InitializeComponent();
        }

        public void get_userid()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();

                string query = "SELECT ISNULL(MAX(pid), 200) + 1 FROM patient_details";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    object result = cmd.ExecuteScalar();
                    textBox1.Text = result.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating Patient ID: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void add_patients_Load(object sender, EventArgs e)
        {
            get_userid();
            button1.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.BorderSize = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter patient name.");
                textBox2.Focus();
                return;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select gender.");
                comboBox1.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter age.");
                textBox3.Focus();
                return;
            }

            if (!int.TryParse(textBox3.Text, out int age) || age <= 0)
            {
                MessageBox.Show("Please enter a valid age.");
                textBox3.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please enter address.");
                textBox4.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please enter phone number.");
                textBox5.Focus();
                return;
            }

            string query = @"
        INSERT INTO patient_details
        (pid, patient_name, gender, age, address, phone, registration_date)
        VALUES
        (@pid, @patient_name, @gender, @age, @address, @phone, GETDATE())";

            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@pid", Convert.ToInt32(textBox1.Text));
                    cmd.Parameters.AddWithValue("@patient_name", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@gender", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@address", textBox4.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", textBox5.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Patient added successfully.");

                textBox2.Clear();
                comboBox1.SelectedIndex = -1;
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding patient: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            // Generate the next Patient ID after closing the connection
            get_userid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();

            get_userid();
        }
    }
}
