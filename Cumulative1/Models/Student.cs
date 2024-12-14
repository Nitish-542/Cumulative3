using Microsoft.AspNetCore.Mvc;

namespace Cumulative1.Models
{
    public class Student : Controller
    {
        public int Id { get; set; }

        public string? SFName { get; set; }

        public string? SLName { get; set; }

        public DateTime EnrollDate { get; set; }

        public string? SNumber { get; set; }

    }
}
