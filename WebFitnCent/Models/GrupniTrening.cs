using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFitnCent.Models
{
    public class GrupniTrening
    {
        private string id;
        private string naziv;
        private string tip_treninga;
        private string fitnes_centar;
        private int trajanje_treninga;
        private string datum_i_vreme_treninga;
        private int maksimalan_broj_posetilaca;
        private List<string> spisak_posetilaca;
        private bool obrisan;

        public string Id { get => id; set => id = value; }
        public string Naziv { get => naziv; set => naziv = value; }
        public string Tip_treninga { get => tip_treninga; set => tip_treninga = value; }
        public string Fitnes_centar { get => fitnes_centar; set => fitnes_centar = value; }
        public int Trajanje_treninga { get => trajanje_treninga; set => trajanje_treninga = value; }
        public string Datum_i_vreme_treninga { get => datum_i_vreme_treninga; set => datum_i_vreme_treninga = value; }
        public int Maksimalan_broj_posetilaca { get => maksimalan_broj_posetilaca; set => maksimalan_broj_posetilaca = value; }
        public List<string> Spisak_posetilaca { get => spisak_posetilaca; set => spisak_posetilaca = value; }
        public bool Obrisan { get => obrisan; set => obrisan = value; }
    }
}