<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Trace="true" Async="true"
         CodeBehind="Default.aspx.cs" Inherits="WorkflowASPNet._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <p>
        This page invokes a Workflow and a WCF Service using the delay as shown. For more information see <a href="http://blogs.msdn.com/b/rjacobs/archive/2011/03/16/asp-net-wf4-wcf-and-async-calls.aspx">ASP.NET WF4 / WCF and Async Calls</a> on <a href="http://www.ronjacobs.com">Ron Jacobs blog</a>
    </p>
    <table>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="CheckBoxAsync" Text="Run Asynchronously" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Delay (milliseconds):</td>
            <td><asp:Textbox ID="txtDelay" runat="server">1000</asp:Textbox></td>
            <td>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDelay" ErrorMessage="Delay is required" />
                <asp:CompareValidator runat="server" ControlToValidate="txtDelay"
                                      ErrorMessage="Value must be an integer" Operator="DataTypeCheck" Type="Integer" />
            </td>
        </tr>
        <tr>
            <td>Name</td>
            <td><asp:TextBox runat="server" ID="TextBoxName">Your Name</asp:TextBox></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:LinkButton runat="server" ID="LinkButtonRunWorkflow" 
                                onclick="LinkButtonSayHello">Say Hello</asp:LinkButton></td>
        </tr>
        <tr>
            <td>Greeting:</td>
            <td><asp:Label runat="server" ID="labelGreeting" /></td>
        </tr>
        <tr>
            <td>Service Delay:</td>
            <td><asp:Label runat="server" ID="labelDelay" /></td>
        </tr>
    </table>
</asp:Content>