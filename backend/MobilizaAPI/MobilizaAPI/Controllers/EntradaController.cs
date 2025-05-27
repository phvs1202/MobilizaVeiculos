using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobilizaAPI.Model;
using MobilizaAPI.Repository;

namespace MobilizaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradaController : ControllerBase
    {
        private readonly DBMobilizaContext _dbContext;
        public EntradaController(DBMobilizaContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("TodasEntradas")] //Trazer todos os entrada
        public async Task<ActionResult<IEnumerable<entrada>>> Get()
        {
            try
            {
                var entradas = await _dbContext.entrada.ToListAsync();
                return Ok(entradas);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("EntradaEspecifica/{id}")] //Trazer entrada especifica
        public async Task<ActionResult<IEnumerable<entrada>>> GetEntrada(int id)
        {
            try
            {
                var entrada = _dbContext.entrada.Where(i => i.id == id).FirstOrDefault();
                return Ok(entrada);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }
    }
}
