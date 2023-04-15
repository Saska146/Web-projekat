using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFitnCent.Models
{
    public class Korisnik
    {
        private string korisnicko_ime;
        private string lozinka;
        private string ime;
        private string prezime;
        private string pol;
        private string email;
        private string datum;
        private string uloga;
        private List<GrupniTrening> spisak_treninga;
        private List<FitnesCentar> fitnesCentri;
        private bool obrisan;
        private FitnesCentar fitnesCentar;

        public string Korisnicko_ime { get => korisnicko_ime; set => korisnicko_ime = value; }
        public string Lozinka { get => lozinka; set => lozinka = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public string Pol { get => pol; set => pol = value; }
        public string Email { get => email; set => email = value; }
        public string Datum { get => datum; set => datum = value; }
        public string Uloga { get => uloga; set => uloga = value; }
        public List<FitnesCentar> FitnesCentri { get => fitnesCentri; set => fitnesCentri = value; }
        public bool Obrisan { get => obrisan; set => obrisan = value; }
        public FitnesCentar FitnesCentar { get => fitnesCentar; set => fitnesCentar = value; }
        public List<GrupniTrening> Spisak_treninga { get => spisak_treninga; set => spisak_treninga = value; }
    }
}