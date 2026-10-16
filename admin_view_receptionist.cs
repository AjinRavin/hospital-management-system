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
    public partial class admin_view_receptionist : Form
    {
        SqlConnection con = Program.con;
        public admin_view_receptionist()
        {
            InitializeComponent();
        }
        public void getreceptionistdetails()
        {
            con.Open();
            string query = "select*from receptionist_details";
            SqlDataAdapter sda = new SqlDataAdapter(query,con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void admin_view_receptionist_Load(object sender, EventArgs e)
        {
            getreceptionistdetails();
            dataGridView1.RowTemplate.Height = 35;
            dataGridView1.AutoSizeColumnsMode =
    DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Dock = DockStyle.None;
            dataGridView1.Width = 850;
            dataGridView1.Height = 310;
            
        }

        

        
    }
}
