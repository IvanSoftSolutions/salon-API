using System;
using System.Collections.Generic;

namespace salon_web_api.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public bool Estado { get; set; }

        public virtual Eventos Event { get; set; } = null!;
        public virtual Usuarios User { get; set; } = null!;
    }
}
