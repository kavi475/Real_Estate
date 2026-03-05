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
          

            if(txt_email.Text == "" || txt_password.Text=="" || txt_cpassword.Text== "")
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

            if(txt_password.Text != txt_cpassword.Text)
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

            bool result = InsterUser();

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

        public bool InsterUser()
        {
            SqlConnection con = new SqlConnection(strcon);
            String query = "INSERT INTO Users(Email,Password,Role)VALUES(@email,@password,@role)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", txt_email.Text);
            cmd.Parameters.AddWithValue("@password", txt_password.Text);
            cmd.Parameters.AddWithValue("@role", ddl_role.SelectedItem.Text);
            con.Open();
            int rows  = cmd.ExecuteNonQuery();
            con.Close();

            return rows > 0;
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
    }
}