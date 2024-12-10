using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Utilidades
{
    public class CMensajeria:ValueChangedMessage<CCuerpo>
    {
        public CMensajeria(CCuerpo value):base(value) { }
    }
}
