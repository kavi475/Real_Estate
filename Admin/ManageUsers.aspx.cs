using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        String strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        protected void gv_user_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gv_user.Rows[e.RowIndex];
            int UserId = Convert.ToInt16(row.Cells[1].Text);
            String query = "DELETE FROM Users WHERE UserId = @UserId";
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", UserId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            BindGrid();
        }

        protected void gv_user_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gv_user.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gv_user_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int UserId = Convert.ToInt32(gv_user.DataKeys[e.RowIndex].Value);

            GridViewRow row = gv_user.Rows[e.RowIndex];

            string email = ((TextBox)row.Cells[2].Controls[0]).Text;
            string password = ((TextBox)row.Cells[3].Controls[0]).Text;
            string role = ((TextBox)row.Cells[4].Controls[0]).Text;

            SqlConnection con = new SqlConnection(strcon);
            string query = "UPDATE Users SET Email=@Email, Password=@Password,Role=@Role WHERE UserId=@UserId";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Role", role);
            cmd.Parameters.AddWithValue("@UserId", UserId);


            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            gv_user.EditIndex = -1;
            BindGrid();
        }

        public void BindGrid()
        {
            SqlConnection con = new SqlConnection(strcon);
            String query = "SELECT * FROM Users";
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            gv_user.DataSource = ds;
            gv_user.DataBind();
            con.Close();
        }

        protected void gv_user_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gv_user.EditIndex = -1;
            BindGrid();
        }
    }
}