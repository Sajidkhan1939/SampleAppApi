using System.ComponentModel.DataAnnotations;

namespace SampleAppApi.Model
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
