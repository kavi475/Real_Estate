using System;
using System.Configuration;
using System.Data.SqlClient;

namespace WebApplication1.User
{
    public partial class UserProfile : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null) { Response.Redirect("/Login.aspx"); return; }
            if (!IsPostBack)
            {
                lblWelcome.Text = "Hi, " + Session["email"].ToString();
                LoadProfile();
            }
        }

        void LoadProfile()
        {
            string email = Session["email"].ToString();
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(
                "SELECT Email, Phone FROM Users WHERE Email = @email", con);
            cmd.Parameters.AddWithValue("@email", email);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtEmail.Text = dr["Email"].ToString();
                txtPhone.Text = dr["Phone"] != DBNull.Value
                    ? dr["Phone"].ToString() : "";
            }
            con.Close();
        }

        protected void btnUpdateEmail_Click(object sender, EventArgs e)
        {
            string newEmail = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();

            // Validate email
            if (newEmail == "" || !newEmail.Contains("@"))
            {
                lblEmailMsg.ForeColor = System.Drawing.Color.Red;
                lblEmailMsg.Text = "Please enter a valid email.";
                return;
            }

            // Validate phone — optional but if filled must be numeric
            if (phone != "" && phone.Length < 10)
            {
                lblEmailMsg.ForeColor = System.Drawing.Color.Red;
                lblEmailMsg.Text = "Please enter a valid phone number.";
                return;
            }

            string currentEmail = Session["email"].ToString();

            // Check if new email already taken by another user
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand checkCmd = new SqlCommand(
                "SELECT COUNT(*) FROM Users WHERE Email=@email AND Email != @current", con);
            checkCmd.Parameters.AddWithValue("@email", newEmail);
            checkCmd.Parameters.AddWithValue("@current", currentEmail);
            con.Open();
            int count = Convert.ToInt32(checkCmd.ExecuteScalar());
            con.Close();

            if (count > 0)
            {
                lblEmailMsg.ForeColor = System.Drawing.Color.Red;
                lblEmailMsg.Text = "This email is already in use by another account.";
                return;
            }

            // Update email and phone
            SqlConnection con2 = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(
                "UPDATE Users SET Email=@newEmail, Phone=@phone WHERE Email=@current", con2);
            cmd.Parameters.AddWithValue("@newEmail", newEmail);
            cmd.Parameters.AddWithValue("@phone", phone == "" ? (object)DBNull.Value : phone);
            cmd.Parameters.AddWithValue("@current", currentEmail);
            con2.Open();
            cmd.ExecuteNonQuery();
            con2.Close();

            // Update session
            Session["email"] = newEmail;
            lblWelcome.Text = "Hi, " + newEmail;

            lblEmailMsg.ForeColor = System.Drawing.Color.Green;
            lblEmailMsg.Text = "Profile updated successfully.";
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string current = txtCurrentPassword.Text.Trim();
            string newPass = txtNewPassword.Text.Trim();
            string confirm = txtConfirmPassword.Text.Trim();

            if (current == "" || newPass == "" || confirm == "")
            {
                lblPasswordMsg.ForeColor = System.Drawing.Color.Red;
                lblPasswordMsg.Text = "All fields are required.";
                return;
            }
            if (newPass.Length < 7 || newPass.Length > 14)
            {
                lblPasswordMsg.ForeColor = System.Drawing.Color.Red;
                lblPasswordMsg.Text = "Password must be 7-14 characters.";
                return;
            }
            if (newPass != confirm)
            {
                lblPasswordMsg.ForeColor = System.Drawing.Color.Red;
                lblPasswordMsg.Text = "Passwords do not match.";
                return;
            }

            string email = Session["email"].ToString();
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand checkCmd = new SqlCommand(
                "SELECT COUNT(*) FROM Users WHERE Email=@email AND Password=@pass", con);
            checkCmd.Parameters.AddWithValue("@email", email);
            checkCmd.Parameters.AddWithValue("@pass", current);
            con.Open();
            int count = Convert.ToInt32(checkCmd.ExecuteScalar());
            con.Close();

            if (count == 0)
            {
                lblPasswordMsg.ForeColor = System.Drawing.Color.Red;
                lblPasswordMsg.Text = "Current password is incorrect.";
                return;
            }

            SqlConnection con2 = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(
                "UPDATE Users SET Password=@newPass WHERE Email=@email", con2);
            cmd.Parameters.AddWithValue("@newPass", newPass);
            cmd.Parameters.AddWithValue("@email", email);
            con2.Open();
            cmd.ExecuteNonQuery();
            con2.Close();

            lblPasswordMsg.ForeColor = System.Drawing.Color.Green;
            lblPasswordMsg.Text = "Password changed successfully.";
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear(); Session.Abandon();
            Response.Redirect("/Login.aspx");
        }
    }
}