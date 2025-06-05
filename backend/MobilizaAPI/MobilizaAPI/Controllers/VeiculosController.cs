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

        [HttpGet("VeiculoEspecifico/{id}")] //Trazer veiculo específico
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
                    veiculo.status_id = 1;
                    _dbContext.SaveChanges();
                }
                return Ok("Veículo cadastrado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao realizar login.", erro = ex.Message });
            }
        }

        [HttpPut("AlterarVeiculo/{id}")] //Alterar veiculo por id
        public async Task<ActionResult<veiculos>> Atualizar(int id, [FromBody] veiculos veiculos)
        {
            try
            {
                var veiculoAtual = await _dbContext.veiculos.FindAsync(id);

                if (veiculoAtual == null)
                    return NotFound();

                veiculoAtual.placa = veiculos.placa;
                veiculoAtual.tipo_veiculo_id = veiculos.tipo_veiculo_id;
                veiculoAtual.usuario_id = veiculos.usuario_id;

                _dbContext.Update(veiculoAtual);
                await _dbContext.SaveChangesAsync();
                return Ok(veiculoAtual);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpDelete("DeletarVeiculo/{id}")] // Deletar veiculo específico
        public async Task<ActionResult> Deletar(int id)
        {
            try
            {
                var veiculos = await _dbContext.veiculos.FindAsync(id);

                if (veiculos == null)
                    return NotFound();

                _dbContext.veiculos.Remove(veiculos);
                await _dbContext.SaveChangesAsync();

                return Ok("Veiculo removido com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("VeiculoPorUsuario/{id}")] //Trazer veiculo por usuario
        public async Task<ActionResult<IEnumerable<veiculos>>> GetVeiculos(int id)
        {
            try
            {
                var veiculos = await _dbContext.veiculos.Where(i => i.usuario_id == id).ToListAsync();
                return Ok(veiculos);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpPut("InativarVeiculos/{id}")] //status de ativo para inativo
        public async Task<ActionResult<veiculos>> Inativar(int id)
        {
            try
            {
                var veiculos = await _dbContext.veiculos.FindAsync(id);
                veiculos.status_id = 2;
                await _dbContext.SaveChangesAsync();
                return Ok("Veículo foi inativado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("VeiculosAtivos/{id}")] //Trazer veiculos ativos
        public async Task<ActionResult<IEnumerable<veiculos>>> GetAtivos(int id)
        {
            try
            {
                var veiculos = await _dbContext.veiculos.Where(i => i.status_id == 1).ToListAsync();
                var especifico = veiculos.Where(i => i.usuario_id == id).ToList();
                return Ok(especifico);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("VeiculosInativos/{id}")] //Trazer veiculos inativos
        public async Task<ActionResult<IEnumerable<veiculos>>> GetInativos(int id)
        {
            try
            {
                var veiculos = await _dbContext.veiculos.Where(i => i.status_id == 2).ToListAsync();
                var especifico = veiculos.Where(i => i.usuario_id == id).ToList();
                return Ok(especifico);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("qtdVeiculos")] //Quantidade de veiculos
        public async Task<ActionResult<IEnumerable<veiculos>>> quantidade()
        {
            try
            {
                var contagem = await _dbContext.veiculos
                    .GroupBy(i => i.tipo_veiculo_id)
                    .Select(i => new
                    {
                        Tipo = i.Key,
                        Quantidade = i.Count()
                    }).ToListAsync();

                var resultado = new
                {
                    Carro = contagem.FirstOrDefault(i => i.Tipo == 1)?.Quantidade ?? 0,
                    Moto = contagem.FirstOrDefault(i => i.Tipo == 2)?.Quantidade ?? 0,
                    Van = contagem.FirstOrDefault(i => i.Tipo == 3)?.Quantidade ?? 0,
                    Caminhao = contagem.FirstOrDefault(i => i.Tipo == 4)?.Quantidade ?? 0
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }
    }
}
