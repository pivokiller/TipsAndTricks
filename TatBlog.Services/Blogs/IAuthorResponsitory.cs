using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Constracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IAuthorResponsitory
    {

        Task<Author> FindAuthorBySlugAsync(string slug, CancellationToken cancellationToken);

        Task<Author> FindAuthorByIdAsync(int id, CancellationToken cancellationToken);

        Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
           IPagingParams pagingParams, CancellationToken cancellationToken = default
           );

        Task<Author> UpdateAuthorAsync(Author author, CancellationToken cancellationToken = default);

        Task<IPagedList<Author>> GetAuthorTopPostAsync(int n, IPagingParams pagingParams, CancellationToken cancellationToken = default);

        Task<bool> IsAuthorExistBySlugAsync(int id, string slug, CancellationToken cancellationToken);
    }
}
