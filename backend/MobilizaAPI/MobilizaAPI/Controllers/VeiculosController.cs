using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobilizaAPI.Model;
using MobilizaAPI.Repository;

namespace MobilizaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly DBMobilizaContext _dbContext;
        public VeiculosController(DBMobilizaContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("TodosVeiculos")] //Trazer todos os tipos
        public async Task<ActionResult<IEnumerable<veiculos>>> Get()
        {
            try
            {
                var veiculos = await _dbContext.veiculos.ToListAsync();
                return Ok(veiculos);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("VeiculoEspecifico/{id}")] //Trazer curso específico
        public async Task<ActionResult<IEnumerable<veiculos>>> GetTipo(int id)
        {
            try
            {
                var veiculos = _dbContext.veiculos.Where(i => i.id == id).FirstOrDefault();
                return Ok(veiculos);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpPost("CadastroVeiculo")] //Cadastrar seu veículo
        public IActionResult Veiculos([FromBody] List<veiculos> veiculos)
        {
            try
            {
                foreach (var veiculo in veiculos)
                {
                    _dbContext.veiculos.Add(veiculo);
                    _dbContext.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao realizar login.", erro = ex.Message });
            }
        }
    }
}
