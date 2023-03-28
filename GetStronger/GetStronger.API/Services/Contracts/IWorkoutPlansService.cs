using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Services.Contracts;
public interface IWorkoutPlansService
{
    Task<Dictionary<string, Workout>> GetUserWorkoutsPlanAsync(string userId);

    Task EditUserWorkoutsPlanAsync(string workoutPlan, Dictionary<string, Workout> newWorkoutsPlan);
}