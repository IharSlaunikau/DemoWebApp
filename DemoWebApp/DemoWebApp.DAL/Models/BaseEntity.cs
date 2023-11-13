using System.ComponentModel.DataAnnotations;

namespace DemoWebApp.DAL.Models;

public abstract class BaseEntity
{
    [Key]
    public abstract int Id { get; set; }
}
