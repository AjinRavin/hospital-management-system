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

namespace hms
{
    public partial class admin_update_doctor : Form
    {
        SqlConnection con = Program.con;
        public admin_update_doctor()
        {
          
            InitializeComponent();

            LoadDoctorIDs();

            comboBox1.SelectedIndexChanged +=
                comboBox1_SelectedIndexChanged;
        }

        private void LoadDoctorIDs()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();

                string query = "SELECT did FROM doctor_details ORDER BY did";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DataSource = null;
                comboBox1.Items.Clear();

                comboBox1.DisplayMember = "did";
                comboBox1.ValueMember = "did";
                comboBox1.DataSource = dt;
                comboBox1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Doctor IDs: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        private void admin_update_doctor_Load(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT did FROM doctor_details ORDER BY did";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBox1.DataSource = null;
                    comboBox1.DisplayMember = "did";
                    comboBox1.ValueMember = "did";
                    comboBox1.DataSource = dt;
                    comboBox1.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Doctor IDs: " + ex.Message);
            }
            comboBox1.SelectedIndex = -1;

            textBox1.Clear();  // Doctor Name
            textBox2.Clear();  // Department
            textBox3.Clear();  // Contact
            textBox4.Clear();  // Email
            textBox5.Clear();  // Address

            pictureBox1.Image = null;
            photoBytes = null;
        }

       
        private byte[] photoBytes = null;
        private void button1_Click(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Doctor ID.");
                comboBox1.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter Doctor Name.");
                textBox1.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter Department Name.");
                textBox2.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text) ||
                !textBox3.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid contact number.");
                textBox3.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please enter Email.");
                textBox4.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please enter Address.");
                textBox5.Focus();
                return;
            }

            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();

                string query = @"
            UPDATE doctor_details
            SET doctor_name = @name,
                department = @department,
                contact = @contact,
                email = @email,
                address = @address,
                photo = @photo
            WHERE did = @did";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@did",
                        Convert.ToInt32(comboBox1.SelectedValue));

                    cmd.Parameters.AddWithValue("@name", textBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@department", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@contact", textBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", textBox4.Text.Trim());
                    cmd.Parameters.AddWithValue("@address", textBox5.Text.Trim());

                    cmd.Parameters.Add("@photo", SqlDbType.VarBinary).Value =
                        (object)photoBytes ?? DBNull.Value;

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        MessageBox.Show("Doctor details updated successfully.");
                    else
                        MessageBox.Show("Doctor not found.");
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("CK_doctor_contact_length"))
                {
                    MessageBox.Show(
                        "Contact number cannot exceed 12 digits.",
                        "Invalid Contact Number",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Unable to update doctor details. Please check the entered information.",
                        "Update Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Something went wrong while updating doctor details.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 ||
        comboBox1.SelectedValue == null ||
        comboBox1.SelectedValue is DataRowView)
            {
                return;
            }

            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();

                string query = @"
            SELECT doctor_name, department, photo,
                   contact, email, address
            FROM doctor_details
            WHERE did = @did";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@did", SqlDbType.Int).Value =
                        Convert.ToInt32(comboBox1.SelectedValue);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            textBox1.Text = dr["doctor_name"].ToString();
                            textBox2.Text = dr["department"].ToString();
                            textBox3.Text = dr["contact"].ToString();
                            textBox4.Text = dr["email"].ToString();
                            textBox5.Text = dr["address"].ToString();

                            photoBytes = dr["photo"] == DBNull.Value
                                ? null
                                : (byte[])dr["photo"];

                            if (photoBytes != null)
                            {
                                using (MemoryStream ms =
                                       new MemoryStream(photoBytes))
                                {
                                    pictureBox1.Image =
                                        Image.FromStream(ms);
                                }
                            }
                            else
                            {
                                pictureBox1.Image = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading doctor details: " +
                                ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
}

