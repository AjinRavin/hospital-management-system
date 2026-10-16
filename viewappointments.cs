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
    public partial class viewappointments : UserControl
    {
        private void dataGridView1_DataBindingComplete(
    object sender,
    DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = null;

            SelectedPatientId = 0;
            SelectedDoctorId = 0;
            SelectedAppointmentId = 0;
        }
        public int SelectedPatientId { get; private set; }
        public int SelectedDoctorId { get; private set; }
        public int SelectedAppointmentId { get; private set; }

        SqlConnection con = Program.con;
        public viewappointments()
        {
            InitializeComponent();

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;

            LoadAllAppointments();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
                if (e.RowIndex < 0)
                    return;

                SelectedAppointmentId = Convert.ToInt32(
                    dataGridView1.Rows[e.RowIndex].Cells["appointment_id"].Value
                );

                SelectedPatientId = Convert.ToInt32(
                    dataGridView1.Rows[e.RowIndex].Cells["patient_id"].Value
                );

                SelectedDoctorId = Convert.ToInt32(
                    dataGridView1.Rows[e.RowIndex].Cells["doctor_id"].Value
                );
            }
        private void LoadAllAppointments()
        {
            try
            {
                string query = @"
SELECT
    a.appointment_id AS [appointment_id],
    a.appointment_date AS [Date],
    a.appointment_time AS [Time],
    a.patient_id AS [patient_id],
    a.doctor_id AS [doctor_id],
    p.patient_name AS [Patient Name],
    d.doctor_name AS [Doctor],
    a.status AS [Status]
FROM appointments a
INNER JOIN patient_details p
    ON a.patient_id = p.pid
INNER JOIN doctor_details d
    ON a.doctor_id = d.did
ORDER BY a.appointment_date DESC,
         a.appointment_time DESC";

                SqlCommand cmd = new SqlCommand(query, con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Columns["patient_id"].Visible = false;
                dataGridView1.Columns["doctor_id"].Visible = false;
                dataGridView1.Columns["appointment_id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message);
            }
        }
        private void viewappointments_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersHeight = 40;
        }
    }
}
