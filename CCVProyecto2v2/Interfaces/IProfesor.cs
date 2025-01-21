using CCVProyecto2v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Interfaces
{
   public interface IProfesor
    {
        Task<List<Profesor>> GetProfesores();
        Task<bool> CrearProfesor(MateriaEnum materia, Profesor profesor);
        Task<bool> EliminarProfesor(int profesorId);
    }
}
