//using EmployeeMangements.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Linq.Dynamic.Core;
//using Microsoft.AspNetCore.Identity;

//namespace EmployeeMangements.Controllers
//{
//    [Route("api")]
//  //  [ApiController]
//    public class UsersController : ControllerBase
//    {
//        private readonly AppDbContext context;
//        private readonly UserManager<ApplicationUser> userManager;
//        private readonly SignInManager<ApplicationUser> signInManager;

//        public UsersController(AppDbContext context,
//            UserManager<ApplicationUser> userManager,
//            SignInManager<ApplicationUser> signInManager)
//        {
//            this.context = context;
//            this.userManager = userManager;
//            this.signInManager = signInManager;
//        }
//        [HttpDelete("users/{id}")]
//        public async Task<IActionResult> DeleteStudent(string id)
//        {
//            var result = await userManager.DeleteAsync(await userManager.FindByIdAsync(id));
//            if (result.Succeeded)
//            {
//                return Ok();
//            }
//            return BadRequest();
//        }
//        [HttpPost("users")]
//        public IActionResult GetUsers()
//        {
//            var pageSize = int.Parse(Request.Form["length"]);

//            // get skiped records
//            var skip = int.Parse(Request.Form["start"]);

//            // get search term
//            var searchTerm = Request.Form["search[value]"];

//            // get order direction
//            var orderDirection = Request.Form["order[0][dir]"];

//            // get clicked column index to order
//            var columnIndex = Request.Form["order[0][column]"];

//            // get clicked column name to order
//            var columnName = Request.Form["columns[" + columnIndex + "][name]"];

//            // apply filtering with search term if it has value
//            IQueryable<ApplicationUser> users = context.Users.Where(m => string.IsNullOrEmpty(searchTerm)
//                ? true
//                : m.Email.Contains(searchTerm) ||
//                m.UserName.Contains(searchTerm) ||
//                m.City.Contains(searchTerm));



//            // apply ordering
//            if (!(string.IsNullOrEmpty(orderDirection) && string.IsNullOrEmpty(columnIndex) && string.IsNullOrEmpty(columnName)))
//            {
//                users = users.OrderBy($"{columnName} {orderDirection}");
//            }

//            // get total records count
//            var recordsTotal = users.Count();

//            // implements pagination
//            var data = users.Skip(skip).Take(pageSize).ToList();

//            // return json object as a result
//            var jsonData = new
//            {
//                recordsFiltered = recordsTotal,
//                recordsTotal,
//                data
//            };

//            return Ok(jsonData);
//        }
//    }
//    }

