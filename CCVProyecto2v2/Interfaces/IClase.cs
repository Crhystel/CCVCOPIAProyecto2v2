using CCVProyecto2v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Interfaces
{
    public interface IClase
    {
        Task<List<Clase>> GetClases();
        Task<bool> CrearClase(Clase clase);
        Task<bool> EliminarClase(int claseId);
        Task<bool> ActualizarClase(int claseId, Clase clase);

    }
}
