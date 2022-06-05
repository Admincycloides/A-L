using AnL.Models;
using AnL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnL.Repository.Implementation
{
    public class ProjectRepository : Repository<ProjectDetails>, IProject
    {
        private DbContext _context;
        private readonly IUnitOfWork _UOW;

        DbSet<ProjectDetails> dbSet;
        public ProjectRepository(DbContext context, IUnitOfWork UOW) : base(context)
        {
            this._context = context;
            this._UOW = UOW;
            dbSet = context.Set<ProjectDetails>();
        }

    }
}
