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
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            return await _context.Usuario.ToListAsync();
        }


        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        //buscamos usuario por nombre de usuario
        // GET: api/Usuario/uprueba

        [HttpGet]
        [Route("ShowUsuario/{correo}/{pass}")]
        public async Task<ActionResult<Usuario>> ShowUsuario(String correo, String pass)
        {
            var Usuario = await _context.Usuario.Where(u => u.Correo == correo && u.Contrasenia == pass).SingleOrDefaultAsync();
            if (Usuario == null)
            {
                return NotFound();
            }

            return Usuario;
        }

        [HttpGet]
        [Route("checkData/{data}/")]
        public async Task<ActionResult<Usuario>> ValidateUser(String data)
        {
            var Datos = await _context.Usuario.Where(u => u.Correo == data).SingleOrDefaultAsync();// verificamos el correo
            if (Datos == null)
            {
                return NotFound();
            }
            return Datos;
        }

        // PUT: api/Usuario/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [RequestSizeLimit(100000000)]
        public async Task<ActionResult<Usuario>> PutUsuario(int id, Usuario usuario)
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
        public async Task<ActionResult<string>> PutUsuario(Usuario usu)
        {
            string result = string.Empty;
            List<MensajeViewModel> msg = new List<MensajeViewModel>();
            if (usu.Correo != null && usu.Contrasenia != null)
            {
                var usuario = await _context.Usuario.Where(u => u.Correo == usu.Correo).SingleOrDefaultAsync();
                if (usuario == null)
                {
                    msg.Add(TipoMensaje.MensajesError("No se logro actualizar la contraseña, usuario no registrado!"));
                }
                else
                {
                    try
                    {

                        usuario.Contrasenia = usu.Contrasenia;
                        usuario.CambioContrasenia = null;

                        _context.Update(usuario);
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
        // POST: api/Usuario
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<String>> PostUsuario(Usuario usuario)
        {
            string result = string.Empty;
            List<MensajeViewModel> msg = new List<MensajeViewModel>();
            if (usuario.Nombre != null)
            {
                usuario.IsAdmin = false;
                _context.Usuario.Add(usuario);
                await _context.SaveChangesAsync();
                CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
                msg.Add(TipoMensaje.Exitoso("Se agrego correctamente el registro"));
            }
            result = JsonConvert.SerializeObject(msg);
            return result;
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<String>> DeleteUsuario(int Id)
        {
            string result = string.Empty;
            List<MensajeViewModel> msg = new List<MensajeViewModel>();
            if (Id > 0)
            {
                var usuario = _context.Usuario.FirstOrDefault(c => c.Id == Id);
                _context.Usuario.Remove(usuario);
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
        public async Task<ActionResult<Usuario>> SendEmail(string user)
        {
            var usuario = await _context.Usuario.SingleOrDefaultAsync(u => u.Correo == user);

            if (usuario == null)
            {
                return NotFound();
            }

            try
            {
                var rng = new Random();
                usuario.Contrasenia = usuario.Contrasenia + rng.Next(-20, 55).ToString();
                usuario.CambioContrasenia = true;
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                EnvioCorreo.SendGrid(usuario.Correo, usuario.Contrasenia);

            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return usuario;
        }        
    }
}
