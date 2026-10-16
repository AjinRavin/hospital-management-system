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
using System.IO;
using System.Drawing.Imaging;

namespace hms
{
    public partial class admin_add_doctor : Form
    {
        SqlConnection con = Program.con;
        public admin_add_doctor()
        {
            InitializeComponent();
            textBox5.MaxLength = 12;
        }
        public void get_userid()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();

                string query = "SELECT ISNULL(MAX(uid),100)+1 FROM user_details";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    textBox2.Text = cmd.ExecuteScalar().ToString();
                }
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        public void get_doctorid()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();

                string query = "SELECT ISNULL(MAX(did),5000)+1 FROM doctor_details";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    textBox1.Text = cmd.ExecuteScalar().ToString();
                }
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
           
            
            textBox3.Clear();  // Doctor Name
            textBox4.Clear();  // Department

            textBox5.Clear();  // Contact
            textBox6.Clear();  // Email
            textBox7.Clear();  // Address
            textBox8.Clear();  // Username
            textBox9.Clear();  // Password

            pictureBoxDoctor.Image = null;  // Clear photo

            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter =
                    "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (Image img = Image.FromFile(openFileDialog.FileName))
                        {
                            pictureBoxDoctor.Image = new Bitmap(img);
                        }

                        pictureBoxDoctor.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to load photo: " + ex.Message);
                    }
                }
            }

        }

        

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text) ||
        string.IsNullOrWhiteSpace(textBox4.Text) ||
        string.IsNullOrWhiteSpace(textBox5.Text) ||
        string.IsNullOrWhiteSpace(textBox6.Text) ||
        string.IsNullOrWhiteSpace(textBox7.Text) ||
        string.IsNullOrWhiteSpace(textBox8.Text) ||
        string.IsNullOrWhiteSpace(textBox9.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            if (textBox5.Text.Trim().Length != 12 ||
                !textBox5.Text.Trim().All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid 12-digit contact number.");
                textBox5.Focus();
                return;
            }

            if (pictureBoxDoctor.Image == null)
            {
                MessageBox.Show("Please select a doctor photo.");
                return;
            }

            byte[] photoBytes;

            // Convert the selected image into bytes
            using (MemoryStream ms = new MemoryStream())
            {
                pictureBoxDoctor.Image.Save(ms, ImageFormat.Png);
                photoBytes = ms.ToArray();
            }

            SqlTransaction transaction = null;

            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();

                transaction = con.BeginTransaction();

                string query1 = @"
            INSERT INTO user_details
            VALUES (@uid, @username, @password, @role)";

                using (SqlCommand cmd1 = new SqlCommand(query1, con, transaction))
                {
                    cmd1.Parameters.AddWithValue("@uid", Convert.ToInt32(textBox2.Text));
                    cmd1.Parameters.AddWithValue("@username", textBox8.Text.Trim());
                    cmd1.Parameters.AddWithValue("@password", textBox9.Text);
                    cmd1.Parameters.AddWithValue("@role", "doctor");

                    cmd1.ExecuteNonQuery();
                }

                string query2 = @"
            INSERT INTO doctor_details
            (did, uid, doctor_name, department, photo,
             contact, email, address)
            VALUES
            (@did, @uid, @name, @department, @photo,
             @contact, @email, @address)";

                using (SqlCommand cmd2 = new SqlCommand(query2, con, transaction))
                {
                    cmd2.Parameters.AddWithValue("@did",
                        Convert.ToInt32(textBox1.Text));

                    cmd2.Parameters.AddWithValue("@uid",
                        Convert.ToInt32(textBox2.Text));

                    cmd2.Parameters.AddWithValue("@name", textBox3.Text.Trim());
                    cmd2.Parameters.AddWithValue("@department", textBox4.Text.Trim());

                    cmd2.Parameters.Add("@photo", SqlDbType.VarBinary, -1)
                        .Value = photoBytes;

                    cmd2.Parameters.AddWithValue("@contact", textBox5.Text.Trim());
                    cmd2.Parameters.AddWithValue("@email", textBox6.Text.Trim());
                    cmd2.Parameters.AddWithValue("@address", textBox7.Text.Trim());

                    cmd2.ExecuteNonQuery();
                }

                transaction.Commit();

                MessageBox.Show("Doctor registered successfully!");

                // Clear the form after successful registration
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
                textBox9.Clear();
                pictureBoxDoctor.Image = null;

                get_userid();
                get_doctorid();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch { }
                }

                MessageBox.Show("Registration failed: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();

                if (transaction != null)
                    transaction.Dispose();
            }
        }

        private void admin_add_doctor_Load(object sender, EventArgs e)
        {
            get_userid();
            get_doctorid();
        }

       
    }
    }

