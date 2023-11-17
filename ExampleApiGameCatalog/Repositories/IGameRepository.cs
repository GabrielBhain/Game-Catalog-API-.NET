using ExampleApiGameCatalog.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleApiGameCatalog.Repositories
{
    public interface IGameRepository : IDisposable
    {
        Task<List<Game>> Get(int page, int quantity);
        Task<Game> Get(Guid id);
        Task<List<Game>> Get(string name, string producer);
        Task Insert(Game game);
        Task Update(Game game);
        Task Remove(Guid id);
    }
}
