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
    public partial class doctor_home : Form
    {
        private viewappointments appointmentPage;
        SqlConnection con = Program.con;
        public doctor_home()
        {
            InitializeComponent();
            LoadTodayAppointments();
            LoadMyPatients();
            LoadConsultationCompleted();
            LoadPrescriptionIssued();
        }

        private void LoadConsultationCompleted()
        {
            LoadCount(
                @"SELECT COUNT(*)
          FROM appointments
          WHERE status = 'Completed'",
                label9
            );
        }

        private void LoadPrescriptionIssued()
        {
            LoadCount(
                @"SELECT COUNT(DISTINCT appointment_id)
          FROM prescriptions
          WHERE appointment_id IS NOT NULL",
                label11
            );
        }

        private void LoadMyPatients()
        {
            LoadCount(
                "SELECT COUNT(*) FROM patient_details",
                label7
            );
        }

        // Common method for loading dashboard counts
        private void LoadCount(string query, Label targetLabel)
        {
            bool openedHere = false;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    openedHere = true;
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    targetLabel.Text = count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading " +
                    targetLabel.Name + ":\n" + ex.Message);
            }
            finally
            {
                if (openedHere && con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void LoadTodayAppointments()
        {
            bool openedHere = false;

            try
            {
                string query = @"
            SELECT
                a.appointment_time AS [Time],
                p.patient_name AS [Patient Name],
                d.doctor_name AS [Doctor],
                ISNULL(a.status, 'Waiting') AS [Status]
            FROM appointments a
            INNER JOIN patient_details p
                ON a.patient_id = p.pid
            INNER JOIN doctor_details d
                ON a.doctor_id = d.did
            WHERE a.appointment_date >= CAST(GETDATE() AS DATE)
              AND a.appointment_date < DATEADD(
                    DAY, 1, CAST(GETDATE() AS DATE))
            ORDER BY a.appointment_time";

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    openedHere = true;
                }

                DataTable dt = new DataTable();

                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                dataGridView1.DataSource = dt;
                label6.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading today's appointments:\n" + ex.Message);
            }
            finally
            {
                if (openedHere && con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void doctor_home_Load(object sender, EventArgs e)
        {
            button1.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.BorderSize = 0;
            button3.FlatAppearance.BorderSize = 0;
            button4.FlatAppearance.BorderSize = 0;
            button5.FlatAppearance.BorderSize = 0;
            button6.FlatAppearance.BorderSize = 0;
            button7.FlatAppearance.BorderSize = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            panel11.Controls.Clear();

            appointmentPage = new viewappointments();

            appointmentPage.Dock = DockStyle.Fill;

            panel11.Controls.Add(appointmentPage);

            appointmentPage.BringToFront();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            doctor_home obj = new doctor_home();
            obj.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            login obj = new login();
            obj.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (appointmentPage == null)
            {
                MessageBox.Show("Please open View Appointments first.");
                return;
            }

            if (appointmentPage.SelectedAppointmentId == 0 ||
                appointmentPage.SelectedPatientId == 0 ||
                appointmentPage.SelectedDoctorId == 0)
            {
                MessageBox.Show("Please select an appointment first.");
                return;
            }

            int appointmentId = appointmentPage.SelectedAppointmentId;
            int pid = appointmentPage.SelectedPatientId;
            int did = appointmentPage.SelectedDoctorId;

            panel11.Controls.Clear();

            add_findings findings = new add_findings(appointmentId, did, pid);

            findings.Dock = DockStyle.Fill;

            panel11.Controls.Add(findings);
            panel11.BackColor = Color.White;

            findings.BringToFront();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            prescribe_medicine obj = new prescribe_medicine();
            panel11.BackColor = Color.White;
            panel11.Controls.Clear();

            obj.Dock = DockStyle.Fill;

            panel11.Controls.Add(obj);
        }

        private void button5_Click(object sender, EventArgs e)
        {
           patient_history obj= new patient_history();
            panel11.BackColor = Color.White;
            panel11.Controls.Clear();

            obj.Dock = DockStyle.Fill;

            panel11.Controls.Add(obj);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            search_patient_details obj = new search_patient_details();
            panel11.BackColor = Color.White;
            panel11.Controls.Clear();

            obj.Dock = DockStyle.Fill;

            panel11.Controls.Add(obj);
        }
    }
}
