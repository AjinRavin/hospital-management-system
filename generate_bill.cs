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
    public partial class generate_bill : UserControl
    {

        SqlConnection con = Program.con;

        int selectedAppointmentId = 0;
        int selectedPatientId = 0;

        public generate_bill()
        {
            InitializeComponent();

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;

            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;

            label7.Text = "₹ 0.00";

            LoadCompletedAppointments();
        }

        private void generate_bill_Load(object sender, EventArgs e)
        {

        }
        private void LoadCompletedAppointments()
        {
            try
            {
                DataTable dt = new DataTable();

                string query = @"
                    SELECT
                        a.appointment_id,
                        a.patient_id,
                        p.patient_name,
                        a.appointment_date,
                        'Appointment #' + CAST(a.appointment_id AS VARCHAR)
                        + ' - ' + p.patient_name
                        + ' - ' + CONVERT(VARCHAR, a.appointment_date, 105)
                        AS appointment_display
                    FROM appointments a
                    INNER JOIN patient_details p
                        ON a.patient_id = p.pid
                    LEFT JOIN bills b
                        ON a.appointment_id = b.appointment_id
                    WHERE a.status = 'Completed'
                      AND b.appointment_id IS NULL
                    ORDER BY a.appointment_date DESC";

                con.Open();

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                con.Close();

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "appointment_display";
                comboBox1.ValueMember = "appointment_id";
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex == -1)
                return;

            if (comboBox1.SelectedValue == null)
                return;

            if (comboBox1.SelectedValue is DataRowView)
                return;

            try
            {
                selectedAppointmentId =
                    Convert.ToInt32(comboBox1.SelectedValue);

                con.Open();

                string query = @"
                    SELECT
                        a.patient_id,
                        p.patient_name
                    FROM appointments a
                    INNER JOIN patient_details p
                        ON a.patient_id = p.pid
                    WHERE a.appointment_id = @appointment_id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@appointment_id",
                    selectedAppointmentId);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    selectedPatientId =
                        Convert.ToInt32(dr["patient_id"]);

                    textBox1.Text =
                        dr["patient_id"].ToString();

                    textBox2.Text =
                        dr["patient_name"].ToString();
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
        private void CalculateTotal()
        {
            decimal consultation = 0;
            decimal medicine = 0;
            decimal other = 0;

            decimal.TryParse(textBox3.Text, out consultation);
            decimal.TryParse(textBox4.Text, out medicine);
            decimal.TryParse(textBox5.Text, out other);

            decimal total = consultation + medicine + other;

            label7.Text = "₹ " + total.ToString("0.00");
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a completed appointment.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter consultation fee.");
                textBox3.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please enter medicine fee.");
                textBox4.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please enter other charges.");
                textBox5.Focus();
                return;
            }

            decimal consultationFee;
            decimal medicineFee;
            decimal otherCharges;

            if (!decimal.TryParse(textBox3.Text, out consultationFee))
            {
                MessageBox.Show("Please enter a valid consultation fee.");
                textBox3.Focus();
                return;
            }

            if (!decimal.TryParse(textBox4.Text, out medicineFee))
            {
                MessageBox.Show("Please enter a valid medicine fee.");
                textBox4.Focus();
                return;
            }

            if (!decimal.TryParse(textBox5.Text, out otherCharges))
            {
                MessageBox.Show("Please enter a valid other charge.");
                textBox5.Focus();
                return;
            }

            if (consultationFee < 0 ||
                medicineFee < 0 ||
                otherCharges < 0)
            {
                MessageBox.Show("Charges cannot be negative.");
                return;
            }

            try
            {
                con.Open();

                // Check whether this appointment was already billed
                string checkQuery = @"
                    SELECT COUNT(*)
                    FROM bills
                    WHERE appointment_id = @appointment_id";

                SqlCommand checkCmd =
                    new SqlCommand(checkQuery, con);

                checkCmd.Parameters.AddWithValue(
                    "@appointment_id",
                    selectedAppointmentId);

                int count =
                    Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show(
                        "This appointment has already been billed.");

                    con.Close();

                    LoadCompletedAppointments();

                    return;
                }

                // Insert bill
                string insertQuery = @"
                    INSERT INTO bills
                    (
                        appointment_id,
                        patient_id,
                        consultation_fee,
                        medicine_fee,
                        other_charges
                    )
                    VALUES
                    (
                        @appointment_id,
                        @patient_id,
                        @consultation_fee,
                        @medicine_fee,
                        @other_charges
                    )";

                SqlCommand cmd =
                    new SqlCommand(insertQuery, con);

                cmd.Parameters.AddWithValue(
                    "@appointment_id",
                    selectedAppointmentId);

                cmd.Parameters.AddWithValue(
                    "@patient_id",
                    selectedPatientId);

                cmd.Parameters.AddWithValue(
                    "@consultation_fee",
                    consultationFee);

                cmd.Parameters.AddWithValue(
                    "@medicine_fee",
                    medicineFee);

                cmd.Parameters.AddWithValue(
                    "@other_charges",
                    otherCharges);

                cmd.ExecuteNonQuery();

                con.Close();

                decimal total =
                    consultationFee +
                    medicineFee +
                    otherCharges;

                MessageBox.Show(
                    "Bill generated successfully!\n\n" +
                    "Total Amount: ₹ " +
                    total.ToString("0.00"));

                // Clear fields
                comboBox1.SelectedIndex = -1;

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();

                label7.Text = "₹ 0.00";

                selectedAppointmentId = 0;
                selectedPatientId = 0;

                // Refresh completed appointments
                LoadCompletedAppointments();
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
    

