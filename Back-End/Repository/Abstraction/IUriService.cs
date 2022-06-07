using AnL.Filter;
using System;

namespace AnL.Repository.Abstraction
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
