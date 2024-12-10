using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Models
{
    public class Clase
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int ProfesorId { get; set; } 
        public Profesor Profesor { get; set; }
        public ICollection<ClaseEstudiante> ClasesEstudiantes { get; set; } = new List<ClaseEstudiante>();
    }
}
