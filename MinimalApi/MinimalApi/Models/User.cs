using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public List<User> Users { get; set; }
        public Category() { Users = new List<User>(); }
    }
    public class User
    {
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(3)]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public int Age { get; set; }
    }
    public class UserDetail : User
    {
        public int Age { get; set; }
        public int CategoryId { get; set; }
    }

    public class Role
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
    }
}
