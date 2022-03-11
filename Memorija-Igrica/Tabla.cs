using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorija_Igrica
{
    internal class Tabla
    {
        private Polje[] polja;
        private bool game; // da li je dozvoljeno da se igra

        public bool Game
        {
            get { return game; }
            set { game = value; }
        }

        public Tabla()
        {
            polja = new Polje[20];
            for(int i = 0; i < polja.Length; i++)
            {
                polja[i] = new Polje();
            }
            game = false;
        }
        public Polje[] Polja
        {
            get { return polja; }
            set { polja = value; }
        }


    }
}
