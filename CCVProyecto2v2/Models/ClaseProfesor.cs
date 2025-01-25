using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Models
{
    public class ClaseProfesor
    {
        [Key]
        public int Id { get; set; }
        public int ClaseId { get; set; }
        public Clase Clase { get; set; }
        public int ProfesorId { get; set; }
        public Profesor Profesor { get; set; }
        public string ClaseNombre { get; set; }
        public string ProfesorNombre { get; set; }
    }
}
