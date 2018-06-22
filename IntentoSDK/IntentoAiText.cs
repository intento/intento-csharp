using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentoSDK
{
    public class IntentoAiText
    {
        Intento intento;
        IntentoAi parent;

        public IntentoAiText(IntentoAi parent)
        {
            this.parent = parent;
            this.intento = parent.Intento;
        }

        public Intento Intento
        { get { return intento; } }

        public IntentoAi Parent
        { get { return parent; } }

        public IntentoAiTextTranslate Translate
        { get { return new IntentoAiTextTranslate(this); } }

    }
}
