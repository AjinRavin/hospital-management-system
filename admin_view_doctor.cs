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
    
    public partial class admin_view_doctor : Form
    {
        SqlConnection con = Program.con;
        public admin_view_doctor()
        {
            InitializeComponent();
        }
        public void getdoctordetails()
        {
            con.Open();
            string query = "select*from doctor_details";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void admin_view_doctor_Load(object sender, EventArgs e)
        {
            getdoctordetails();

        }
    }
}
