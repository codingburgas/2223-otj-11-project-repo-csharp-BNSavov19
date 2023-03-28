using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models;
public class ExerciseSet
{
    [Required]
    public int? Id { get; set; }

    [Required]
    public int? reps { get; set; }

    [ForeignKey("ExerciseId")]
    public Exercise? exercise { get; set; }
}

