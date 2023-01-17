using CurdOperation.data;
using CurdOperation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CurdOperation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SchoolDbContext _context;
        public HomeController(ILogger<HomeController> logger, SchoolDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            StudentModel studentModel = new StudentModel();
            studentModel.StudentsList = new List<Student>();
            var data = _context.StudentDetails.ToList();

            foreach (var item in data)
            {
                studentModel.StudentsList.Add(
                    new Student{ 
                        Id = (int?)item.Id,
                        Name =item.Name,
                        Subject = item.Subject,
                        Standard = item.Standard
                });
            }

            return View(studentModel);
        }

        [HttpGet]
        public IActionResult Save() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Save(Student student)
        {
            try
            {
                var input = new StudentDetail()
                {
                    Id = (decimal)student.Id,
                    Name = student.Name,
                    Subject = student.Subject,
                    Standard = student.Standard
                };

                _context.StudentDetails.Add(input);
                _context.SaveChanges();
                TempData["SaveStatus"] = 1;
            }
            catch (Exception ex) {
                TempData["SaveStatus"] = 0;
            }
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult Update(int Id = 0)
        {
            Student student = new Student();
            var data = _context.StudentDetails.Where(m => m.Id == Id).FirstOrDefault();
            if (data != null)
            {
                student.Subject = data.Subject;
                student.Standard = data.Standard;
                student.Id = (int?)data.Id;
                student.Name = data.Name;
            }
            return View(student);
        }
        [HttpPost]
        public IActionResult Update(Student student)
        {
            try
            {
                var data = _context.StudentDetails.Where(m => m.Id == student.Id).FirstOrDefault();
                data.Name = student.Name;
                data.Subject = student.Subject;
                data.Standard = student.Standard;
                _context.SaveChanges();
                TempData["UpdateStatus"] = 1;
            }
            catch
            {
                TempData["UpdateStatus"] = 0;
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Delete(int id) 
        {
            try 
            {
                var student = _context.StudentDetails.FirstOrDefault(s => s.Id == id);
                if (student != null)
                {
                    _context.StudentDetails.Remove(student);
                    _context.SaveChanges();
                    TempData["SaveStatus"] = 1;
                }
                else
                {
                    TempData["SaveStatus"] = 0; 
                }
                
            }catch(Exception ex)
            {
                TempData["SaveStatus"] = -1;
            }

            return RedirectToAction("Index", "Home");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}