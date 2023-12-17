using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trailer.DataAccess.Data;
using Trailer.DataAccess.Repository.IRepository;
using Trailer.Models;

namespace Trailer.DataAccess.Repository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public ApplicationDbContext _db;
        public MovieRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Movie obj)
        {
            var objFromDb = _db.Movies.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.Director = obj.Director;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.Price100 = obj.Price100;

                if(obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
