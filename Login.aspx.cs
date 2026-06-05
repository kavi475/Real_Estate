using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace WebApplication1
{
    public partial class Login : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_message.Text = "";
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            lbl_message.ForeColor = System.Drawing.Color.Red;

            string email = txt_email.Text.Trim();
            string password = txt_password.Text.Trim();

            // 🔹 1. Empty validation
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lbl_message.Text = "⚠ Please enter both email and password.";
                return;
            }

            // 🔹 2. Email format validation
            if (!IsValidEmail(email))
            {
                lbl_message.Text = "⚠ Please enter a valid email address.";
                return;
            }

            // 🔹 3. Check email exists first
            if (!IsEmailExists(email))
            {
                lbl_message.Text = "⚠ Account not found. Please register first.";
                return;
            }

            // 🔹 4. Password check
            if (!ChkPassword(email, password))
            {
                lbl_message.Text = "⚠ Incorrect password. Please try again.";
                return;
            }

            // 🔹 5. Get Role + Agent Status
            var info = GetUserAuthInfo(email);

            // 🔒 Agent approval check
            if (info.Role == "Agent" && info.AgentStatus != "Approved")
            {
                lbl_message.Text = "⚠ Your agent account is pending admin approval.";
                return;
            }

            // 🔹 SUCCESS MESSAGE (optional flash)
            lbl_message.ForeColor = System.Drawing.Color.Green;
            lbl_message.Text = "Login successful! Redirecting...";

            // 🔹 Session
            Session["email"] = email;
            Session["role"] = info.Role;

            // 🔹 Redirect
            if (info.Role == "Admin")
                Response.Redirect("/Admin/AdminDashboard.aspx");
            else if (info.Role == "Agent")
                Response.Redirect("/Agent/Dashboard.aspx");
            else
                Response.Redirect("/User/UserDashboard.aspx");
        }

        // ================= HELPERS =================

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsEmailExists(string email)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Users WHERE Email=@Email", con);

                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

        private bool ChkPassword(string email, string password)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT Password FROM Users WHERE Email=@Email", con);

                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                object hash = cmd.ExecuteScalar();

                if (hash == null) return false;

                return BCrypt.Net.BCrypt.Verify(password, hash.ToString());
            }
        }

        private (string Role, string AgentStatus) GetUserAuthInfo(string email)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT u.Role, ISNULL(a.Status,'Approved') AS Status
                    FROM Users u
                    LEFT JOIN AgentProfiles a ON u.UserId = a.UserId
                    WHERE u.Email = @Email", con);

                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    return (dr["Role"].ToString(), dr["Status"].ToString());
                }
            }

            return ("User", "Approved");
        }
    }
}