using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace hms
{
    public partial class login : Form
    {
        SqlConnection con = Program.con;
        public login()
        {
            InitializeComponent();
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "admin" &&
      textBox2.Text == "pwd")
            {
                MessageBox.Show("Login successful");

                admin_home obj = new admin_home();
                obj.Show();
                this.Hide();
                return;
            }

            // Validate empty fields
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                textBox1.Text == "Enter Username" ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                textBox2.Text == "Enter Password")
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            string role = "";

            try
            {
                // Ensure the shared connection is ready
                if (con.State == ConnectionState.Broken)
                    con.Close();

                if (con.State == ConnectionState.Closed)
                    con.Open();

                string query = @"
            SELECT *
            FROM user_details
            WHERE username = @username
              AND password = @password";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@username", textBox1.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@password", textBox2.Text);

                    // Read the role, then close the reader
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            role = dr[3].ToString().Trim();
                        }
                    }
                }

                // Close the shared connection before opening another form
                if (con.State == ConnectionState.Open)
                    con.Close();

                // Open the correct dashboard only after closing the reader
                if (role == "doctor")
                {
                    doctor_home obj = new doctor_home();
                    obj.Show();
                    this.Hide();
                }
                else if (role == "receptionist")
                {
                    receptionist_home obj = new receptionist_home();
                    obj.Show();
                    this.Hide();
                }
                else if (role != "")
                {
                    MessageBox.Show("Invalid user role");
                }
                else
                {
                    MessageBox.Show("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();

                MessageBox.Show(
                    "Login error:\n" + ex.Message);
            }
        }

        private void login_Load(object sender, EventArgs e)
        {
            
            textBox1.Height = 45;
            textBox2.Height = 45;
            textBox2.PasswordChar = '\0';

            textBox1.Text = "Enter Username";
            textBox2.Text = "Enter Password";
          


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int radius = 15;

            e.Graphics.SmoothingMode =
                System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(panel1.Width - radius, 0,
                            radius, radius, 270, 90);
                path.AddArc(panel1.Width - radius,
                            panel1.Height - radius,
                            radius, radius, 0, 90);
                path.AddArc(0,
                            panel1.Height - radius,
                            radius, radius, 90, 90);

                path.CloseFigure();

                using (Pen pen = new Pen(
                    Color.FromArgb(35, 67, 98), 2))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Enter Password")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.White;
                textBox2.PasswordChar = '*';
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {

            if (textBox1.Text == "Enter Username")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.White;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Enter Username";
                textBox1.ForeColor = Color.White;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {

            if (textBox2.Text == "")
            {
                textBox2.PasswordChar = '\0';
                textBox2.Text = "Enter Password";
                
            }
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
    }
}
