using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using BLL;

namespace DAL
{
    public class EmployeeSqlRepository
    {
        private readonly string _connectionString;

        public EmployeeSqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SaveEmployee(Employee employee)
        {
            using (var objConnection = new SqlConnection(_connectionString))
            {
                objConnection.Open();
                var objCommand = new SqlCommand("sp_Employee", objConnection);
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Parameters.AddWithValue("@Action", "Insert");
                objCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                objCommand.Parameters.AddWithValue("@FirstName", employee.FirstName);
                objCommand.Parameters.AddWithValue("@LastName", employee.LastName);
                objCommand.Parameters.AddWithValue("@Designation", employee.Designation);
                objCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                objCommand.Parameters.AddWithValue("@Gender", employee.Gender);
                objCommand.Parameters.AddWithValue("@Qualification", employee.Qualification);
                objCommand.Parameters.AddWithValue("@State", employee.State);
                objCommand.ExecuteNonQuery();
            }
        }

        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();
            using (var objConnection = new SqlConnection(_connectionString))
            {
                objConnection.Open();
                var objCommand = new SqlCommand("sp_Employee", objConnection);
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Parameters.AddWithValue("@Action", "GetAllEmp");
                
                using (var reader = objCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            EmployeeId = reader["EmployeeId"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Designation = reader["Designation"].ToString(),
                            DateOfJoining = reader["DateOfJoining"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            Qualification = reader["Qualification"].ToString(),
                            State = reader["State"].ToString()
                        });
                    }
                }
            }
            return employees;
        }

        // Adapted GetEmployee to return Employee object instead of DataSet for better MVC integration
        public Employee GetEmployee(string employeeId)
        {
            Employee employee = null;
            using (var objConnection = new SqlConnection(_connectionString))
            {
                objConnection.Open();
                var objCommand = new SqlCommand("sp_Employee", objConnection);
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Parameters.AddWithValue("@Action", "GetEmp"); // Assuming 'GetEmp' returns a single row
                objCommand.Parameters.AddWithValue("@EmployeeId", employeeId);

                using (var reader = objCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            EmployeeId = reader["EmployeeId"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Designation = reader["Designation"].ToString(),
                            DateOfJoining = reader["DateOfJoining"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            Qualification = reader["Qualification"].ToString(),
                            State = reader["State"].ToString()
                        };
                    }
                }
            }
            return employee;
        }

        public void UpdateEmployee(Employee employee)
        {
            using (var objConnection = new SqlConnection(_connectionString))
            {
                objConnection.Open();
                var objCommand = new SqlCommand("sp_Employee", objConnection);
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Parameters.AddWithValue("@Action", "Update");
                objCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                objCommand.Parameters.AddWithValue("@FirstName", employee.FirstName);
                objCommand.Parameters.AddWithValue("@LastName", employee.LastName);
                objCommand.Parameters.AddWithValue("@Designation", employee.Designation);
                objCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                objCommand.Parameters.AddWithValue("@Gender", employee.Gender);
                objCommand.Parameters.AddWithValue("@Qualification", employee.Qualification);
                objCommand.Parameters.AddWithValue("@State", employee.State);
                objCommand.ExecuteNonQuery();
            }
        }

        public void DeleteEmployee(string employeeId)
        {
            using (var objConnection = new SqlConnection(_connectionString))
            {
                objConnection.Open();
                var objCommand = new SqlCommand("sp_Employee", objConnection);
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Parameters.AddWithValue("@Action", "Delete");
                objCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                objCommand.ExecuteNonQuery();
            }
        }
    }
}
