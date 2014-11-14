﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Assign_task.ascx.cs" Inherits="KanProject.WebUserControl2" %>
<asp:Panel ID="Panel1" runat="server">
    <h1>Assign Task</h1>
       <table>
            <tr>
                <td>Member</td>
                <td><asp:DropDownList ID="listUser" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Task</td>
                <td><asp:DropDownList ID="listTask" runat="server" OnSelectedIndexChanged="listTask_SelectedIndexChanged"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Task Description</td>
                <td><asp:TextBox ID="taskDes" runat="server" Height="100px" Width="400px" ></asp:TextBox></td>
            </tr>
       </table><br />
        <asp:Button ID="submit" runat="server" Text="Submit" OnClick="submit_Click"/>
 </asp:Panel>