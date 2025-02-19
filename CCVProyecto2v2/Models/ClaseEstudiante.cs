﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Models
{
    public class ClaseEstudiante
    {
        [Key]
        public int Id { get; set; }
        public int ClaseId { get; set; }
        public Clase Clase { get; set; }
        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }
        public string ClaseNombre { get; set; }
        public string EstudianteNombre { get; set; }
    }
}
