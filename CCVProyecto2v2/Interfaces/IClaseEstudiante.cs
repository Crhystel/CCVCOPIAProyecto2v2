using CCVProyecto2v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Interfaces
{
    public interface IClaseEstudiante
    {
        Task<List<ClaseEstudiante>> GetClaseEstudiantes();
        Task<bool> CrearClaseEstudiante(ClaseEstudiante claseEstudiante);
        Task<bool> EliminarClaseEstudiante(int claseEstudianteId);
        Task<bool> ActualizarClaseEstudiante(int claseEstudianteId, ClaseEstudiante claseEstudiante);
    }
}
