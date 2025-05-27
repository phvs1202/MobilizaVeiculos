using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobilizaAPI.Model;
using MobilizaAPI.Repository;
using Org.BouncyCastle.Crypto;
using MobilizaAPI.Helper;

namespace MobilizaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DBMobilizaContext _dbContext;
        public UsuariosController(DBMobilizaContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("TodosUser")] //Trazer todos os usuários
        public async Task<ActionResult<IEnumerable<usuarios>>> Get()
        {
            try
            {
                var usuarios = await _dbContext.usuarios.ToListAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("UserEspecifico/{id}")] //Trazer usuário específico
        public async Task<ActionResult<IEnumerable<usuarios>>> GetUser(int id)
        {
            try
            {
                var usuarios = _dbContext.usuarios.Where(i => i.id == id).FirstOrDefault();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpPost("LoginUser")] //Login do usuário
        public IActionResult Login([FromBody] LoginRequest login)
        {
            try
            {
                var usuario = _dbContext.usuarios.FirstOrDefault(u => u.email == login.Email);

                // Verifica se o usuario existe e se a senha está correta
                if (usuario == null || !PasswordHasher.VerifyPassword(login.Senha, usuario.senha))
                    return Unauthorized(new { message = "Email ou senha incorretos!" });

                return Ok(new
                {
                    message = "Login bem-sucedido!",
                    cliente = new
                    {
                        id = usuario.id,
                        nome = usuario.nome,
                        email = usuario.email,
                        tipo_usuario = usuario.tipo_usuario_id,
                        curso_id = usuario.curso_id
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao realizar login.", erro = ex.Message });
            }
        }

        [HttpPost("CadastroUser")] //Cadastrar usuário
        public async Task<ActionResult<usuarios>> CriarUser([FromBody] usuarios User)
        {
            try
            {
                var a = await _dbContext.usuarios.Where(i => i.email == User.email).FirstOrDefaultAsync();
                if (a != null)
                    return BadRequest("Email já existente, crie outro.");

                User.senha = PasswordHasher.HashPassword(User.senha);

                _dbContext.usuarios.Add(User);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(User);
        }
    }
}
