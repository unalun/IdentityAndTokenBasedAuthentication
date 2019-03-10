using System.ComponentModel.DataAnnotations;

namespace IdentityAndTokenBasedAuthentication.Entities
{
    public interface IEntity
    {
        [Key]
        int Id { get; set; }
    }
}
