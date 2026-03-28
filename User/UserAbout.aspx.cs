using System;
namespace WebApplication1.User
{
    public partial class UserAbout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                lblWelcome.Text = "Hi, " + Session["email"].ToString();
            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("/Home.aspx");
        }
    }
}