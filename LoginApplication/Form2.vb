Imports System.Data
Imports System.Data.SqlClient
Public Class Form2
    Dim con As SqlConnection = New SqlConnection("Data Source=DESKTOP-QB4O65F\SQLEXPRESS;Initial Catalog=LoginDB;Integrated Security=True")
    Dim cmd As SqlCommand = New SqlCommand
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim con As SqlConnection = New SqlConnection("Data Source=DESKTOP-QB4O65F\SQLEXPRESS;Initial Catalog=LoginDB;Integrated Security=True")
        Dim cmd As SqlCommand = New SqlCommand
        cmd.Connection = con
        cmd.CommandText = "USP_INSERT_TASK"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@EmpName", txtEmpName.Text)
        cmd.Parameters.AddWithValue("@TaskName", txtTaskName.Text)
        cmd.Parameters.AddWithValue("@Description", txtDescription.Text)
        con.Open()
        cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Save Successfull", "information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        txtEmpName.Clear()
        txtTaskName.Clear()
        txtDescription.Clear()
        txtEmpName.Focus()
        ShowData()
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Using con As SqlConnection = New SqlConnection("Data Source=DESKTOP-QB4O65F\SQLEXPRESS;Initial Catalog=LoginDB;Integrated Security=True")
            con.Open()
            ShowData()
        End Using
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub

    Public Sub ShowData()
        cmd.Connection = con
        cmd = con.CreateCommand()
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT T.Id,T.EmpName,T.TaskName,T.Description FROM TaskInfo T"
        cmd.ExecuteNonQuery()
        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dt)
        DataGridView1.DataSource = dt
    End Sub
End Class