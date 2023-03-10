using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Mappings;

namespace TatBlog.Data.Contexts;

public class BlogDbContext : DbContext
{
    #region DbSet
    public DbSet<Author> Authors { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Tag> Tags { get; set; }
    #endregion

    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {

    }

    public BlogDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Chuỗi kết nối
        optionsBuilder.UseSqlServer(@"Server=None;Database=TatBlog;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryMap).Assembly);
    }
}
