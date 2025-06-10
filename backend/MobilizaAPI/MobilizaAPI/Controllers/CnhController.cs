using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobilizaAPI.Model;
using MobilizaAPI.Repository;
using QRCoder;
using System.Drawing.Imaging;

namespace MobilizaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CnhController : ControllerBase
    {
        private readonly DBMobilizaContext _dbContext;
        public CnhController(DBMobilizaContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("TodasCnh")] //Trazer todas as cnhs
        public async Task<ActionResult<IEnumerable<cnh>>> Get()
        {
            try
            {
                var cnh = await _dbContext.cnh.ToListAsync();
                return Ok(cnh);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("CnhEspecifica/{id}")] //Trazer cnh específica
        public async Task<ActionResult<IEnumerable<cnh>>> GetUser(int id)
        {
            try
            {
                var cnh = _dbContext.cnh.Where(i => i.id == id).FirstOrDefault();
                return Ok(cnh);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpPut("AlterarCnh/{id}")] //Alterar cnh por id
        public async Task<ActionResult<cnh>> Atualizar(int id, [FromBody] cnh cnh)
        {
            try
            {
                var cnhAtual = await _dbContext.cnh.FindAsync(id);

                if (cnhAtual == null)
                    return NotFound();

                cnhAtual.data_validade = cnh.data_validade;
                cnhAtual.numero_cnh = cnh.numero_cnh;
                cnhAtual.categoria = cnh.categoria;
                cnhAtual.usuario_id = cnh.usuario_id;
                cnhAtual.status_id = cnh.status_id;

                _dbContext.Update(cnhAtual);
                await _dbContext.SaveChangesAsync();
                return Ok(cnhAtual);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpDelete("DeletarCnh/{id}")] // Deletar cnh específica
        public async Task<ActionResult> Deletar(int id)
        {
            try
            {
                var cnh = await _dbContext.cnh.FindAsync(id);

                if (cnh == null)
                    return NotFound();

                _dbContext.cnh.Remove(cnh);
                await _dbContext.SaveChangesAsync();

                return Ok("Os dados da cnh foram removidas com sucesso!");
            }
            catch (Exception ex)
            {
                var detalhesErro = ex.InnerException != null ? $" - Detalhes: {ex.InnerException.Message}" : "";
                return BadRequest($"{ex.Message}{detalhesErro}");
            }
        }

        [HttpPut("InativarCnh/{id}")] //status de ativo para inativo
        public async Task<ActionResult<cnh>> Inativar(int id)
        {
            try
            {
                var cnh = await _dbContext.cnh.FindAsync(id);
                cnh.status_id = 2;
                await _dbContext.SaveChangesAsync();
                return Ok("Cnh foi inativada com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("CnhEspecifico/{numero}")] //Procurar user por cnh
        public async Task<ActionResult<IEnumerable<usuarios>>> GetPorCNH(int numero)
        {
            try
            {
                var cnh = await _dbContext.cnh.Where(i => EF.Functions.Like(i.numero_cnh.ToString(), $"{numero}%")).ToListAsync();
                return Ok(cnh);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }
    }
}
