using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Core.DTO;

public class PostItem
{
    public string CategoryName { get; set; }

    public IList<Tag> Tags { get; set; }
}
