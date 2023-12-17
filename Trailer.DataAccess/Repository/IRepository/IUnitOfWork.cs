using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trailer.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; } //for Category Model

        IMovieRepository Movie { get; } //for Movie Model

        Task SaveAsync();
    }
}
