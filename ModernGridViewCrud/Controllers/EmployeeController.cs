using Microsoft.AspNetCore.Mvc;
using BLL;
using DAL;

namespace ModernGridViewCrud.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeSqlRepository _repository;

        public EmployeeController(EmployeeSqlRepository repository)
        {
            _repository = repository;
        }

        // GET: Employee
        public IActionResult Index()
        {
            var employees = _repository.GetAllEmployees();
            return View(employees);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    employee.EmployeeId = Guid.NewGuid().ToString();
                    _repository.SaveEmployee(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes: " + ex.Message);
                }
            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _repository.GetEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.UpdateEmployee(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to update: " + ex.Message);
                }
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = _repository.GetEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _repository.DeleteEmployee(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
