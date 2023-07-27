using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data;
public class AppDbContext : DbContext
{
    public required DbSet<Todo> Todos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=todos.db; Cache=Shared");

}