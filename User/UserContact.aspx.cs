using System;
namespace WebApplication1.User
{
    public partial class UserContact : System.Web.UI.Page
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "Thank you! Your message has been sent successfully.";
            lblMessage.Visible = true;
            txtName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtSubject.Text = "";
            txtMessage.Text = "";
        }
    }
}