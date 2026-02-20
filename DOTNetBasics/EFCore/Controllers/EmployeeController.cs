using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EMSDBContext _context;

        public EmployeeController(EMSDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = _context.Employees.ToList();

            var emps = _context.Employees.AsNoTracking().ToList();

            //_context = null; if it was not readonly someone can accidently modify this

            //AoNoTracking
            //This is useful when we are only reading data and we don't need to update it,
            //it will improve performance and reduce memory usage
            //When we use AsNoTracking we cannot update the entities, we cannot change the state of the entities,
            //we cannot use the ChangeTracker to track the entities, we cannot use the SaveChanges to save the changes to the database, we cannot use the Update to update the entities, we cannot use the Remove to remove the entities, we cannot use the Add to add new entities, we cannot use the Attach to attach existing entities, we cannot use the Entry to get the entry of the entities, we cannot use the Find to find the entities, we cannot use the FirstOrDefault to get the first or default entity, we cannot use the SingleOrDefault to get the single or default entity, we cannot use the ToList to get the list of entities, we cannot use the ToArray to get the array of entities, we cannot use the ToDictionary to get the dictionary of entities, we cannot use the ToHashSet to get the hashset of entities, we cannot use the ToLookup to get the lookup of entities, we cannot use the ToObservableCollection to get the observable collection of entities, we cannot use the ToAsyncEnumerable to get the async enumerable of entities, we cannot use the ToAsyncEnumerable to get the async enumerable of entities
            //When we use AsNoTracking we can use the AsNoTrackingWithIdentityResolution to track the entities

            return Ok(emps);
        }


        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpGet("HighlyPaid")]
        public IActionResult HighlyPaidEmployees()
        {

            // This example demonstrate difference between IEnumerable and IQueryable
            // both are deferred
            // but the difference is where is filter/query is executed
            // you can view the actual queries that is getting executed in console as we have added support for
            // private readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(config => config.AddConsole());


            IEnumerable<Employee> emps = _context.Employees.Where(e => e.Salary > 1000);

            emps = emps.Take(1);

            foreach (var item in emps)
            {

            }

            IQueryable<Employee> employees = _context.Employees.Where(e => e.Salary > 1000);


            employees = employees.Take(1);

            foreach (var item in employees)
            {

            }

            return Ok(employees);
        }

        [HttpPost]
        public IActionResult CreateEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetEmployees), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, Employee employee)
        {
            var existingEmployee = _context.Employees.Find(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }
            existingEmployee.Name = employee.Name;
            existingEmployee.Salary = employee.Salary;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return NoContent();
        }


    }
}
