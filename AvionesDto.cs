using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T11A
{
    public class AvionesDto
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Matricula { get; set; }
        public Nullable<bool> Activo { get; set; }
    }
}
