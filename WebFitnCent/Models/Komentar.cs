using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFitnCent.Models
{
    public class Komentar
    {
        private string id;
        private string posetilac_korisnicko;
        private string fitnes_Centar_id;
        private string tekst_komentara;
        private int ocena;
        private bool vidljiv;

        public string Id { get => id; set => id = value; }
        public string Tekst_komentara { get => tekst_komentara; set => tekst_komentara = value; }
        public int Ocena { get => ocena; set => ocena = value; }
        public bool Vidljiv { get => vidljiv; set => vidljiv = value; }
        public string Posetilac_korisnicko { get => posetilac_korisnicko; set => posetilac_korisnicko = value; }
        public string Fitnes_Centar_id { get => fitnes_Centar_id; set => fitnes_Centar_id = value; }
    }
}