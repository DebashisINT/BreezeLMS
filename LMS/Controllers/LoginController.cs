using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LMS.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        private IConfiguration Configuration;
        public LoginController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        private string GetConnectionString()
        {
            return this.Configuration.GetConnectionString("DefaultConnection");
        }
        public ActionResult SubmitForm(LoginModel omodel)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string Encryptpass = omodel.Encrypt(omodel.password.Trim());

                DataTable ds = new DataTable();
                using (SqlCommand cmd = new SqlCommand("PRC_LMSLOGIN", con))
                {
                    cmd.Parameters.AddWithValue("@ACTION", "LOGIN");
                    cmd.Parameters.AddWithValue("@username", omodel.username);
                    cmd.Parameters.AddWithValue("@password", Encryptpass);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(ds);
                    cmd.Dispose();
                    con.Dispose();

                    if (ds != null && ds.Rows.Count > 0)
                    {
                        return RedirectToAction("Privacy", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }                    
                }
            }

            // return View();

        }

    }
}
