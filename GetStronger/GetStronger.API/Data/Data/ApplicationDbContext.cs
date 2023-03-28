using Data.Models;
using Data.Models.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data;
internal class ApplicationDbContext: IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Exercise>? Exercise { get; set; }

    public virtual DbSet<ExerciseSet>? ExerciseSet { get; set; }

    public virtual DbSet<Workout>? Workout { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .Entity<ExerciseSet>()
            .HasOne(es => es.exercise); //how exercise id

        builder
            .Entity<Workout>()
            .HasMany(w => w.ExerciseSets)
            .WithOne()
            .HasForeignKey(es => es.exercise)
            .OnDelete(DeleteBehavior.Cascade);
    }


}
