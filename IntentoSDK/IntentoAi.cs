using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentoSDK
{
    public class IntentoAi
    {
        Intento intento;

        public IntentoAi(Intento intento)
        {
            this.intento = intento;
        }

        public Intento Intento
        { get { return intento; } }

        public Intento Parent
        { get { return intento; } }

        public IntentoAiText Text
        { get { return new IntentoAiText(this); } }

    }
}
