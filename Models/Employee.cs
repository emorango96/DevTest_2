using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DevTest_2.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Employee name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Employee last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "RFC is required")]
        public string RFC { get; set; }

        [DisplayFormat(DataFormatString ="{0:d}")]
        public DateTime BirthDate { get; set; }
        public EmployeeStatus Status { get; set; }

        public string ValidateFields()
        {
            return !ValidateRFC() ? "RFC is not in a valid format" : null;
        }

        public bool ValidateRFC()
        {
            string rfcRegex = @"^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1]))((-)?([A-Z\d]{3}))?$";

            return Regex.IsMatch(RFC ?? "", rfcRegex);
        }
    }

    public enum EmployeeStatus
    {
        NotSet,
        Active,
        Inactive,
    }    

    public class EmployeeDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
    }
}