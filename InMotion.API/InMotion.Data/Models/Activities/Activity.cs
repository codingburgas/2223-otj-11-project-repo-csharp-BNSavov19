using InMotion.Data.Models.Auth;
using System.ComponentModel.DataAnnotations.Schema;

namespace InMotion.Data.Models.Activities;

public class Activity
{
    public string? Id { get; set; }

    public string? UserId { get; set; }

    public string? Description { get; set; }

    public ActivityType Type { get; set; }

    public DateOnly DueDate { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    public bool? IsCompleted { get; set; }
}
