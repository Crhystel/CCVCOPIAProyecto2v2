using CCVProyecto2v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Interfaces
{
    public interface IClaseProfesor
    {
        Task<List<ClaseProfesor>> GetClaseProfesores();
        Task<bool> CrearClaseProfesor(ClaseProfesor claseProfesor);
        Task<bool> EliminarClaseProfesor(int claseProfesorId);
        Task<bool> ActualizarClaseProfesor(int claseProfesorId, ClaseProfesor claseProfesor);
    }
}
