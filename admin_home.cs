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
using System.Drawing.Drawing2D;
namespace hms
{
    public partial class admin_home : Form
    {
        SqlConnection con=Program.con;
        private viewappointments appointmentPage;

        private void MakeRouded(Panel panel
            )
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, 20, 20, 180, 90);
                path.AddArc(panel.Width - 20, 0, 20, 20, 270, 90);
            path.AddArc(panel.Width - 20,panel.Height- 20, 20,20,0, 90);
            path.AddArc(0, panel.Height - 20, 20, 20, 90, 90);
        }
        public void LoadTotalReceptionists()
        {
            string query = "select COUNT(*)from receptionist_details";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int count = (int)cmd.ExecuteScalar();
            label8.Text = count.ToString();
            con.Close();
        }
        public void LoadTotaldoctors()
        {
            string query = "select COUNT(*)from doctor_details";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int count = (int)cmd.ExecuteScalar();
            label7.Text = count.ToString();
            con.Close();
        }
        private void LoadTotalPatients()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM patient_details";

                SqlCommand cmd = new SqlCommand(query, con);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                int totalPatients = Convert.ToInt32(cmd.ExecuteScalar());

                label10.Text = totalPatients.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading total patients: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        private void LoadTotalAppointments()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM appointments";

                SqlCommand cmd = new SqlCommand(query, con);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                int totalAppointments = Convert.ToInt32(cmd.ExecuteScalar());

                label12.Text = totalAppointments.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading total appointments: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }





        public admin_home()
        {
            InitializeComponent();
            LoadTotalReceptionists();
            MakeRouded(paneldoctor);
            LoadTotaldoctors();
            LoadTotalPatients();
            LoadTotalAppointments();

            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;

        }

        private void label1_Click(object sender, EventArgs e)
        {
            admin_add_receptionist obj = new admin_add_receptionist();
            obj.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            admin_view_receptionist obj = new admin_view_receptionist();
            obj.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            admin_delete_receptionist obj = new admin_delete_receptionist();
            obj.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            admin_update_receptionist obj = new admin_update_receptionist();
            obj.Show();
        }

        private void addReceptionistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            admin_add_receptionist obj = new admin_add_receptionist();
            obj.Show();
        }

        private void viewReceptionistToolStripMenuItem_Click(object sender, EventArgs e)
        {

            admin_view_receptionist obj = new admin_view_receptionist();
            obj.Show();
        }

        private void deleteReceptionistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            admin_delete_receptionist obj = new admin_delete_receptionist();
            obj.Show();
        }

        private void updateReceptionistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            admin_update_receptionist obj = new admin_update_receptionist();
            obj.Show();
        }

        private void admin_home_Load(object sender, EventArgs e)
        {
           
            // =========================
            // BUTTON STYLE
            // =========================
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button8.FlatAppearance.BorderSize = 0;
            button9.FlatAppearance.BorderSize = 0;
            button10.FlatStyle = FlatStyle.Flat;
            button10.FlatAppearance.BorderSize = 0;
            button11.FlatStyle = FlatStyle.Flat;
            button11.FlatAppearance.BorderSize = 0;


            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
           
            button5.FlatStyle = FlatStyle.Flat;
            button5.FlatAppearance.BorderSize = 0;
            button6.FlatStyle = FlatStyle.Flat;
            button6.FlatAppearance.BorderSize = 0;
            button7.FlatStyle = FlatStyle.Flat;
            button7.FlatAppearance.BorderSize = 0;
            button1.Padding = new Padding(0, 0, 0, 0);
        }

       

        private void button3_Click(object sender, EventArgs e)
        {
            login obj = new login();
            obj.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            receptionist_admin obj = new receptionist_admin();
            obj.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            doctor_admin obj = new doctor_admin();
            obj.Show();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(0, 122, 204);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(21,42,70);
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(0, 122, 204);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(21,42,70);
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(0, 122, 204);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(21,42,70);
        }

        

       

        private void button8_Click(object sender, EventArgs e)
        {
            admin_add_doctor obj = new admin_add_doctor();
            obj.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            admin_add_receptionist obj = new admin_add_receptionist();
            obj.Show();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label17.Text = DateTime.Now.ToString("dd mm yyyy");
            label18.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        

       

        private void button5_Click(object sender, EventArgs e)
        {
            panel4.Controls.Clear();

            appointmentPage = new viewappointments();

            appointmentPage.Dock = DockStyle.Fill;

            panel4.Controls.Add(appointmentPage);

            appointmentPage.BringToFront();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            patient_history obj = new patient_history();

            panel4.Controls.Clear();

            obj.Dock = DockStyle.Fill;

            panel4.Controls.Add(obj);
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            admin_add_receptionist obj = new admin_add_receptionist();
            obj.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            search_patient_details obj = new search_patient_details();
            panel4.BackColor = Color.White;
            panel4.Controls.Clear();

            obj.Dock = DockStyle.Fill;

            panel4.Controls.Add(obj);
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            admin_add_doctor obj = new admin_add_doctor();
            obj.Show();

        }

        
        

        
    }
}
