using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models;

public class Workout
{
    [Required]
    public string? Id { get; set; }

     //order should matter
    [ForeignKey("ExerciseSetId")]
    public List<ExerciseSet>? ExerciseSets { get; set; }

}
