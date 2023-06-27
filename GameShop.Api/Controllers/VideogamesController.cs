using GameShop.Api.DataTransferObjects;
using GameShop.Core.Entities;
using GameShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameShop.Api.Controllers
{
    [ApiController]
    public class VideogamesController : ControllerBase
    {
        private readonly IRepository<Videogame> videogameRepository;
        private readonly IRepository<Category> categoryRepository;

        public VideogamesController(IRepository<Videogame> videogameRepository, IRepository<Category> categoryRepository) 
        {
            this.videogameRepository = videogameRepository;
            this.categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Registra un videojuego dentro de una categoría.
        /// </summary>
        /// <param name="categoryId">El id de la categoría en donde se agregará el videojuego.</param>
        /// <param name="videogame">El videojuego a registrar.</param>
        /// <returns>El videojuego agregado</returns>
        [HttpPost("categories/{categoryId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VideogameDetailDto> AddVideoGame([FromRoute]int categoryId, [FromBody]VideogameCreateDto videogame)
        {
            var category = this.categoryRepository.GetById(categoryId);
            if(category is null)
            {
                return BadRequest($"No se encontró una categoría con id {categoryId} para registrar el videojuego.");
            }

            if(string.IsNullOrEmpty(videogame.Name))
            {
                return BadRequest("No se puede registrar un videojuego sin nombre.");
            }

            var registeredVideogame = this.videogameRepository.Add(new Videogame
            {
                Name = videogame.Name,
                PublishingDate = videogame.PublishingDate,
                Author = videogame.Author,
                GameMode = videogame.GameMode,
                Quantity = videogame.Quantity,
                CategoryId = videogame.CategoryId,
            });

            return new CreatedAtActionResult("GetVideogameById", "Videogames",
                new
                {
                    categoryId = categoryId,
                    videogameId = registeredVideogame.Id
                },
                new VideogameDetailDto
                {
                    Id = registeredVideogame.Id,
                    Name = registeredVideogame.Name,
                    PublishingDate = registeredVideogame.PublishingDate,
                    Author = registeredVideogame.Author,
                    GameMode = registeredVideogame.GameMode,
                    Quantity = registeredVideogame.Quantity,
                    CategoryId = registeredVideogame.CategoryId
                }
            );
        }

        [HttpGet("categories/{categoryId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<VideogameCreateDto>> GetVideogames([FromRoute] int categoryId)
        {
            var category = this.categoryRepository.GetById(categoryId);
            if(category is null)
            {
                return BadRequest($"No se encontró una categoría con id {categoryId}.");
            }

            return Ok(this.videogameRepository.Filter(x => x.CategoryId == categoryId)
                .Select(x => new VideogameDetailDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PublishingDate = x.PublishingDate,
                    Author = x.Author,
                    GameMode = x.GameMode,
                    Quantity = x.Quantity,
                    CategoryId = categoryId
                }));
        }

        [HttpGet("categories/{categoryId}/[controller]/{videogameId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VideogameDetailDto> GetVideogameById([FromRoute]int categoryId, int videogameId)
        {
            var category = this.categoryRepository.GetById(categoryId);
            if(category is null)
            {
                return BadRequest($"No se encontró una categoría con el id {categoryId}.");
            }

            var videogame = this.videogameRepository.GetById(videogameId);
            if(videogame is null)
            {
                return BadRequest($"No se encontró un registro del videojuego con el id {videogameId}");
            }

            return Ok(new VideogameDetailDto
            {
                Id = videogameId,
                Name = videogame.Name,
                PublishingDate = videogame.PublishingDate,
                Author = videogame.Author,
                GameMode = videogame.GameMode,
                Quantity = videogame.Quantity,
                CategoryId = categoryId
            });
        }

        [HttpGet("categories/[controller]/Gamemode/{videogameGamemode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VideogameDetailDto> GetByGamemode(string gamemode)
        {
            var videogames = videogameRepository.Get();
            var videogame = videogames.Where(x => x.GameMode == gamemode);

            return Ok(videogame);

        }

        [HttpGet("categories/[controller]/{categoryName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VideogameDetailDto> GetByGenre(string genre)
        {
            var categories = categoryRepository.Get();
            var category = categories.SingleOrDefault(x => x.Genre == genre);

            var videogames = videogameRepository.Filter(x => x.CategoryId == category.Id );
            return Ok(videogames.Select(x => new VideogameDetailDto
            {
                Id = x.Id,
                Name = x.Name,
                PublishingDate = x.PublishingDate,
                Author = x.Author,
                GameMode = x.GameMode,
                Quantity = x.Quantity,
                CategoryId = category.Id
            }));

        }
    }
}
