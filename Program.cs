using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace hms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static SqlConnection con = new SqlConnection("server=DESKTOP-MCL3D5N\\SQLEXPRESS;database=hospitaldb;integrated security=TRUE;");
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new login());
           //Application.Run(new admin_home());
            //Application.Run(new admin_add_receptionist());
            //Application.Run(new receptionist_admin());
            //Application.Run(new doctor_admin());
            //Application.Run(new admin_update_doctor());
            //Application.Run(new admin_delete_doctor());
           //Application.Run(new receptionist_home());
            //Application.Run(new doctor_home());

        }


    }
}
