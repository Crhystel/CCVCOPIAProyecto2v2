using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Dto
{
    public partial class ClaseProfesorDto:ObservableObject
    {
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public int claseId;
        [ObservableProperty]
        public int profesorId;
        [ObservableProperty]
        public string profesorNombre;
        [ObservableProperty]
        public string claseNombre;
    }
}
