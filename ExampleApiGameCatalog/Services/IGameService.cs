using ExampleApiGameCatalog.InputModel;
using ExampleApiGameCatalog.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleApiGameCatalog.Services
{
    public interface IGameService : IDisposable
    {
        Task<List<GameViewModel>> Get(int page, int quantity);
        Task<GameViewModel> Get(Guid id);
        Task<GameViewModel> Insert(GameInputModel game);
        Task Update(Guid id, GameInputModel game);
        Task Update(Guid id, double price);
        Task Remove(Guid id);
    }
}
