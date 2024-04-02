using System.ComponentModel.DataAnnotations;

namespace SampleAppApi.Model
{
    public class User
    {
        [Required]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public Guid? RoleId { get; set; }
        public virtual Role? Role{get; set;} 

    }
}
