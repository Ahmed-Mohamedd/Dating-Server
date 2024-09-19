using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public required string UserName { get; set; }

       

    }
}
