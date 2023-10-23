using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHero.DTO;
using SuperHero.Models;

namespace SuperHero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public SuperHeroController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarHerois()
        {
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHeroModel>> BuscarHeroiPorId(int id)
        {
            var hero = await _dataContext.SuperHeroes.FirstOrDefaultAsync(h => h.Id == id);
            if (hero == null)
                return BadRequest("Hero Not Found");
            return Ok(hero);
        }

        [HttpGet("cidade/{cidade}")]
        public async Task<ActionResult<List<SuperHeroModel>>> BuscarHeroisPorCidade(string cidade)
        {
            var heroes = await _dataContext.SuperHeroes.Where(h => h.Place == cidade).ToListAsync();
            if (heroes == null)
                return BadRequest("Hero Not Found");
            return Ok(heroes);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarHeroi(SuperHeroModel hero)
        {
            Console.WriteLine(hero.Id);
            _dataContext.SuperHeroes.Add(hero);
            await _dataContext.SaveChangesAsync();

            return Ok("Heroi adicionado com sucesso!");
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHeroModel>>> AtualizarHeroi(SuperHeroDTO request)
        {
            var dbHero = await _dataContext.SuperHeroes.FirstOrDefaultAsync(h => h.Id == request.Id);
            if (dbHero == null)
                return BadRequest("Hero not found");

            dbHero.FirstName = request.FirstName;
            dbHero.LastName = request.LastName;
            dbHero.Place = request.Place;

            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarHeroi(int id)
        {
            var dbHero = await _dataContext.SuperHeroes.FindAsync(id);
            if (dbHero == null)
                return BadRequest("Hero not found");
            _dataContext.SuperHeroes.Remove(dbHero);
            await _dataContext.SaveChangesAsync();

            return Ok("Heroi Deletado Com Sucesso!");
        }
    }
}
