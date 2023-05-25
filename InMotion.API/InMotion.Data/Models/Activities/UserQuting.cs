using InMotion.Data.Models.Auth;
using System.ComponentModel.DataAnnotations.Schema;

namespace InMotion.Data.Models.Activities;

public class UserQuting
{
    public string? Id { get; set; }

    public string? UserId { get; set; }

    public Quiting Quiting { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}
