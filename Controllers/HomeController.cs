using EmployeeMangements.Models;
using EmployeeMangements.viewModles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment hostingEnvironment)
        {
            this.employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        // GET: HomeController1
        [AllowAnonymous]
        public ActionResult Index()
        {
            var emp = employeeRepository.GetAllEmployee();
            return View(emp);
        }

        // GET: HomeController1/Details/5
        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            

            Employee employee = employeeRepository.GetEmployee(id.Value);
            if(employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound",id.Value);
            }
            HomeDetailsViewModel homeDetailsViewModel = new()
            {
                Employee = employee,
                PageTitle = "Employee Details"
            };
            return View(homeDetailsViewModel);


        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeCreateViewModel employee)
        {
            if(ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(employee);
                Employee newEmployee = new()
                {
                    Name = employee.Name,
                    Email = employee.Email,
                    Department = employee.Department,
                    PhotoPath =uniqueFileName,
                };
                employeeRepository.Add(newEmployee);
                return RedirectToAction("details",new { id = newEmployee.Id });
            }
                return View();
            
        }
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new()
            {
                Id=employee.Id,
                Name =employee.Name,
                Email =employee.Email,
                Department =employee.Department,
                ExistingPhotoPath =employee.PhotoPath,
            };
            return View(employeeEditViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeEditViewModel employee)
        {
            
            if (ModelState.IsValid)
            {
                Employee employee1 = employeeRepository.GetEmployee(employee.Id);
                employee1.Name = employee.Name;
                employee1.Email = employee.Email;
                employee1.Department = employee.Department; 
                if (employee.Photo != null) 
                {
                    if(employee.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "images", employee.ExistingPhotoPath);  
                        System.IO.File.Delete(filePath);

                    }
                    employee1.PhotoPath = ProcessUploadedFile(employee);
                }
                
                employeeRepository.Update(employee1);
                return RedirectToAction("index");
            }
            return View();

        }

        public ActionResult Delete(int id)
        {
            Employee employee = employeeRepository.GetEmployee(id);
            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Employee employee)
        {
            employeeRepository.Delete(employee.Id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult setLanguage(string culture,string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires =DateTimeOffset.UtcNow.AddYears(1)}
                );
            return LocalRedirect(returnUrl);
        }
        private string ProcessUploadedFile(EmployeeCreateViewModel employee)
        {
            string uniqueFileName = null;
            if (employee.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + employee.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using( var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    employee.Photo.CopyTo(fileStream);
                }// fileStreem.close();
                  
            }

            return uniqueFileName;
        }
    }
}
