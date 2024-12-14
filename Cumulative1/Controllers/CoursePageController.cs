using Cumulative1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative1.Controllers
{
    public class CoursePageController : Controller
    {
        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }

        public IActionResult List()
        {
            List<Course> Courses = _api.ListCourse();
            return View(Courses);
        }
        public IActionResult Show(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Course NewCourse)
        {
            int Id = _api.AddCourse(NewCourse);


            return RedirectToAction("Show", new { id = Id });
        }

        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            int CId = _api.DeleteCourse(id);
            // redirects to list action
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Fetch the course using the API
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }

        [HttpPost]
        public IActionResult Update(int id, string CourseCode, int TeacherId, DateTime StartDate, DateTime FinishDate, string CourseName)
        {

            Course UpdatedCourse = new Course();

            UpdatedCourse.Ccode = CourseCode;
            UpdatedCourse.Tid = TeacherId;
            UpdatedCourse.startdate = StartDate;
            UpdatedCourse.finishdate = FinishDate;
            UpdatedCourse.Cname = CourseName;


            // Update the course using the API (assuming _api.UpdateCourse works similarly to the Teacher example)
            _api.UpdateCourse(id, UpdatedCourse);

            // Redirect to the 'Show' action to display the updated course
            return RedirectToAction("Show", new { id = id });
        }
    }
}
