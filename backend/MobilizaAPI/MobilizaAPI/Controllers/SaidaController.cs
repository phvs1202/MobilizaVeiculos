using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobilizaAPI.Model;
using MobilizaAPI.Repository;

namespace MobilizaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaidaController : ControllerBase
    {
        private readonly DBMobilizaContext _dbContext;
        public SaidaController(DBMobilizaContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("TodasSaidas")] //Trazer todos as saidas
        public async Task<ActionResult<IEnumerable<saida>>> Get()
        {
            try
            {
                var saidas = await _dbContext.saida.ToListAsync();
                return Ok(saidas);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("SaidaEspecifica/{id}")] //Trazer saida específica
        public async Task<ActionResult<IEnumerable<saida>>> GetSaida(int id)
        {
            try
            {
                var saida = _dbContext.saida.Where(i => i.id == id).FirstOrDefault();
                return Ok(saida);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }
    }
}
