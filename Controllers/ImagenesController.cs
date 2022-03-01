using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class ImagenesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ImagenesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Imagenes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Imagenes>>> GetImagenes()
        {
            return await _context.Imagenes.ToListAsync();
        }

        // GET: api/Imagenes/5
        [HttpGet("{highlight}")]
        public async Task<ActionResult<IEnumerable<Imagenes>>> GetImagenes(bool isHighLight)
        {
            return await _context.Imagenes.Where(i => i.Highlight == isHighLight).ToListAsync();
        }

        // POST: api/Imagenes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<String>> PostImagenes(Imagenes imagenes)
        {
            string result = string.Empty;
            List<MensajeViewModel> msg = new List<MensajeViewModel>();
            if ( imagenes.Imagen != null)
            {
                _context.Imagenes.Add(imagenes);
                await _context.SaveChangesAsync();
                msg.Add(TipoMensaje.Exitoso("Se agrego correctamente el registro"));
            }

            result = JsonConvert.SerializeObject(msg);

            return result;
        }

        // DELETE: api/Imagenes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteImagenes(int id)
        {
            string result = string.Empty;
            List<MensajeViewModel> msg = new List<MensajeViewModel>();

            if (id > 0)
            {
                var imagenes = _context.Imagenes.FirstOrDefault(a => a.Id == id);
                _context.Imagenes.Remove(imagenes);
                await _context.SaveChangesAsync();
                msg.Add(TipoMensaje.Exitoso("Se elimino correctamente el registro"));
            }

            result = JsonConvert.SerializeObject(msg);

            return result;
        }
    }
}
