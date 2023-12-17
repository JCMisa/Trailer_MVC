using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trailer.Models;

namespace Trailer.DataAccess.Repository.IRepository
{
    public interface IMovieRepository : IRepository<Movie>
    {
        void Update(Movie obj);
    }
}
