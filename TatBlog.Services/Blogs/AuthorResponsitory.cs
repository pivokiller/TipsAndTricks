using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using TatBlog.Core.Constracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class AuthorResponsitory : IAuthorResponsitory
    {
        private BlogDbContext _context;

        public AuthorResponsitory(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<Author> FindAuthorBySlugAsync(string slug, CancellationToken cancellationToken)
        {
            IQueryable<Author> authorSlug = _context.Set<Author>()
                .Where(a => a.UrlSlug == slug);
            return await authorSlug.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Author> FindAuthorByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<Author>()
                .Where(a => a.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public Task<Author> UpdateAuthorAsync(Author author, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsAuthorExistBySlugAsync(int id, string slug, CancellationToken cancellationToken)
        {
            return await _context.Set<Author>().AnyAsync(a => a.Id != id && a.UrlSlug == slug, cancellationToken);
        }

        public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .Select(a => new AuthorItem()
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    ImageUrl = a.ImageUrl,
                    Email = a.Email,
                    UrlSlug = a.UrlSlug,
                    Notes = a.Notes,

                }).ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IPagedList<Author>> GetAuthorTopPostAsync(int n, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            Author authorsTop = _context.Set<Author>()
           .Include(a => a.Posts)
           .OrderByDescending(a => a.Posts.Count(p => p.Published)).First();
            return await _context.Set<Author>()
                .Include(a => a.Posts)
                .Where(a => a.Posts.Count(p => p.Published) == authorsTop.Posts.Count(p => p.Published))
                .Take(n).ToPagedListAsync(pagingParams, cancellationToken);
        }
    }
}
