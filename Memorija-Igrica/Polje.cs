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
        private string boja;
        private bool pogodjeno;

        public Polje()
        {
            broj = 0;
            boja = "White";
            pogodjeno = false;
        }

        public Polje(int broj, string boja)
        {
            this.broj = broj;
            this.boja = boja;
            pogodjeno = false;
        }

        public bool Pogodjeno
        {
            get { return pogodjeno; }
            set { pogodjeno = value; }
        }

        public string Boja
        {
            get { return boja; }
            set { boja = value; }
        }
        public int Broj {
            get { return broj; } 
            set { broj = value; }
        }


    }
}
