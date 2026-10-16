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
    public partial class add_findings : UserControl
    {
        public int AppointmentId { get; private set; }
        public add_findings(int appointmentId, int did, int pid)
        {
            InitializeComponent();

            AppointmentId = appointmentId;

            LoadDoctors();
            LoadPatients();

            comboBox1.SelectedValue = did;
            comboBox2.SelectedValue = pid;

            comboBox1.Enabled = false;
            comboBox2.Enabled = false;

            textBox1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        SqlConnection con = Program.con;
        public add_findings()
        {
            InitializeComponent();
            LoadDoctors();
            LoadPatients();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            textBox1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        private void LoadDoctors()
        {
            try
            {
                string query = @"SELECT did,
                         CAST(did AS VARCHAR) + ' - ' + doctor_name AS DoctorDisplay
                         FROM doctor_details";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "DoctorDisplay";
                comboBox1.ValueMember = "did";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadPatients()
        {
            try
            {
                string query = @"SELECT pid,
                         CAST(pid AS VARCHAR) + ' - ' + patient_name AS PatientDisplay
                         FROM patient_details";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "PatientDisplay";
                comboBox2.ValueMember = "pid";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void add_findings_Load(object sender, EventArgs e)
        {
            button1.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.BorderSize = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a doctor");
                return;
            }

            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a patient");
                return;
            }

            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Please enter symptoms");
                return;
            }

            if (textBox3.Text.Trim() == "")
            {
                MessageBox.Show("Please enter diagnosis");
                return;
            }

            if (textBox4.Text.Trim() == "")
            {
                MessageBox.Show("Please enter treatment");
                return;
            }

            try
            {
                int did = Convert.ToInt32(comboBox1.SelectedValue);
                int pid = Convert.ToInt32(comboBox2.SelectedValue);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // 1. Save findings
                string query = @"INSERT INTO findings
                         (did, pid, datetime, symptoms, diagnosis, treatment)
                         VALUES
                         (@did, @pid, @datetime, @symptoms, @diagnosis, @treatment)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add("@did", SqlDbType.Int).Value = did;
                cmd.Parameters.Add("@pid", SqlDbType.Int).Value = pid;

                cmd.Parameters.Add("@datetime", SqlDbType.DateTime).Value =
                    DateTime.Parse(textBox1.Text);

                cmd.Parameters.Add("@symptoms", SqlDbType.VarChar).Value =
                    textBox2.Text.Trim();

                cmd.Parameters.Add("@diagnosis", SqlDbType.VarChar).Value =
                    textBox3.Text.Trim();

                cmd.Parameters.Add("@treatment", SqlDbType.VarChar).Value =
                    textBox4.Text.Trim();

                cmd.ExecuteNonQuery();


                // 2. Change appointment status
                string updateQuery = @"
    UPDATE appointments
    SET status = 'In Progress'
    WHERE appointment_id = @appointment_id";

                SqlCommand updateCmd = new SqlCommand(updateQuery, con);

                updateCmd.Parameters.Add("@appointment_id", SqlDbType.Int).Value =
                    AppointmentId;

                updateCmd.ExecuteNonQuery();

                con.Close();

                MessageBox.Show("Finding added successfully\nAppointment is now In Progress");

                // Clear form
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;

                textBox1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            textBox1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }
    }
}
