﻿namespace SimpleWebApp.Storage.Models
{
    public class EmployeeUpdate
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
    }
}
