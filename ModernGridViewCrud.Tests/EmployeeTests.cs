using Xunit;
using BLL;
using System;

namespace ModernGridViewCrud.Tests
{
    public class EmployeeTests
    {
        [Fact]
        public void EmployeeId_ShouldThrowException_WhenNull()
        {
            var employee = new Employee();
            var exception = Assert.Throws<Exception>(() => employee.EmployeeId = null);
            Assert.Equal("Employee Id is null.", exception.Message);
        }

        [Fact]
        public void EmployeeId_ShouldThrowException_WhenEmpty()
        {
            var employee = new Employee();
            var exception = Assert.Throws<Exception>(() => employee.EmployeeId = "");
            Assert.Equal("Employee Id is null.", exception.Message);
        }

        [Fact]
        public void EmployeeId_ShouldSet_WhenValid()
        {
            var employee = new Employee();
            employee.EmployeeId = "123";
            Assert.Equal("123", employee.EmployeeId);
        }

        [Fact]
        public void FirstName_ShouldThrowException_WhenNull()
        {
            var employee = new Employee();
            var exception = Assert.Throws<Exception>(() => employee.FirstName = null);
            Assert.Equal("First Name is Required.", exception.Message);
        }

        [Fact]
        public void LastName_ShouldThrowException_WhenNull()
        {
            var employee = new Employee();
            var exception = Assert.Throws<Exception>(() => employee.LastName = null);
            Assert.Equal("Last Name is Required.", exception.Message);
        }

        [Fact]
        public void Designation_ShouldThrowException_WhenNull()
        {
            var employee = new Employee();
            var exception = Assert.Throws<Exception>(() => employee.Designation = null);
            Assert.Equal("Designation is required. ", exception.Message); // Note the trailing space in original code
        }

        [Fact]
        public void DateOfJoining_ShouldThrowException_WhenNull()
        {
            var employee = new Employee();
            var exception = Assert.Throws<Exception>(() => employee.DateOfJoining = null);
            Assert.Equal("Date of Joining is Required.", exception.Message);
        }

        [Fact]
        public void Gender_ShouldThrowException_WhenNull()
        {
            var employee = new Employee();
            var exception = Assert.Throws<Exception>(() => employee.Gender = null);
            Assert.Equal("Select Gender.", exception.Message);
        }

         [Fact]
        public void Qualification_ShouldThrowException_WhenNull()
        {
            var employee = new Employee();
            var exception = Assert.Throws<Exception>(() => employee.Qualification = null);
            Assert.Equal("Select Qualification", exception.Message);
        }

        [Fact]
        public void State_ShouldThrowException_WhenSelectState()
        {
            var employee = new Employee();
            var exception = Assert.Throws<Exception>(() => employee.State = "Select State");
            Assert.Equal("Please Select State.", exception.Message);
        }

        [Fact]
        public void State_ShouldSet_WhenValid()
        {
            var employee = new Employee();
            employee.State = "Karnataka";
            Assert.Equal("Karnataka", employee.State);
        }
    }
}
