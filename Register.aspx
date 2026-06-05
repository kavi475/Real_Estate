<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebApplication1.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register</title>
    <link rel="stylesheet" href="/css/Register.css" />
     <link rel="stylesheet" href="CSS/login.css" />
</head>

<body>
    <div class="form-container">
        <h2>Create Account</h2>

        <form id="form1" runat="server">
            <table border="0">

                <!-- Email -->
                <tr>
                    <td>
                        <asp:TextBox
                            ID="txt_email"
                            runat="server"
                            Placeholder="Enter your email">
                        </asp:TextBox>
                    </td>
                </tr>

                <!-- Password -->
                <tr>
                    <td>
                        <asp:TextBox
                            ID="txt_password"
                            runat="server"
                            TextMode="Password"
                            Placeholder="Enter your password">
                        </asp:TextBox>
                    </td>
                </tr>

                <!-- Confirm Password -->
                <tr>
                    <td>
                        <asp:TextBox
                            ID="txt_cpassword"
                            runat="server"
                            TextMode="Password"
                            Placeholder="Confirm your password">
                        </asp:TextBox>
                    </td>
                </tr>

                <!-- Role -->
                <tr>
                    <td>
                        <asp:DropDownList ID="ddl_role" runat="server">
                            <asp:ListItem>Customer</asp:ListItem>
                            <asp:ListItem>Agent</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>

                <!-- Register Button -->
                <tr>
                    <td>
                        <asp:Button
                            ID="btn_register"
                            runat="server"
                            Text="Register"
                            OnClick="btn_register_Click" />
                    </td>
                </tr>

                <tr>
                    <td>Already have an account?
        <a href="login.aspx" id="link_reigster">Log In</a>
                    </td>
                </tr>

                <!-- Message -->
                <tr>
                    <td>
                        <asp:Label
                            ID="lbl_message"
                            runat="server"
                            Visible="False">
                        </asp:Label>
                    </td>
                </tr>

            </table>
        </form>
    </div>
</body>
</html>
