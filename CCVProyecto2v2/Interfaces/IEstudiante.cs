using CCVProyecto2v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Interfaces
{
    public interface IEstudiante
    {
        Task<List<Estudiante>> GetEstudiantes();
        Task<bool> CrearEstudiante(GradoEnum grado,Estudiante estudiante);
        Task<bool> EliminarEstudiante(int estudianteId);
        Task<bool> ActualizarEstudiante(int estudianteId, Estudiante estudiante);
    }
}
