using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BCrypt.Net;

namespace WebApplication1
{
    public partial class Register : System.Web.UI.Page
    {
        String strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_register_Click(object sender, EventArgs e)
        {
            if (txt_email.Text == "" || txt_password.Text == "" || txt_cpassword.Text == "")
            {
                lbl_message.Visible = true;
                lbl_message.Text = "All fields are required !!";
                return;
            }

            if (!txt_email.Text.Contains("@"))
            {
                lbl_message.Visible = true;
                lbl_message.Text = "Invalid email format";
                return;
            }

            if (txt_password.Text != txt_cpassword.Text)
            {
                lbl_message.Visible = true;
                lbl_message.Text = "Passwords do not match";
                return;
            }

            if (txt_password.Text.Length < 7 || txt_password.Text.Length > 14)
            {
                lbl_message.Visible = true;
                lbl_message.Text = "Password must be between 7 and 14 characters";
                return;
            }

            if (ExistUser(txt_email.Text.Trim()))
            {
                lbl_message.Visible = true;
                lbl_message.Text = "User already exists. <a href='Login.aspx'>Login here</a>";
                return;
            }

            bool result = InsertUser();

            if (result == true)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                lbl_message.Visible = true;
                lbl_message.Text = "Registration Failed";
            }
        }

        public bool InsertUser()
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txt_password.Text);
            string role = ddl_role.SelectedItem.Text;

            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();

                // 1️⃣ Insert into Users
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Users (Email, Password, Role)
            OUTPUT INSERTED.UserId
            VALUES (@Email, @Password, @Role)", con);

                cmd.Parameters.AddWithValue("@Email", txt_email.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@Role", role);

                int userId = Convert.ToInt32(cmd.ExecuteScalar());

                // 2️⃣ If Agent → create Pending profile
                if (role == "Agent")
                {
                    SqlCommand agentCmd = new SqlCommand(@"
                INSERT INTO AgentProfiles (UserId, Status)
                VALUES (@UserId, 'Pending')", con);

                    agentCmd.Parameters.AddWithValue("@UserId", userId);
                    agentCmd.ExecuteNonQuery();
                }

                return true;
            }
        }

        public bool ExistUser(String email)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                String query = "SELECT COUNT(*) FROM Users WHERE Email=@email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", email.Trim());

                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

                return count > 0;
            }
        }
    }
}