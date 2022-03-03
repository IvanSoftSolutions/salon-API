using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using salon_web_api.Models;
using salon_web_api.ViewModels;
using salon_web_api.Utilerias;
using Newtonsoft.Json;


namespace salon_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }


        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuarios>> GetUsuarios(int id)
        {
            var usuarios = await _context.Usuarios.FindAsync(id);

            if (usuarios == null)
            {
                return NotFound();
            }

            return usuarios;
        }

        //buscamos usuario por nombre de usuario
        // GET: api/Usuarios/uprueba

        [HttpGet]
        [Route("ShowUsuarios/{correo}/{pass}")]
        public async Task<ActionResult<Usuarios>> ShowUsuarios(String correo, String pass)
        {
            var Usuarios = await _context.Usuarios.Where(u => u.Correo == correo && u.Contrasenia == pass).SingleOrDefaultAsync();
            if (Usuarios == null)
            {
                return NotFound();
            }

            return Usuarios;
        }

        [HttpGet]
        [Route("checkData/{data}/")]
        public async Task<ActionResult<Usuarios>> ValidateUser(String data)
        {
            var Datos = await _context.Usuarios.Where(u => u.Correo == data).SingleOrDefaultAsync();// verificamos el correo
            if (Datos == null)
            {
                return NotFound();
            }
            return Datos;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [RequestSizeLimit(100000000)]
        public async Task<ActionResult<Usuarios>> PutUsuario(int id, Usuarios usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return Ok();
                    throw;
                }
            }

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Eventos.Any(e => e.Id == id);
        }

        //-------------------------------------------------------------------------------------------------------------
        //actualizacion del pass del ususario 
        [HttpPut("updpass")]
        public async Task<ActionResult<string>> PutUsuarios(Usuarios usu)
        {
            string result = string.Empty;
            List<MensajeViewModel> msg = new List<MensajeViewModel>();
            if (usu.Correo != null && usu.Contrasenia != null)
            {
                var usuarios = await _context.Usuarios.Where(u => u.Correo == usu.Correo).SingleOrDefaultAsync();
                if (usuarios == null)
                {
                    msg.Add(TipoMensaje.MensajesError("No se logro actualizar la contraseña, usuario no registrado!"));
                }
                else
                {
                    try
                    {

                        usuarios.Contrasenia = usu.Contrasenia;
                        usuarios.CambioContrasenia = null;

                        _context.Update(usuarios);
                        await _context.SaveChangesAsync();
                        msg.Add(TipoMensaje.Exitoso("Se actualizo correctamente el registro"));

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        msg.Add(TipoMensaje.MensajesError("Ha surgIdo un error, favor de intentarlo mas tarde"));
                    }
                }
            }
            else
            {
                msg.Add(TipoMensaje.ErroresAtributos("Nombre"));
            }
            result = JsonConvert.SerializeObject(msg);

            return result;
        }
        //-------------------------------------------------------------------------------------------------------------
        // POST: api/Usuarios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<String>> PostUsuarios(Usuarios usuarios)
        {
            string result = string.Empty;
            List<MensajeViewModel> msg = new List<MensajeViewModel>();
            if (usuarios.Nombre != null)
            {
                usuarios.IsAdmin = false;
                _context.Usuarios.Add(usuarios);
                await _context.SaveChangesAsync();
                CreatedAtAction("GetUsuarios", new { id = usuarios.Id }, usuarios);
                msg.Add(TipoMensaje.Exitoso("Se agrego correctamente el registro"));
            }
            result = JsonConvert.SerializeObject(msg);
            return result;
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<String>> DeleteUsuarios(int Id)
        {
            string result = string.Empty;
            List<MensajeViewModel> msg = new List<MensajeViewModel>();
            if (Id > 0)
            {
                var usuario = _context.Usuarios.FirstOrDefault(c => c.Id == Id);
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                msg.Add(TipoMensaje.Exitoso("Se elimino correctamente el registro"));
            }
            else
            {
                msg.Add(TipoMensaje.Exitoso("Se elimino correctamente el registro"));
            }

            result = JsonConvert.SerializeObject(msg);
            return result;

        }

        [HttpGet("SendEmail/{user}")]
        public async Task<ActionResult<Usuarios>> SendEmail(string user)
        {
            var usuarios = await _context.Usuarios.SingleOrDefaultAsync(u => u.Correo == user);

            if (usuarios == null)
            {
                return NotFound();
            }

            try
            {
                var rng = new Random();
                usuarios.Contrasenia = usuarios.Contrasenia + rng.Next(-20, 55).ToString();
                usuarios.CambioContrasenia = true;
                _context.Update(usuarios);
                await _context.SaveChangesAsync();
                EnvioCorreo.SendGrid(usuarios.Correo, usuarios.Contrasenia);

            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return usuarios;
        }        
    }
}
