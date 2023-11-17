using ExampleApiGameCatalog.Exceptions;
using ExampleApiGameCatalog.InputModel;
using ExampleApiGameCatalog.Services;
using ExampleApiGameCatalog.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApiGameCatalog.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Retrieve all games in a paginated manner
        /// </summary>
        /// <remarks>
        /// It is not possible to return games without pagination
        /// </remarks>
        /// <param name="page">Indicates which page is being queried. Minimum 1</param>
        /// <param name="quantity">Indicates the quantity of records per page. Minimum 1 and maximum 50</param>
        /// <response code="200">Returns the list of games</response>
        /// <response code="204">If there are no games</response>   
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameViewModel>>> Get([FromQuery, Range(1, int.MaxValue)] int page = 1, [FromQuery, Range(1, 50)] int quantity = 5)
        {
            var games = await _gameService.Get(page, quantity);

            if (games.Count() == 0)
                return NoContent();

            return Ok(games);
        }

        /// <summary>
        /// Retrieve a game by its Id
        /// </summary>
        /// <param name="gameId">Id of the desired game</param>
        /// <response code="200">Returns the filtered game</response>
        /// <response code="204">If there is no game with this id</response>   
        [HttpGet("{gameId:guid}")]
        public async Task<ActionResult<GameViewModel>> Get([FromRoute] Guid gameId)
        {
            var game = await _gameService.Get(gameId);

            if (game == null)
                return NoContent();

            return Ok(game);
        }

        /// <summary>
        /// Insert a game into the catalog
        /// </summary>
        /// <param name="gameInputModel">Data of the game to be inserted</param>
        /// <response code="200">If the game is inserted successfully</response>
        /// <response code="422">If a game with the same name for the same producer already exists</response>   
        [HttpPost]
        public async Task<ActionResult<GameViewModel>> InsertGame([FromBody] GameInputModel gameInputModel)
        {
            try
            {
                var game = await _gameService.Insert(gameInputModel);

                return Ok(game);
            }
            catch (GameAlreadyRegisteredException ex)
            {
                return UnprocessableEntity("A game with this name for this producer already exists");
            }
        }

        /// <summary>
        /// Update a game in the catalog
        /// </summary>
        /// <param name="gameId">Id of the game to be updated</param>
        /// <param name="gameInputModel">New data to update the indicated game</param>
        /// <response code="200">If the game is updated successfully</response>
        /// <response code="404">If there is no game with this Id</response>   
        [HttpPut("{gameId:guid}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid gameId, [FromBody] GameInputModel gameInputModel)
        {
            try
            {
                await _gameService.Update(gameId, gameInputModel);

                return Ok();
            }
            catch (GameNotFoundException ex)
            {
                return NotFound("This game does not exist");
            }
        }

        /// <summary>
        /// Update the price of a game
        /// </summary>
        /// <param name="gameId">Id of the game to be updated</param>
        /// <param name="price">New price of the game</param>
        /// <response code="200">If the price is updated successfully</response>
        /// <response code="404">If there is no game with this Id</response>   
        [HttpPatch("{gameId:guid}/price/{price:double}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid gameId, [FromRoute] double price)
        {
            try
            {
                await _gameService.Update(gameId, price);

                return Ok();
            }
            catch (GameNotFoundException ex)
            {
                return NotFound("This game does not exist");
            }
        }

        /// <summary>
        /// Delete a game
        /// </summary>
        /// <param name="gameId">Id of the game to be deleted</param>
        /// <response code="200">If the game is deleted successfully</response>
        /// <response code="404">If there is no game with this Id</response>
        [HttpDelete("{gameId:guid}")]
        public async Task<ActionResult> DeleteGame([FromRoute] Guid gameId)
        {
            try
            {
                await _gameService.Remove(gameId);

                return Ok();
            }
            catch (GameNotFoundException ex)
            {
                return NotFound("This game does not exist");
            }
        }
    }
}
