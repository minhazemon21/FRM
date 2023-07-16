using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ADO_Example.Models;

namespace ADO_Example.DAL
{
    public class Employee_DAL
    {
        string conString = ConfigurationManager.ConnectionStrings["adoConnection"].ToString();

        //get all Employee

        public List<Employee> GetAllEmployee()
        {
            List<Employee> EmployeeList = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand command= conn.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_Get_All_Employee";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable empTable = new DataTable();
                conn.Open();
                sqlDA.Fill(empTable);
                conn.Close();

                foreach(DataRow dr in empTable.Rows)
                {
                    EmployeeList.Add(new Employee
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        FatherName = dr["FatherName"].ToString(),
                        MotherName = dr["MotherName"].ToString(),
                        Address = dr["Address"].ToString(),
                        PhoneNo = dr["PhoneNo"].ToString(),
                        Salary = Convert.ToDecimal(dr["Salary"]),
                        Age = Convert.ToInt32(dr["Age"]),
                        //CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                        

                    });
                }
            }
            return EmployeeList;
        }
        public bool InsertEmployee(Employee emp)
        {
            int Id = 0;
            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("USP_Insert_Employee", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", emp.Name);
                command.Parameters.AddWithValue("@FName", emp.FatherName);
                command.Parameters.AddWithValue("@MName", emp.MotherName);
                command.Parameters.AddWithValue("@Address", emp.Address);
                command.Parameters.AddWithValue("@PhoneNo", emp.PhoneNo);
                command.Parameters.AddWithValue("@Salary", emp.Salary);
                command.Parameters.AddWithValue("@Age", emp.Age);

                conn.Open();
                Id = command.ExecuteNonQuery();
                conn.Close();

            }
            return true;
        }
    }
}