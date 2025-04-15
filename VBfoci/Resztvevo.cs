using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBfoci
{
    internal class Resztvevo
    {
        //Készítette: Tóth Róbert, Ács Norbert
        public string Orszag { get; set; }
        public int Helyezes { get; set; }
        public int Ev { get; set; }
        public string Helyszin { get; set; }

        //pontosvesszővel elválasztott szövegből töltjük fel az adatokat
        public Resztvevo(string sor) 
        {
            var adatok = sor.Split(';');
            Orszag = adatok[0];
            Helyezes = int.Parse(adatok[1]);
            Ev = int.Parse(adatok[2]);
            Helyszin = adatok[3];
        }  
    }
}
