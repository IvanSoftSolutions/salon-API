using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using salon_web_api.Models;
using salon_web_api.Utilerias;
using salon_web_api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace salon_web_api.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// GET: api/
        ///
        [HttpPost]
        public async Task<ActionResult<Usuario>> Login(Usuario usuario)
        {
            //string result = string.Empty;
            //List<MensajeViewModel> msg = new List<MensajeViewModel>();
            var usuarios = await _context.Usuario.Where(u => u.Correo == usuario.Correo && u.Contrasenia == usuario.Contrasenia).SingleOrDefaultAsync();

            if (usuarios == null)
                return NotFound();
            //if(usuarios == null)
            //{
            //    msg.Add(TipoMensaje.MensajesError("Usuarios y/o Contraseña incorrectos"));
            //}
            //else
            //{
            //    msg.Add(TipoMensaje.Exitoso("Se agrego correctamente el registro"));
            //}
            
            //result = JsonConvert.SerializeObject(msg);


            return usuarios;
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
