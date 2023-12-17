using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trailer.DataAccess.Data;
using Trailer.DataAccess.Repository.IRepository;

namespace Trailer.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _db;

        public ICategoryRepository Category { get; private set; }

        public IMovieRepository Movie { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Movie = new MovieRepository(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
