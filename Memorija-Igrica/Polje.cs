using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorija_Igrica
{
    internal class Polje
    {
        private int broj;
        private bool pogodjeno; // Da li je polje i dalje u igri

        public Polje()
        {
            broj = 0;
            pogodjeno = false;
        }

        public Polje(int broj)
        {
            this.broj = broj;
            pogodjeno = false;
        }

        public bool Pogodjeno
        {
            get { return pogodjeno; }
            set { pogodjeno = value; }
        }

        public int Broj {
            get { return broj; } 
            set { broj = value; }
        }


    }
}
