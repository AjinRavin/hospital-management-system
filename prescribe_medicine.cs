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
    public partial class prescribe_medicine : UserControl
    {
       SqlConnection con = Program.con;

        int selectedAppointmentId = 0;
        int selectedDoctorId = 0;
        int selectedPatientId = 0;

        public prescribe_medicine()
        {
            InitializeComponent();
            LoadAppointments();

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
        }

        private void LoadAppointments()
        {
            try
            {
                con.Open();

                string query = @"
    SELECT 
        a.appointment_id,
        a.patient_id,
        p.patient_name,
        a.doctor_id,
        d.doctor_name,
        a.appointment_date,
        'Appointment #' + CAST(a.appointment_id AS VARCHAR) 
        + ' - ' + p.patient_name 
        + ' - ' + CONVERT(VARCHAR, a.appointment_date, 105) 
        AS appointment_display
    FROM appointments a
    INNER JOIN patient_details p
        ON a.patient_id = p.pid
    INNER JOIN doctor_details d
        ON a.doctor_id = d.did
    WHERE a.status = 'In Progress'
    ORDER BY a.appointment_date DESC";

                SqlCommand cmd = new SqlCommand(query, con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "appointment_display";
                comboBox1.ValueMember = "appointment_id";
                comboBox1.SelectedIndex = -1;
                textBox1.Clear();
                textBox2.Clear();
                textBox1.BackColor = Color.White;
                textBox2.BackColor = Color.White;
                textBox1.ForeColor = Color.Black;
                textBox2.ForeColor = Color.Black;
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
            if (comboBox1.SelectedItem == null)
                return;

            DataRowView row = comboBox1.SelectedItem as DataRowView;

            if (row == null)
                return;

            selectedAppointmentId = Convert.ToInt32(row["appointment_id"]);
            selectedPatientId = Convert.ToInt32(row["patient_id"]);
            selectedDoctorId = Convert.ToInt32(row["doctor_id"]);

            textBox1.Text = row["patient_id"].ToString();
            textBox2.Text = row["patient_name"].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedAppointmentId == 0)
            {
                MessageBox.Show("Please select an appointment.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter medicine.");
                textBox3.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please enter dosage.");
                textBox4.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please enter frequency.");
                textBox5.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please enter duration.");
                textBox6.Focus();
                return;
            }

            try
            {
                con.Open();

                string query = @"
    INSERT INTO prescriptions
    (did, pid, medicine, dosage, frequency, duration, prescription_date, appointment_id)
    VALUES
    (@did, @pid, @medicine, @dosage, @frequency, @duration, GETDATE(), @appointment_id)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@did", selectedDoctorId);
                cmd.Parameters.AddWithValue("@pid", selectedPatientId);
                cmd.Parameters.AddWithValue("@medicine", textBox3.Text);
                cmd.Parameters.AddWithValue("@dosage", textBox4.Text);
                cmd.Parameters.AddWithValue("@frequency", textBox5.Text);
                cmd.Parameters.AddWithValue("@duration", textBox6.Text);
                cmd.Parameters.AddWithValue("@appointment_id", selectedAppointmentId);

                cmd.ExecuteNonQuery();

                string updateQuery = @"
    UPDATE appointments
    SET status = 'Completed'
    WHERE appointment_id = @appointment_id";

                SqlCommand updateCmd = new SqlCommand(updateQuery, con);

                updateCmd.Parameters.AddWithValue(
                    "@appointment_id",
                    selectedAppointmentId
                );

                updateCmd.ExecuteNonQuery();

                MessageBox.Show("Medicine prescribed successfully!");

                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
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
    }
}

