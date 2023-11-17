using ExampleApiGameCatalog.Entities;
using ExampleApiGameCatalog.Exceptions;
using ExampleApiGameCatalog.InputModel;
using ExampleApiGameCatalog.Repositories;
using ExampleApiGameCatalog.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApiGameCatalog.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<List<GameViewModel>> Get(int page, int quantity)
        {
            var games = await _gameRepository.Get(page, quantity);

            return games.Select(game => new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            })
            .ToList();
        }

        public async Task<GameViewModel> Get(Guid id)
        {
            var game = await _gameRepository.Get(id);

            if (game == null)
                return null;

            return new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public async Task<GameViewModel> Insert(GameInputModel game)
        {
            var existingGame = await _gameRepository.Get(game.Name, game.Producer);

            if (existingGame.Count > 0)
                throw new GameAlreadyRegisteredException();

            var gameInsert = new Game
            {
                Id = Guid.NewGuid(),
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };

            await _gameRepository.Insert(gameInsert);

            return new GameViewModel
            {
                Id = gameInsert.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public async Task Update(Guid id, GameInputModel game)
        {
            var existingGame = await _gameRepository.Get(id);

            if (existingGame == null)
                throw new GameNotRegisteredException();

            existingGame.Name = game.Name;
            existingGame.Producer = game.Producer;
            existingGame.Price = game.Price;

            await _gameRepository.Update(existingGame);
        }

        public async Task Update(Guid id, double price)
        {
            var existingGame = await _gameRepository.Get(id);

            if (existingGame == null)
                throw new GameNotRegisteredException();

            existingGame.Price = price;

            await _gameRepository.Update(existingGame);
        }

        public async Task Remove(Guid id)
        {
            var game = await _gameRepository.Get(id);

            if (game == null)
                throw new GameNotRegisteredException();

            await _gameRepository.Remove(id);
        }

        public void Dispose()
        {
            _gameRepository?.Dispose();
        }
    }
}
