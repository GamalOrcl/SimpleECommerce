﻿namespace SimpleECommerce.Core.Models
{
    public class UpdateUserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
    }
}
