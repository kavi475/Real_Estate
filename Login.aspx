<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication1.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link rel="stylesheet" href="/css/login.css" />
</head>

<body>
    <div class="form-container">
        <h1>Sign In</h1>

        <form id="form1" runat="server">
            <table border="0">

                <!-- Email -->
                <tr>
                    <td>
                        <asp:TextBox 
                            ID="txt_email" 
                            runat="server"
                            Placeholder="Enter your email"
                            AutoPostBack="True"
                            OnTextChanged="txt_email_TextChanged">
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

                <!-- Role -->
                <tr>
                    <td>
                        <asp:DropDownList ID="ddl_role" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>

                <!-- Login Button -->
                <tr>
                    <td>
                        <asp:Button 
                            ID="btn_login" 
                            runat="server" 
                            Text="Login" 
                            OnClick="btn_login_Click" />
                    </td>
                </tr>

                <!-- Register -->
                <tr>
                    <td>
                        Don’t have an account?
                        <a href="Register.aspx" id="link_reigster">Register</a>
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
