using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Constracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
{
    // Tìm bài viết có tên định danh là 'slug'
    // và được đăng vào tháng 'month' năm 'year'
    Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default);

    // Tìm Top N bài viết phổ được nhiều người xem nhất
    Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default);

    // Kiểm tra xem tên định danh của bài viết đã có hay chưa
    Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default);

    // Tăng số lượt xem của một bài viết
    Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default);

    // Lấy danh sách chuyên mục và số lượng bài viết
    // nằm thuộc từng chuyên mục/chủ đề
    Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default);

    // Lấy danh sách từ khóa/thẻ và phân trang theo
    // các tham số pagingParams
    Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);

    Task<Tag> GetTagSlusAsync(string slug, CancellationToken cancellation = default);

    Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default);

    Task<IList<CategoryItem>> GetCategorysAsync(CancellationToken cancellationToken = default);

    Task<bool> DeleleTagWithSlugAsync(string slug, CancellationToken cancellation = default);

    Task<Tag> FindTagWithIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Category> FindCategoryWithIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> DeleteCategoryWithSlugAsync(string slug, CancellationToken cancellationToken);

    Task<bool> IsCategoryExistSlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<Post> FindPostByPostQueryAsync(PostQuery postQuery, CancellationToken cancellationToken = default);

    Task<IPagedList<Post>> GetPagedPostsAsync(PostQuery condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    Task<Post> CreateOrUpdatePostAsync(Post post, IEnumerable<string> tags,CancellationToken cancellationToken = default);

    // Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default);
}
