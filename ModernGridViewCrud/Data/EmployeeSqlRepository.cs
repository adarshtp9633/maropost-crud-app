using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ModernGridViewCrud.Models;

namespace ModernGridViewCrud.Data
{
    public class EmployeeSqlRepository
    {
        private readonly string _connectionString;

        public EmployeeSqlRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
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

        public DataSet GetAllEmployees()
        {
            using (var objConnection = new SqlConnection(_connectionString))
            {
                objConnection.Open();
                var objCommand = new SqlCommand("sp_Employee", objConnection);
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Parameters.AddWithValue("@Action", "GetAllEmp");
                var objDataSet = new DataSet();
                var objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(objDataSet);
                return objDataSet;
            }
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
