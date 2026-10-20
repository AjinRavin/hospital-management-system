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
using System.Windows.Forms.DataVisualization.Charting;


namespace hms
{
    public partial class receptionist_home : Form
    {
        SqlConnection con = Program.con;

        public receptionist_home()
        {
            InitializeComponent();
            LoadUpcomingAppointments();
            LoadTotalpatients();
            LoadTotaldoctors();
            SetupPatientChart();
            LoadPatientChart();
            chart1.Visible = true;
            chart1.BringToFront();
            LoadTodayAppointments();
            LoadBillsGenerated();



        }
        private void LoadPatientChart()
        {
            chart1.Series["Patients"].Points.Clear();

            string query = @"
        SELECT 
            CAST(registration_date AS DATE) AS RegistrationDate,
            COUNT(*) AS PatientCount
        FROM patient_details
        WHERE registration_date >= DATEADD(DAY, -6, CAST(GETDATE() AS DATE))
        GROUP BY CAST(registration_date AS DATE)
        ORDER BY RegistrationDate";

            Dictionary<DateTime, int> data = new Dictionary<DateTime, int>();

            try
            {
                // Make sure Program.con is clean
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DateTime date =
                                Convert.ToDateTime(dr["RegistrationDate"]).Date;

                            int count =
                                Convert.ToInt32(dr["PatientCount"]);

                            data[date] = count;
                        }
                    }
                }

                // Reader is now definitely closed
                con.Close();

                // Show last 7 days
                for (int i = 6; i >= 0; i--)
                {
                    DateTime date = DateTime.Today.AddDays(-i);

                    int count = 0;

                    if (data.ContainsKey(date))
                    {
                        count = data[date];
                    }

                    chart1.Series["Patients"].Points.AddXY(
                        date.ToString("d MMM"),
                        count
                    );
                }
            }
            catch (Exception ex)
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

                MessageBox.Show(
                    "Patient Chart Error:\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        private void SetupPatientChart()
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Legends.Clear();

            ChartArea area = new ChartArea("Area1");

            area.BackColor = Color.FromArgb(3, 32, 58);

            area.AxisX.LabelStyle.ForeColor = Color.White;
            area.AxisX.LineColor = Color.White;
            area.AxisX.MajorGrid.Enabled = false;

            area.AxisY.LabelStyle.ForeColor = Color.White;
            area.AxisY.LineColor = Color.White;
            area.AxisY.MajorGrid.LineColor =
                Color.FromArgb(30, 70, 100);

            chart1.ChartAreas.Add(area);

            Series series = new Series("Patients");

            series.ChartType = SeriesChartType.Spline;
            series.BorderWidth = 3;
            series.Color = Color.DeepSkyBlue;

            series.MarkerStyle = MarkerStyle.Circle;
            series.MarkerSize = 7;
            series.MarkerColor = Color.DeepSkyBlue;

            chart1.Series.Add(series);

            chart1.BackColor = Color.FromArgb(3, 32, 58);
        }
        public void LoadTotalpatients()
        {
            string query = "SELECT COUNT(*) FROM patient_details";

            try
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    label6.Text = count.ToString();
                }

                con.Close();
            }
            catch (Exception ex)
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

                MessageBox.Show("Patient Count Error:\n\n" + ex.Message);
            }
        }
        
            public void LoadTotaldoctors()
            {
                string query = "SELECT COUNT(*) FROM doctor_details";

                try
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }

                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        label11.Text = count.ToString();
                    }

                    con.Close();
                }
                catch (Exception ex)
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }

                    MessageBox.Show("Doctor Count Error:\n\n" + ex.Message);
                }
            }
        private void LoadUpcomingAppointments()
        {
            try
            {
                string query = @"
            SELECT
                a.appointment_date AS [Date],
                a.appointment_time AS [Time],
                p.patient_name AS [Patient Name],
                d.doctor_name AS [Doctor],
                ISNULL(a.status, 'Scheduled') AS [Status]
            FROM appointments a
            INNER JOIN patient_details p
                ON a.patient_id = p.pid
            INNER JOIN doctor_details d
                ON a.doctor_id = d.did
            WHERE ISNULL(a.status, 'Scheduled') <> 'Completed'
              AND a.appointment_date >= CAST(GETDATE() AS DATE)
            ORDER BY a.appointment_date, a.appointment_time";

                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

                con.Open();

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }

                con.Close();
            }
            catch (Exception ex)
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

                MessageBox.Show(
                    "Upcoming Appointment Error:\n\n" + ex.Message);
            }
        }
        private void LoadTodayAppointments()
        {
            try
            {
                string query = @"
            SELECT COUNT(*)
            FROM appointments
            WHERE appointment_date = CAST(GETDATE() AS DATE)";

                if (con.State != ConnectionState.Closed)
                    con.Close();

                con.Open();

                SqlCommand cmd = new SqlCommand(query, con);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                label7.Text = count.ToString();

                con.Close();
            }
            catch (Exception ex)
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();

                MessageBox.Show(
                    "Today's Appointments Error:\n\n" + ex.Message);
            }
        }
        private void LoadBillsGenerated()
        {
            try
            {
                string query = @"
            SELECT COUNT(*)
            FROM bills
            WHERE CAST(bill_date AS DATE) = CAST(GETDATE() AS DATE)";

                if (con.State != ConnectionState.Closed)
                    con.Close();

                con.Open();

                SqlCommand cmd = new SqlCommand(query, con);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                label9.Text = count.ToString();

                con.Close();
            }
            catch (Exception ex)
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();

                MessageBox.Show(
                    "Bills Generated Error:\n\n" + ex.Message);
            }
        }


       

        private void receptionist_home_Load(object sender, EventArgs e)
        {
            button1.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.BorderSize = 0;
            button3.FlatAppearance.BorderSize = 0;
            button4.FlatAppearance.BorderSize = 0;
            button5.FlatAppearance.BorderSize = 0;
            button6.FlatAppearance.BorderSize = 0;
            dataGridView1.RowTemplate.Height = 45;
            dataGridView1.ColumnHeadersHeight = 45;
            chart1.Titles.Clear();
            chart1.Titles.Add("Patient Registration Overview");
            chart1.Titles[0].ForeColor = Color.White;
            chart1.Titles[0].Font = new Font("Segoe UI", 12, FontStyle.Bold);
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            add_patients obj = new add_patients();

            panel4.Controls.Clear();
            panel4.BackColor = Color.White;


            obj.Dock = DockStyle.Fill;

            panel4.Controls.Add(obj);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            book_appointments obj = new book_appointments();

            panel4.Controls.Clear();
            panel4.BackColor = Color.White;
            obj.Dock = DockStyle.Fill;

            panel4.Controls.Add(obj);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            receptionist_home obj = new receptionist_home();
            obj.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            login obj = new login();
            obj.Show();        }

       

        private void button5_Click(object sender, EventArgs e)
        {
            patient_history obj = new patient_history();

            panel4.Controls.Clear();
            panel4.BackColor = Color.White;
            obj.Dock = DockStyle.Fill;

            panel4.Controls.Add(obj);
        }

        private void button4_Click(object sender, EventArgs e)
        {
           generate_bill obj = new generate_bill();

            panel4.Controls.Clear();

            obj.Dock = DockStyle.Fill;

            panel4.Controls.Add(obj);
            panel4.BackColor = Color.White;
        }
    }
}
