﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly dbsponteContext db;

        public CursosController(dbsponteContext context)
        {
            db = context;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cursos>>> GetCursos()
        {
            try
            {
                return await db.Cursos.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                Dispose(true);
            }
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cursos>> GetCursos(int id)
        {
            try
            {
                var cursos = await db.Cursos.FindAsync(id);

                if (cursos == null)
                {
                    return NotFound("Curso não identificado ou deletado!");
                }

                return cursos;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                Dispose(true);
            }
        }

        // PUT: api/Cursos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCursos(int id, Cursos cursos)
        {
            if (id != cursos.CursoId)
            {
                return BadRequest("Curso não encontrado para atualização!");
            }
            else
            {

                var oCurso = await (from r in db.Cursos where r.Nome == cursos.Nome && r.DisciplinasAssociadas == cursos.DisciplinasAssociadas && r.CursoId != cursos.CursoId select new { r.CursoId }).FirstOrDefaultAsync();
                if (oCurso == null)
                {
                    db.Entry(cursos).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CursosExists(id))
                        {
                            return NotFound("Curso não encontrado!");
                        }
                        else
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        Dispose(true);
                    }

                    return NoContent();
                }
                else return BadRequest("Não será possível atualizar, os dados informados são iguais a outro curso já cadastrado!");
            }

        }

        // POST: api/Cursos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Cursos>> PostCursos(Cursos cursos)
        {
            try
            {
                var oCurso = await (from r in db.Cursos where r.Nome == cursos.Nome && r.DisciplinasAssociadas == cursos.DisciplinasAssociadas select new { r.CursoId }).FirstOrDefaultAsync();
                if (oCurso == null)
                {
                    db.Cursos.Add(cursos);
                    await db.SaveChangesAsync();

                    return CreatedAtAction("GetCursos", new { id = cursos.CursoId }, cursos);
                }
                else return BadRequest("Já existe um curso com as mesmas caracteristicas cadastrado!");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                Dispose(true);
            }
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cursos>> DeleteCursos(int id)
        {
            try
            {
                var cursos = await db.Cursos.FindAsync(id);
                if (cursos == null)
                {
                    return NotFound();
                }

                db.Cursos.Remove(cursos);
                await db.SaveChangesAsync();

                return cursos;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                Dispose(true);
            }
        }

        private bool CursosExists(int id)
        {
            return db.Cursos.Any(e => e.CursoId == id);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}
