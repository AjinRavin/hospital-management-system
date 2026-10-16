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
    public partial class patient_history : UserControl
    {
        SqlConnection con = Program.con;
        int selectedPatientId = 0;
        bool loadingPatients = false;
        public patient_history()
        {
            InitializeComponent();
            textBox1.ReadOnly = true;
            textBox1.BackColor = Color.White;

            LoadPatientIDs();
        }
        private void LoadPatientIDs()
        {
            try
            {
                loadingPatients = true;

                DataTable dt = new DataTable();

                con.Open();

                string query = "SELECT pid FROM patient_details ORDER BY pid";

                SqlCommand cmd = new SqlCommand(query, con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                // Close connection BEFORE changing ComboBox DataSource
                con.Close();

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "pid";
                comboBox1.ValueMember = "pid";

                comboBox1.SelectedIndex = -1;

                textBox1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();

                loadingPatients = false;
            }
        }

        private void LoadAppointments()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();

                string query = @"
            SELECT
                a.appointment_date AS [Appointment Date],
                a.appointment_time AS [Appointment Time],
                d.doctor_name AS Doctor,
                a.status AS Status
            FROM appointments a
            INNER JOIN doctor_details d
                ON a.doctor_id = d.did
            WHERE a.patient_id = @pid
            ORDER BY a.appointment_date DESC,
                     a.appointment_time DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@pid", selectedPatientId);

                    DataTable dt = new DataTable();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void LoadFindings()
        {
            try
            {
                con.Open();

                string query = @"
            SELECT
                datetime AS Date,
                symptoms AS Symptoms,
                diagnosis AS Diagnosis,
                treatment AS Treatment
            FROM findings
            WHERE pid = @pid
            ORDER BY datetime DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@pid", selectedPatientId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt;
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
        private void LoadPrescriptions()
        {
            try
            {
                con.Open();

                string query = @"
            SELECT
                medicine AS Medicine,
                dosage AS Dosage,
                frequency AS Frequency,
                duration AS Duration,
                prescription_date AS Date
            FROM prescriptions
            WHERE pid = @pid
            ORDER BY prescription_date DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@pid", selectedPatientId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView4.DataSource = dt;
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
            if (loadingPatients)
                return;

            if (comboBox1.SelectedIndex == -1)
                return;

            if (comboBox1.SelectedValue == null)
                return;

            // Sometimes SelectedValue is a DataRowView while binding
            if (comboBox1.SelectedValue is DataRowView)
                return;

            try
            {
                con.Open();

                string query = @"
            SELECT patient_name
            FROM patient_details
            WHERE pid = @pid";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@pid",
                    Convert.ToInt32(comboBox1.SelectedValue)
                );

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    textBox1.Text = result.ToString();
                }
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

            if (comboBox1.SelectedIndex == -1 ||
       comboBox1.SelectedValue == null ||
       comboBox1.SelectedValue is DataRowView)
            {
                MessageBox.Show("Please select a patient.");
                return;
            }

            selectedPatientId = Convert.ToInt32(comboBox1.SelectedValue);

            LoadAppointments();
            LoadFindings();
            LoadPrescriptions();

            // Clear selection AFTER all grids have loaded
            this.BeginInvoke(new Action(() =>
            {
                ClearAllGridSelections();
            }));
        }

        private void patient_history_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView2.ColumnHeadersHeight = 40;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = false;
            dataGridView4.ColumnHeadersHeight = 40;
            dataGridView4.ReadOnly = true;
            dataGridView4.AllowUserToAddRows = false;
            dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView4.MultiSelect = false;
            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = null;
            SetupGrid(dataGridView1);
            SetupGrid(dataGridView2);
            SetupGrid(dataGridView4);


        }
        private void SetupGrid(DataGridView dgv)
        {
            dgv.ColumnHeadersHeight = 40;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
        }

        private void ClearAllGridSelections()
        {
            dataGridView1.CurrentCell = null;
            dataGridView1.ClearSelection();

            dataGridView2.CurrentCell = null;
            dataGridView2.ClearSelection();

            dataGridView4.CurrentCell = null;
            dataGridView4.ClearSelection();
        }
    }
}
