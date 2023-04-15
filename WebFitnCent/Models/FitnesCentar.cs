using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFitnCent.Models
{
    public class FitnesCentar
    {
        private string id;
        private string naziv;
        private string adresa;
        private string godina_otvaranja;
        private string vlasnik;
        private float cena_mesecne;
        private float cena_godisnje;
        private float cena_jednog;
        private float cena_grupnog;
        private float cena_personalnog;
        private bool obrisan;

        public string Naziv { get => naziv; set => naziv = value; }
        public string Adresa { get => adresa; set => adresa = value; }
        public string Godina_otvaranja { get => godina_otvaranja; set => godina_otvaranja = value; }
        public string Vlasnik { get => vlasnik; set => vlasnik = value; }
        public float Cena_mesecne { get => cena_mesecne; set => cena_mesecne = value; }
        public float Cena_godisnje { get => cena_godisnje; set => cena_godisnje = value; }
        public float Cena_jednog { get => cena_jednog; set => cena_jednog = value; }
        public float Cena_grupnog { get => cena_grupnog; set => cena_grupnog = value; }
        public float Cena_personalnog { get => cena_personalnog; set => cena_personalnog = value; }
        public string Id { get => id; set => id = value; }
        public bool Obrisan { get => obrisan; set => obrisan = value; }
    }
}