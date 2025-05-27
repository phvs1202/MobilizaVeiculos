using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobilizaAPI.Model;
using MobilizaAPI.Repository;

namespace MobilizaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoVeiculoController : ControllerBase
    {
        private readonly DBMobilizaContext _dbContext;
        public TipoVeiculoController(DBMobilizaContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("TodosTipos")] //Trazer todos os tipos
        public async Task<ActionResult<IEnumerable<tipo_veiculo>>> Get()
        {
            try
            {
                var tipo = await _dbContext.tipo_veiculo.ToListAsync();
                return Ok(tipo);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("TipoEspecifico/{id}")] //Trazer tipo específico
        public async Task<ActionResult<IEnumerable<tipo_veiculo>>> GetTipo(int id)
        {
            try
            {
                var tipo = _dbContext.tipo_veiculo.Where(i => i.id == id).FirstOrDefault();
                return Ok(tipo);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }
    }
}
