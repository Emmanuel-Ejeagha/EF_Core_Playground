using System;
using InstallEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace InstallEFCore.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    public DbSet<Recipe> Recipes { get; set; }
}
