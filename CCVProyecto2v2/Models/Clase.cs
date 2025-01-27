using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CCVProyecto2v2.Models
{
    public class Clase
    {
        [Key]
        

        public int Id { get; set; }

        public string Nombre { get; set; }
       
    }
}
