using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobilizaAPI.Model;
using MobilizaAPI.Repository;

namespace MobilizaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly DBMobilizaContext _dbContext;
        public CursoController(DBMobilizaContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("TodosCurso")] //Trazer todos os cursos
        public async Task<ActionResult<IEnumerable<curso>>> Get()
        {
            try
            {
                var curso = await _dbContext.curso.ToListAsync();
                return Ok(curso);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("CursoEspecifico/{id}")] //Trazer curso específico
        public async Task<ActionResult<IEnumerable<curso>>> GetCurso(int id)
        {
            try
            {
                var curso = _dbContext.curso.Where(i => i.id == id).FirstOrDefault();
                return Ok(curso);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} - Detalhes: {ex.InnerException?.Message}");
            }
        }
    }
}
