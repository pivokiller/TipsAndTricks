using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders;

public class DataSeeder : IDataSeeder
{
    private readonly BlogDbContext _dbContext;

    public DataSeeder(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Initialize()
    {
        _dbContext.Database.EnsureCreated();

        if (_dbContext.Posts.Any()) return;

        var authors = AddAuthors();
        var category = AddCategories();
        var tags = AddTags();
        var posts = AddPosts(authors, category, tags);
    }

    private IList<Author> AddAuthors() 
    {
        var authors = new List<Author>()
        {
            new()
            {
                FullName = "Jason Mouth",
                UrlSlug = "jason-mouth",
                Email = "json@gmail.com",
                JoinedDate = new DateTime(2022, 10, 21)
            },
            new()
            {
                FullName = "Jessica Wonder",
                UrlSlug = "jessica-wonder",
                Email = "jessica665@motip.com",
                JoinedDate = new DateTime(2020, 4, 19)
            },
            new()
            {
                FullName = "Thomas Prim",
                UrlSlug = "thomas-prim",
                Email = "thomas@gmail.com",
                JoinedDate = new DateTime(2022, 3, 8)
            },
            new()
            {
                FullName = "Michael Jackson",
                UrlSlug = "michael-jackson",
                Email = "michael@gmail.com",
                JoinedDate = new DateTime(2019, 5, 20)
            },
            new()
            {
                FullName = "Isaac Newton",
                UrlSlug = "isaac-newton",
                Email = "isaac@gmail.com",
                JoinedDate = new DateTime(2022, 11, 28)
            }
        };

        _dbContext.Authors.AddRange(authors);
        _dbContext.SaveChanges();

        return authors;
    }

    private IList<Category> AddCategories() 
    {
        var categories = new List<Category>()
        {
            new() { Name = ".NET Core", Description = ".NET Core", UrlSlug = "net-core" },
            new() { Name = "Architecture", Description = "Architecture", UrlSlug = "architecture" },
            new() { Name = "Messaging", Description = "Messaging", UrlSlug = "messaging" },
            new() { Name = "OPP", Description = "Object-Oriented Program", UrlSlug = "oop" },
            new() { Name = "Design Patterns", Description = "Design Patterns", UrlSlug = "design-patterns" },
            new() { Name = "Vue.js", Description = "Vue.js", UrlSlug = "vuejs" },
            new() { Name = "CLI", Description = "Command Line Interface", UrlSlug = "cli" },
            new() { Name = "React", Description = "React", UrlSlug = "react" },
            new() { Name = "NPM", Description = "Node Package Manager", UrlSlug = "npm" },
            new() { Name = "Node.js", Description = "Node.js", UrlSlug = "nodejs" }
        };

        _dbContext.Categories.AddRange(categories);
        _dbContext.SaveChanges();

        return categories;
    }

    private IList<Tag> AddTags() 
    {
        var tags = new List<Tag>()
        {
            new() { Name = "Google", Description = "Google applications", UrlSlug = "google" },
            new() { Name = "ASP.NET MVC", Description = "ASP.NET MVC", UrlSlug = "aspnet-mvc" },
            new() { Name = "Razor Page", Description = "Razor Page", UrlSlug = "razor-page" },
            new() { Name = "Blazor", Description = "Blazor", UrlSlug = "blazor" },
            new() { Name = "Deep Learning", Description = "Deep Learning", UrlSlug = "deep-learning" },
            new() { Name = "Neural Network", Description = "Neural Network", UrlSlug = "neural-network" }
        };

        _dbContext.Tags.AddRange(tags);
        _dbContext.SaveChanges();

        return tags;
    }

    private IList<Post> AddPosts(IList<Author> authors, IList<Category> categories, IList<Tag> tags) 
    {
        var posts = new List<Post>()
        {
            new()
            {
                Title = "ASP.NET Core Diagnostic Scenarios",
                ShortDescription = "David and friends has a great repos...",
                Description = "Here's a few great DON'T and DO examples...",
                Meta = "David and friends has a great repository filled...",
                UrlSlug = "aspnet-core-diagnnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[0],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            },
            new()
            {

                Title = "ASP.NET Core Diagnostic Scenarios",
                ShortDescription = "David and friend has great .....",
                Description = "Here's a few DON'T and DO example...",
                Meta = "Nothing to read...",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 2,
                Author = authors[1],
                Category = categories[1],
                Tags = new List<Tag>()
                {
                    tags[1]
                }
            },
            new()
            {
                Title = "ASP.NET Core Diagnostic Scenarios",
                ShortDescription = "David and friend has great .....",
                Description = "Here's a few DON'T and DO example...",
                Meta = "Nothing to read...",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 5,
                Author = authors[2],
                Category = categories[2],
                Tags = new List<Tag>()
                {
                    tags[2]
                }
            },
            new()
            {
                Title = "ASP.NET Core Diagnostic Scenarios",
                ShortDescription = "David and friend has great .....",
                Description = "Here's a few DON'T and DO example...",
                Meta = "Nothing to read...",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 15,
                Author = authors[3],
                Category = categories[3],
                Tags = new List<Tag>()
                {
                    tags[3]
                }
            },
            new()
            {
                Title = "ASP.NET Core Diagnostic Scenarios",
                ShortDescription = "David and friend has great .....",
                Description = "Here's a few DON'T and DO example...",
                Meta = "Nothing to read...",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 20,
                Author = authors[4],
                Category = categories[4],
                Tags = new List<Tag>()
                {
                    tags[4]
                }
            }
        };

        _dbContext.Posts.AddRange(posts);
        _dbContext.SaveChanges();

        return posts;
    }

}
