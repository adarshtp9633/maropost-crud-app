using Microsoft.AspNetCore.Mvc;
using ModernGridViewCrud.Models;
using ModernGridViewCrud.Data;
using System.Diagnostics;

namespace ModernGridViewCrud.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmployeeSqlRepository _repository;

        public HomeController(ILogger<HomeController> logger, EmployeeSqlRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            var dataSet = _repository.GetAllEmployees();
            var employees = new List<Employee>();

            foreach (System.Data.DataRow row in dataSet.Tables[0].Rows)
            {
                employees.Add(new Employee
                {
                    EmployeeId = row["EmployeeId"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    Designation = row["Designation"].ToString(),
                    DateOfJoining = row["DateOfJoining"].ToString(),
                    Gender = row["Gender"].ToString(),
                    Qualification = row["Qualification"].ToString(),
                    State = row["State"].ToString()
                });
            }

            return View(employees);
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.EmployeeId = Guid.NewGuid().ToString();
                _repository.SaveEmployee(employee);
                return RedirectToAction("Index");
            }
            // Reload list if validation fails
            var dataSet = _repository.GetAllEmployees();
            var employees = new List<Employee>();
             foreach (System.Data.DataRow row in dataSet.Tables[0].Rows)
            {
                 employees.Add(new Employee
                {
                    EmployeeId = row["EmployeeId"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    Designation = row["Designation"].ToString(),
                    DateOfJoining = row["DateOfJoining"].ToString(),
                    Gender = row["Gender"].ToString(),
                    Qualification = row["Qualification"].ToString(),
                    State = row["State"].ToString()
                });
            }
            return View("Index", employees);
        }

        public IActionResult Delete(string id)
        {
            _repository.DeleteEmployee(id);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
