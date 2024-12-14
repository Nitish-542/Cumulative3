using Cumulative1.Models;
using Cumulative1.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Mysqlx.Datatypes;

namespace Cumulative1.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly TeacherAPIController _api;

        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }


        public IActionResult List(DateTime? StartDate, DateTime? EndDate)
        {

            List<Teacher> Teachers = _api.ListTeachers(StartDate, EndDate);
            return View(Teachers);
        }


        [HttpGet]
        public IActionResult Show(int id)
        {
            // Validate the provided teacher ID
            if (id <= 0)
            {
                // Set an error message to be displayed above the content
                ViewBag.ErrorMessage = "Invalid Teacher ID. Please provide a valid ID.";

                // Return an error view if the ID is invalid
                return View("Error");
            }

            // Get the selected teacher from the API
            var selectedTeacher = _api.FindTeacher(id);

            // Check if the teacher exists
            if (selectedTeacher == null)
            {
                // Set an error message to be displayed above the content
                ViewBag.ErrorMessage = "The specified teacher does not exist. Please check the Teacher ID.";

                // Return an error view if the teacher doesn't exist
                return View("Error");
            }

            // Retrieve the courses assigned to the teacher
            var teacherCourses = _api.GetCoursesByTeacher(id);

            // Prepare the view model with the teacher's details and courses
            var viewModel = new TeacherWithCoursesViewModel
            {
                Teacher = selectedTeacher,
                // Set empty list if no courses are found
                Courses = teacherCourses ?? new List<string>()
            };

            // Return the view with the teacher's details and courses
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Teacher NewTeacher)
        {
            int TeacherId = _api.AddTeacher(NewTeacher);


            return RedirectToAction("Show", new { id = TeacherId });
        }


        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            int TeacherId = _api.DeleteTeacher(id);

            // redircts to list action
            return RedirectToAction("List");
        }
        public IActionResult Edit(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }
        [HttpPost]
        [HttpPost]
        public IActionResult Update(int id,string TeacherFName,string TeacherLName,DateTime Hire,string EmployeeNumber,decimal Salary)
        {
            // Perform validation checks
            if (string.IsNullOrWhiteSpace(TeacherFName))
            {
                ModelState.AddModelError("Teacher First Name", "First Name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(TeacherLName))
            {
                ModelState.AddModelError("Teacher List Name", "Last Name cannot be empty.");
            }

            if (Hire > DateTime.Now)
            {
                ModelState.AddModelError("Hire", "Hire Date cannot be in the future.");
            }

            // If there are validation errors, re-render the Edit view with the current model
            if (!ModelState.IsValid)
            {
                // Return the view with validation errors
                var selectedTeacher = new Teacher
                {
                    TeacherFName = TeacherFName,
                    TeacherLName = TeacherLName,
                    Hire = Hire,
                    EmployeeNumber = EmployeeNumber,
                    Salary = Salary
                };
                return View("Edit", selectedTeacher);
            }

            // Proceed to update the teacher if validation passes
            Teacher UpdatedTeacher = new Teacher
            {
                TeacherFName = TeacherFName,
                TeacherLName = TeacherLName,
                Hire = Hire,
                EmployeeNumber = EmployeeNumber,
                Salary = Salary
            };

            _api.UpdateTeacher(id, UpdatedTeacher);

            // Redirect to the Show action after a successful update
            return RedirectToAction("Show", new { id = id });
        }

    }
}
