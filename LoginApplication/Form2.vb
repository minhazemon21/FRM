Imports System.Data
Imports System.Data.SqlClient
Public Class Form2
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
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub
End Class