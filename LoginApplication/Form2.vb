Imports System.Data
Imports System.Data.SqlClient
Public Class Form2
    Dim con As SqlConnection = New SqlConnection("Data Source=DESKTOP-QB4O65F\SQLEXPRESS;Initial Catalog=LoginDB;Integrated Security=True")
    Dim cmd As SqlCommand = New SqlCommand
    Dim i As Integer
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd.Connection = con
        cmd = con.CreateCommand()
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "UPDATE TaskInfo SET IsActive = 0 WHERE Id = " & i & ""
        cmd.ExecuteNonQuery()
        MessageBox.Show("Delete Successfull", "information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        txtEmpName.Clear()
        txtTaskName.Clear()
        txtDescription.Clear()
        txtEmpName.Focus()
        ShowData()
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim con As SqlConnection = New SqlConnection("Data Source=DESKTOP-QB4O65F\SQLEXPRESS;Initial Catalog=LoginDB;Integrated Security=True")
        Dim cmd As SqlCommand = New SqlCommand
        cmd.Connection = con
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        ShowData()
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd.Connection = con
        cmd = con.CreateCommand()
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "UPDATE TaskInfo SET EmpName = '" + txtEmpName.Text + "', TaskName = '" + txtTaskName.Text + "', Description = '" + txtDescription.Text + "' WHERE Id = " & i & ""
        cmd.ExecuteNonQuery()
        MessageBox.Show("Update Successfull", "information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        txtEmpName.Clear()
        txtTaskName.Clear()
        txtDescription.Clear()
        txtEmpName.Focus()
        ShowData()

    End Sub

    Public Sub ShowData()
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        con.Open()
        cmd.Connection = con
        cmd = con.CreateCommand()
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT T.Id,T.EmpName,T.TaskName,T.Description FROM TaskInfo T WHERE T.IsActive = 1"
        cmd.ExecuteNonQuery()
        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dt)
        DataGridView1.DataSource = dt
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con.Open()

            i = Convert.ToInt32(DataGridView1.SelectedCells.Item(0).Value.ToString())

            cmd.Connection = con
            cmd = con.CreateCommand()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT T.Id,T.EmpName,T.TaskName,T.Description FROM TaskInfo T WHERE T.Id = " & i & ""
            cmd.ExecuteNonQuery()
            Dim dt As New DataTable()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
            Dim dr As SqlClient.SqlDataReader
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While dr.Read
                txtEmpName.Text = dr.GetString(1).ToString()
                txtTaskName.Text = dr.GetString(2).ToString()
                txtDescription.Text = dr.GetString(3).ToString()
            End While

        Catch ex As Exception

        End Try

    End Sub
End Class