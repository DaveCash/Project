﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Text;
namespace KanProject
{

    public partial class WebUserControl2 : System.Web.UI.UserControl
    {
        sqlHelp sh = new sqlHelp();
        protected void Page_Load(object sender, EventArgs e)
        {
            string projectPath = @"|DataDirectory|\AccessDB.mdb;";
            string conStr = "Provider = Microsoft.Jet.OLEDB.4.0;" + "Data Source = " + projectPath;
            OleDbConnection Connection = new OleDbConnection();

            Connection = new OleDbConnection();
            Connection.ConnectionString = conStr;
            Connection.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = Connection;
            cmd.CommandText = "SELECT TaskName,TaskUser  FROM Task";
            cmd.CommandType = CommandType.Text;

            OleDbCommand cmd1 = new OleDbCommand();
            cmd1.Connection = Connection;
            cmd1.CommandText = "SELECT UserName FROM UserData";
            cmd1.CommandType = CommandType.Text;

            OleDbDataReader myReader = cmd.ExecuteReader();
            OleDbDataReader myReader1 = cmd1.ExecuteReader();
            bool notEoF;
            notEoF = myReader.Read();

            //while (notEoF)
            //{
            //    listTask.Items.Add(myReader["TaskName"].ToString());
            //    listUser.Items.Add(myReader["TaskUser"].ToString());
            //    notEoF = myReader.Read();
            //}
            myReader.Close();
            Connection.Close();
        }

        protected void submit_Click(object sender, EventArgs e)
        {
           
            try
            {
                if (!Regex.IsMatch(Complexity.Text.Trim(), @"^\d+$"))
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "alert(\"Complexity只能是整数\");", true);
                    return;
                }
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(TaskId.Value))
                {
                    sb.Append("INSERT INTO Task(ProjectId,RowIndex,ColIndex,TaskDetail,TaskComplexity,TaskUser,TaskName)" + " VALUES(" + projectId.Value + ",1," + colIndex.Value + ",'" + taskDes.Text + "'," + Complexity.Text + ",0,'" + txtTaskName.Text + "')");
                }
                else
                {
                    sb.Append("update Task set ");
                    sb.Append(" TaskName='" + txtTaskName.Text + "',");
                    sb.Append("TaskDetail='" + taskDes.Text + "',");
                    sb.Append("TaskComplexity=" + Complexity.Text);
                    sb.Append(" where TaskId=" + TaskId.Value);
                }
                sh.excuSql(sb.ToString());
                Page.ClientScript.RegisterStartupScript(GetType(), "", "alert(\"保存成功\");", true);
                Response.Redirect("Default.aspx");
            }
            catch (Exception ex)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "", "alert(\"" + ex.Message + "\");", true);
            }
           

        }

        protected void listTask_SelectedIndexChanged(object sender, EventArgs e)
        {

            string projectPath = @"|DataDirectory|\Access.mdbDB;";
            string conStr = "Provider = Microsoft.Jet.OLEDB.4.0;" + "Data Source = " + projectPath;
            OleDbConnection Connection = new OleDbConnection();

            Connection = new OleDbConnection();
            Connection.ConnectionString = conStr;
            Connection.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = Connection;
            cmd.CommandText = "SELECT TaskDetail FROM Task WHERE TaskName =" + txtTaskName.Text;
            cmd.CommandType = CommandType.Text;

            OleDbDataReader myReader = cmd.ExecuteReader();
            bool notEoF;
            notEoF = myReader.Read();

            if (myReader != null)
            {
                taskDes.Text = myReader.ToString();
            }
        }
    }
}