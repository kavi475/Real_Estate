using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebApplication1.Admin
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null || Session["role"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            string role = Session["role"].ToString();
            if (role != "Admin" && role != "1")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
                LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT UserId, Email, Role, Phone, CreatedAt FROM Users", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvUsers.DataSource = dt;
                    gvUsers.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("⚠ Error loading users: " + ex.Message, false);
            }
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            string email = txtNewEmail.Text.Trim();
            string password = txtNewPassword.Text.Trim();
            string role = ddlNewRole.SelectedValue;
            string phone = txtNewPhone.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowMessage("⚠ Email and Password are required.", false);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand chk = new SqlCommand(
                        "SELECT COUNT(*) FROM Users WHERE Email = @Email", con);
                    chk.Parameters.AddWithValue("@Email", email);
                    con.Open();
                    if ((int)chk.ExecuteScalar() > 0)
                    {
                        ShowMessage("⚠ Email already exists.", false);
                        return;
                    }

                    string hashed = HashPassword(password);

                    SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO Users (Email, Password, Role, Phone, CreatedAt)
                          VALUES (@Email, @Password, @Role, @Phone, GETDATE())", con);

                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", hashed);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.ExecuteNonQuery();
                }

                txtNewEmail.Text = "";
                txtNewPassword.Text = "";
                txtNewPhone.Text = "";

                ShowMessage("✔ User added successfully!", true);
                LoadUsers();
            }
            catch (Exception ex)
            {
                ShowMessage("⚠ Error adding user: " + ex.Message, false);
            }
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsers();
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsers();
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvUsers.Rows[e.RowIndex];
                int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

                string email = ((TextBox)row.FindControl("txtGridEmail")).Text.Trim();
                string role = ((DropDownList)row.FindControl("ddlGridRole")).SelectedValue;
                string phone = ((TextBox)row.FindControl("txtGridPhone")).Text.Trim();

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"UPDATE Users SET Email = @Email, Role = @Role, Phone = @Phone
                          WHERE UserId = @UserId", con);

                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                gvUsers.EditIndex = -1;
                ShowMessage("✔ User updated successfully!", true);
                LoadUsers();
            }
            catch (Exception ex)
            {
                ShowMessage("⚠ Error updating user: " + ex.Message, false);
            }
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Users WHERE UserId = @UserId", con);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                ShowMessage("✔ User deleted successfully!", true);
                LoadUsers();
            }
            catch (Exception ex)
            {
                ShowMessage("⚠ Error deleting user: " + ex.Message, false);
            }
        }

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            gvUsers.EditIndex = -1;
            LoadUsers();
        }

        private string HashPassword(string password)
        {
            try { return BCrypt.Net.BCrypt.HashPassword(password); }
            catch { return password; }
        }

        private void ShowMessage(string message, bool success)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = success
                ? System.Drawing.Color.Green
                : System.Drawing.Color.Red;
        }
    }
}