using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Login : System.Web.UI.Page
    {

        String strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            if(txt_email.Text == "" || txt_password.Text == "")
            {
                lbl_message.Visible = true;
                lbl_message.Text = "All fields are required";
                return;
            }

            if (!txt_email.Text.Contains("@"))
            {
                lbl_message.Visible = true;
                lbl_message.Text = "Invalid email format";
                return;
            }

            if (txt_password.Text.Length < 7 || txt_password.Text.Length > 14)
            {
                lbl_message.Visible = true;
                lbl_message.Text = "Password must be between 7 and 14 characters";
                return;
            }

            if (!ExistUser(txt_email.Text))
            {
                lbl_message.Visible = true;
                lbl_message.Text = "User not registered.";
                return;
            }

            if (!ChkPassword(txt_email.Text,txt_password.Text))
            {
                lbl_message.Visible = true;
                lbl_message.Text = "Incorrect password";
                return;
            }

            String role = GetUserRole(txt_email.Text);

            Session["email"] = txt_email.Text;
            Session["role"] = role;

            if (role == "Admin")
            {
                Response.Redirect("/Admin/AdminDashboard.aspx");
            }
            else if (role == "Agent")
            {
                Response.Redirect("/Agent/Dashboard.aspx");
            }
            else
            {
                Response.Redirect("UserDashboard.aspx");
            }
        }

        public bool ExistUser(String email)
        {
            SqlConnection con = new SqlConnection(strcon);

            String query = "SELECT COUNT(*) FROM Users WHERE Email=@email";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", email.Trim());

            con.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            return count > 0;
        }

        public bool ChkPassword(String email,String password)
        {
            SqlConnection con = new SqlConnection(strcon);
            String query = "SELECT COUNT(*) FROM Users WHERE Email=@email AND Password=@password";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);
            con.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return count > 0;
        }

        public String GetUserRole(String email)
        {
            SqlConnection con = new SqlConnection(strcon);
            String query = "SELECT ROLE FROM Users WHERE Email=@email";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", email);
            con.Open();
            String role = cmd.ExecuteScalar().ToString();
            con.Close();
            return role;
        }

        public string GetRoleByEmail(string email)
        {
            SqlConnection con = new SqlConnection(strcon);

            string query = "SELECT Role FROM Users WHERE Email=@email";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", email);

            con.Open();
            object result = cmd.ExecuteScalar();
            con.Close();

            if (result != null)
                return result.ToString();
            else
                return null;
        }


        protected void txt_email_TextChanged(object sender, EventArgs e)
        {
            String role = GetRoleByEmail(txt_email.Text);
            ddl_role.Items.Clear();

            if(role != null)
            {
                ddl_role.Items.Add(role);
                ddl_role.Enabled = false;
            }
            else
            {
                ddl_role.Enabled = true;
                ddl_role.Items.Add("Customer");
                ddl_role.Items.Add("Agent");
            }
        }
    }
}