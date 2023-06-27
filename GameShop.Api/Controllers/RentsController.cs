using GameShop.Api.DataTransferObjects;
using GameShop.Core.Entities;
using GameShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameShop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentsController : ControllerBase
    {
        private readonly IRepository<Rent> rentRepository;
        private readonly IRepository<Videogame> videogameRepository;

        public RentsController(IRepository<Rent> rentRepository, IRepository<Videogame> videogameRepository)
        {
            this.rentRepository = rentRepository;
            this.videogameRepository = videogameRepository;
        }

        [HttpPost("videogames/{videogameId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<RentDetailDto> AddRent([FromRoute] int videogameId, [FromBody] RentCreateDto rent)
        {
            var videogame = this.videogameRepository.GetById(videogameId);
            if(videogame is null)
            {
                return BadRequest($"No se encontró el videojuego con id {videogameId} a rentar.");
            }

            if(string.IsNullOrEmpty(rent.Name))
            {
                return BadRequest("No se puede rentar un videojuego sin nombre del cliente.");
            }

            if(videogame.Quantity <= 3)
            {
                return NotFound("No se puede rentar el videojuego por política de escasez en inventario");
            }

            var newRent = this.rentRepository.Add(new Rent
            {
                DateRented = rent.DateRented,
                Name = rent.Name,
                VideogameId = rent.VideogameId,
            });

            videogame.Quantity--;
            this.videogameRepository.Update(videogame);

            return new CreatedAtActionResult("GetRentById", "Rents",
                new
                {
                    videogameId = videogameId,
                    rentId = newRent.Id
                },
                new RentDetailDto
                {
                    Id = newRent.Id,
                    DateRented = newRent.DateRented,
                    Name = newRent.Name,
                    VideogameId = newRent.VideogameId
                }
            );
        }

        [HttpGet("videogames/{videogameId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<VideogameCreateDto>> GetRents([FromRoute] int videogameId)
        {
            var videogame = this.videogameRepository.GetById(videogameId);
            if (videogame is null)
            {
                return BadRequest($"No se encontró un videojuego con id {videogameId}.");
            }

            return Ok(this.rentRepository.Filter(x => x.VideogameId == videogameId)
                .Select(x => new RentDetailDto
                {
                    DateRented = x.DateRented,
                    Name = x.Name,
                    VideogameId = x.VideogameId
                }));
        }

        [HttpPut("{id}")]
        public ActionResult<VideogameDetailDto> ReturnVideogame(int id, int videogameId)
        {
            var videogame = this.videogameRepository.GetById(videogameId);

            videogame.Quantity++;
            var updatedVideogame = videogameRepository.Update(videogame);

            var rent = this.rentRepository.GetById(id);
            this.rentRepository.Delete(rent);

            return Ok(updatedVideogame);
        }

    }
}
