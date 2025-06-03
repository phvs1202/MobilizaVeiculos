using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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

        [HttpPost("AdicionarEntrada")] //Adicionar entrada
        public async Task<ActionResult<entrada>> AdicionarEntrada([FromBody] entrada entrada)
        {
            try
            {
                _dbContext.entrada.Add(entrada);
                await _dbContext.SaveChangesAsync();
                return Ok(entrada);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpPut("AlterarEntrada/{id}")] //Alterar entrada por id
        public async Task<ActionResult<entrada>> Atualizar(int id, [FromBody] entrada entrada)
        {
            try
            {
                var entradaAtual = await _dbContext.entrada.FindAsync(id);

                if (entrada == null)
                    return NotFound();

                entradaAtual.hora = entrada.hora;
                entradaAtual.usuarios_id = entrada.usuarios_id;
                entradaAtual.motivo_entrada = entrada.motivo_entrada;
                entradaAtual.tipo_veiculo_id = entrada.tipo_veiculo_id;

                _dbContext.Update(entradaAtual);
                await _dbContext.SaveChangesAsync();
                return Ok(entradaAtual);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpDelete("DeletarEntrada/{id}")] // Deletar entrada específica
        public async Task<ActionResult> Deletar(int id)
        {
            try
            {
                var entrada = await _dbContext.entrada.FindAsync(id);

                if (entrada == null)
                    return NotFound();

                _dbContext.entrada.Remove(entrada);
                await _dbContext.SaveChangesAsync();

                return Ok("Entrada removida com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }
    }
}
