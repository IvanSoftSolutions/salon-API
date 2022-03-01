using System;
using System.Collections.Generic;

namespace salon_web_api.Models
{
    public partial class Imagenes
    {
        public int Id { get; set; }
        public string Imagen { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Album { get; set; }
        public bool Highlight { get; set; }
    }
}
