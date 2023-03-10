using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Constracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _context;

    public BlogRepository(BlogDbContext context)
    {
        _context = context;
    }

    // Tìm bài viết có tên định danh là 'slug'
    // và được đăng vào tháng 'month' năm 'year'
    public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
    {
        IQueryable<Post> postsQuery = _context.Set<Post>()
            .Include(x => x.Category)
            .Include(x => x.Author);
        
        if (year > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
        }

        if (month > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
        }

        if (!string.IsNullOrWhiteSpace(slug))
        {
            postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
        }

        return await postsQuery.FirstOrDefaultAsync(cancellationToken);

        /* throw new NotImplementedException(); */
    }

    // Tìm Top N bài viết phổ được nhiều người xem nhất
    public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Post>()
            .Include(x => x.Author)
            .Include(x => x.Category)
            .OrderByDescending(p => p.ViewCount)
            .Take(numPosts)
            .ToListAsync(cancellationToken);

        /* throw new NotImplementedException(); */

    }

    // Kiểm tra xem tên định danh của bài viết đã có hay chưa
    public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Post>()
            .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);

        /* throw new NotImplementedException(); */
    }

    // Tăng số lượng xem của một bài viết
    public async Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default)
    {
        await _context.Set<Post>()
            .Where(x => x.Id == postId)
            .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);

        /* throw new NotImplementedException(); */
    }

    // Lấy danh sách chuyên mục và số lượng bài viết
    // nằm thuộc từng chuyên mục/chủ đề
    public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Category> categories = _context.Set<Category>();

        if (showOnMenu)
        {
            categories = categories.Where(x => x.ShowOnMenu);
        }

        return await categories
            .OrderBy(x => x.Name)
            .Select(x => new CategoryItem()
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                ShowOnMenu = x.ShowOnMenu,
                PostCount = x.Posts.Count(p => p.Published)
            })
            .ToListAsync(cancellationToken);
    }

    // Lấy danh sách từ khóa/thẻ và phân trang theo
    // các tham số pagingParams
    public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        var tagQuery = _context.Set<Tag>()
            .Select(x => new TagItem()
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                PostCount = x.Posts.Count(p => p.Published)
            });

        return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
    }

    // Tìm chuyên mục theo Slug
    public async Task<Tag> GetTagSlusAsync(string slug, CancellationToken cancellation = default)
    {
        IQueryable<Tag> queryTag = _context.Set<Tag>();
        queryTag.Where(x => x.UrlSlug == slug);
        return await queryTag.FirstOrDefaultAsync(cancellation);
    }

    // Lấy hết toàn bộ Tag
    public async Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Tag> tags = _context.Set<Tag>();
        return await tags.Select(x => new TagItem()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            UrlSlug = x.UrlSlug,
            PostCount = x.Posts.Count(x => x.Published)
        }).ToListAsync(cancellationToken);
    }

    // Xóa Tag với tên định danh Slug
    public async Task<bool> DeleleTagWithSlugAsync(string slug, CancellationToken cancellation = default)
    {
        var tagDelete = await _context.Set<Tag>()
            .Where(t => t.UrlSlug == slug)
            .FirstOrDefaultAsync(cancellation);
        if (tagDelete == null)
        {
            return false;
        }
        else
        {
            _context.Set<Tag>().Remove(tagDelete);
            await _context.SaveChangesAsync(cancellation);
            return true;
        }
    }

    // Lấy toàn bộ Category
    public async Task<IList<CategoryItem>> GetCategorysAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Category> tags = _context.Set<Category>();
        return await tags.Select(x => new CategoryItem()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            UrlSlug = x.UrlSlug,
            PostCount = x.Posts.Count(x => x.Published)
        }).ToListAsync(cancellationToken);
    }

    // Tìm thẻ Tag bằng ID
    public async Task<Tag> FindTagWithIdAsync(int id, CancellationToken cancellationToken = default)
    {
        IQueryable<Tag> tagQuery = _context.Set<Tag>().Where(x => x.Id == id);
        return await tagQuery.FirstOrDefaultAsync(cancellationToken);
    }

    // Tìm Category bằng ID
    public async Task<Category> FindCategoryWithIdAsync(int id, CancellationToken cancellationToken = default)
    {
        IQueryable<Category> categoryQuery = _context.Set<Category>()
            .Where(c => c.Id == id);
        return await categoryQuery.FirstOrDefaultAsync(cancellationToken);
    }

    // Xóa Category theo Slug
    public async Task<bool> DeleteCategoryWithSlugAsync(string slug, CancellationToken cancellationToken)
    {
        var categoryDelete = await _context.Set<Category>()
            .Where(c => c.UrlSlug == slug).FirstOrDefaultAsync(cancellationToken);
        if (categoryDelete == null)
        {
            return false;
        }
        else
        {
            _context.Set<Category>().Remove(categoryDelete);
            return true;
        }
    }

    // Kiểm tra Category đã tồn tại với định danh Slug hay chưa
    public async Task<bool> IsCategoryExistSlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var categoryExist = await _context.Set<Category>()
            .Where(c => c.UrlSlug == slug).FirstOrDefaultAsync(cancellationToken);
        if (categoryExist == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public Task<Post> FindPostByPostQueryAsync(PostQuery postQuery, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IPagedList<Post>> GetPagedPostsAsync(PostQuery condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await FilterPosts(condition).ToPagedListAsync(pageNumber, pageSize, nameof(Post.PostedDate), "DESC", cancellationToken);
    }

    // Bổ sung
    public async Task<Tag> GetTagAsync(
        string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Tag>()
            .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
    }

    public async Task<bool> DeleteTagAsync(
        int tagId, CancellationToken cancellationToken = default)
    {
        //var tag = await _context.Set<Tag>().FindAsync(tagId);

        //if (tag == null) return false;

        //_context.Set<Tag>().Remove(tag);
        //return await _context.SaveChangesAsync(cancellationToken) > 0;

        return await _context.Set<Tag>()
            .Where(x => x.Id == tagId)
            .ExecuteDeleteAsync(cancellationToken) > 0;
    }

    public async Task<bool> CreateOrUpdateTagAsync(
        Tag tag, CancellationToken cancellationToken = default)
    {
        if (tag.Id > 0)
        {
            _context.Set<Tag>().Update(tag);
        }
        else
        {
            _context.Set<Tag>().Add(tag);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }


    public async Task<Post> GetPostAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        var postQuery = new PostQuery()
        {
            PublishedOnly = false,
            TitleSlug = slug
        };

        return await FilterPosts(postQuery).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Post> GetPostByIdAsync(
        int postId, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        if (!includeDetails)
        {
            return await _context.Set<Post>().FindAsync(postId);
        }

        return await _context.Set<Post>()
            .Include(x => x.Category)
            .Include(x => x.Author)
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
    }

    public async Task<bool> TogglePublishedFlagAsync(
        int postId, CancellationToken cancellationToken = default)
    {
        var post = await _context.Set<Post>().FindAsync(postId);

        if (post is null) return false;

        post.Published = !post.Published;
        await _context.SaveChangesAsync(cancellationToken);

        return post.Published;
    }

    public async Task<IList<Post>> GetRandomArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Post>()
            .OrderBy(x => Guid.NewGuid())
            .Take(numPosts)
            .ToListAsync(cancellationToken);
    }

    public async Task<IPagedList<T>> GetPagedPostsAsync<T>(
        PostQuery condition,
        IPagingParams pagingParams,
        Func<IQueryable<Post>, IQueryable<T>> mapper)
    {
        var posts = FilterPosts(condition);
        var projectedPosts = mapper(posts);

        return await projectedPosts.ToPagedListAsync(pagingParams);
    }

    public async Task<Post> CreateOrUpdatePostAsync(
        Post post, IEnumerable<string> tags,
        CancellationToken cancellationToken = default)
    {
        if (post.Id > 0)
        {
            await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
        }
        else
        {
            post.Tags = new List<Tag>();
        }

        /* var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => new
            {
                Name = x,
                Slug = x.GenerateSlug()
            })
            .GroupBy(x => x.Slug)
            .ToDictionary(g => g.Key, g => g.First().Name);


        foreach (var kv in validTags)
        {
            if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

            var tag = await GetTagAsync(kv.Key, cancellationToken) ?? new Tag()
            {
                Name = kv.Value,
                Description = kv.Value,
                UrlSlug = kv.Key
            };

            post.Tags.Add(tag);
        }

        post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

        if (post.Id > 0)
            _context.Update(post);
        else
            _context.Add(post);

        await _context.SaveChangesAsync(cancellationToken); */

        return post;
    }

    /* public async Task<bool> IsPostSlugExistedAsync(
        int postId, string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Post>()
            .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
    } */

    private IQueryable<Post> FilterPosts(PostQuery condition)
    {
        IQueryable<Post> posts = _context.Set<Post>()
            .Include(x => x.Category)
            .Include(x => x.Author)
            .Include(x => x.Tags);

        if (condition.PublishedOnly)
        {
            posts = posts.Where(x => x.Published);
        }

        if (condition.NotPublished)
        {
            posts = posts.Where(x => !x.Published);
        }

        if (condition.CategoryId > 0)
        {
            posts = posts.Where(x => x.CategoryId == condition.CategoryId);
        }

        if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
        {
            posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
        }

        if (condition.AuthorId > 0)
        {
            posts = posts.Where(x => x.AuthorId == condition.AuthorId);
        }

        if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
        {
            posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
        }

        if (!string.IsNullOrWhiteSpace(condition.TagSlug))
        {
            posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
        }

        if (!string.IsNullOrWhiteSpace(condition.Keyword))
        {
            posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
                                     x.ShortDescription.Contains(condition.Keyword) ||
                                     x.Description.Contains(condition.Keyword) ||
                                     x.Category.Name.Contains(condition.Keyword) ||
                                     x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
        }

        //if (condition.Year > 0)
        //{
        //    posts = posts.Where(x => x.PostedDate.Year == condition.Year);
        //}

        //if (condition.Month > 0)
        //{
        //    posts = posts.Where(x => x.PostedDate.Month == condition.Month);
        //}

        if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
        {
            posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
        }

        return posts;

        //// Compact version
        //return _context.Set<Post>()
        //	.Include(x => x.Category)
        //	.Include(x => x.Author)
        //	.Include(x => x.Tags)
        //	.WhereIf(condition.PublishedOnly, x => x.Published)
        //	.WhereIf(condition.NotPublished, x => !x.Published)
        //	.WhereIf(condition.CategoryId > 0, x => x.CategoryId == condition.CategoryId)
        //	.WhereIf(!string.IsNullOrWhiteSpace(condition.CategorySlug), x => x.Category.UrlSlug == condition.CategorySlug)
        //	.WhereIf(condition.AuthorId > 0, x => x.AuthorId == condition.AuthorId)
        //	.WhereIf(!string.IsNullOrWhiteSpace(condition.AuthorSlug), x => x.Author.UrlSlug == condition.AuthorSlug)
        //	.WhereIf(!string.IsNullOrWhiteSpace(condition.TagSlug), x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug))
        //	.WhereIf(!string.IsNullOrWhiteSpace(condition.Keyword), x => x.Title.Contains(condition.Keyword) ||
        //	                                                             x.ShortDescription.Contains(condition.Keyword) ||
        //	                                                             x.Description.Contains(condition.Keyword) ||
        //	                                                             x.Category.Name.Contains(condition.Keyword) ||
        //	                                                             x.Tags.Any(t => t.Name.Contains(condition.Keyword)))
        //	.WhereIf(condition.Year > 0, x => x.PostedDate.Year == condition.Year)
        //	.WhereIf(condition.Month > 0, x => x.PostedDate.Month == condition.Month)
        //	.WhereIf(!string.IsNullOrWhiteSpace(condition.TitleSlug), x => x.UrlSlug == condition.TitleSlug);
    }
}
