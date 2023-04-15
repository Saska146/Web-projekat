using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebFitnCent.Models
{
    public class UCPomocna
    {
       public void UpisiKorisnika(object korisnici)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jSearializer =
                   new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonData = jSearializer.Serialize(korisnici);//userList means your list<> object

            System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("/JSONFajlovi/Korisnici.json"), jsonData);// You can use your own file path here
        }

        public List<Korisnik> UcitajKorisnike()
        {
            //ovo ces na kraju da obrises
            if(File.Exists(HttpContext.Current.Server.MapPath("/JSONFajlovi/Korisnici.json")) == false)
            {
                UpisiKorisnika(new List<Korisnik>());
                return new List<Korisnik>();
            }

            string json = File.ReadAllText(HttpContext.Current.Server.MapPath("/JSONFajlovi/Korisnici.json"));
             List<Korisnik>  korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(json);
            return korisnici;
         }

        public void UpisFitnesCentara(object fitnesCentri)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jSearializer =
                   new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonData = jSearializer.Serialize(fitnesCentri);

            System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("/JSONFajlovi/FitnesCentri.json"), jsonData);// You can use your own file path here
        }

        public List<FitnesCentar> UcitajFitnesCentre()
        {
            //ovo ces na kraju da obrises
            if (File.Exists(HttpContext.Current.Server.MapPath("/JSONFajlovi/FitnesCentri.json")) == false)
            {
                UpisFitnesCentara(new List<FitnesCentar>());
                return new List<FitnesCentar>();
            }

            string json = File.ReadAllText(HttpContext.Current.Server.MapPath("/JSONFajlovi/FitnesCentri.json"));
            List<FitnesCentar> fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(json);
            return fitnesCentri;
        }



        public void UpisGrupnihTreninga(object grupniTreninzi)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jSearializer =
                   new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonData = jSearializer.Serialize(grupniTreninzi);

            System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("/JSONFajlovi/GrupniTreninzi.json"), jsonData);// You can use your own file path here
        }

        public List<GrupniTrening> UcitajGrupneTreninge()
        {
            //ovo ces na kraju da obrises
            if (File.Exists(HttpContext.Current.Server.MapPath("/JSONFajlovi/GrupniTreninzi.json")) == false)
            {
                UpisGrupnihTreninga(new List<GrupniTrening>());
                return new List<GrupniTrening>();
            }

            string json = File.ReadAllText(HttpContext.Current.Server.MapPath("/JSONFajlovi/GrupniTreninzi.json"));
            List<GrupniTrening> grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
            return grupniTreninzi;
        }

        public void UpisKomentara(object komentari)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jSearializer =
                   new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonData = jSearializer.Serialize(komentari);

            System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("/JSONFajlovi/Komentari.json"), jsonData);// You can use your own file path here
        }

        public List<Komentar> UcitajKomentare()
        {
            //ovo ces na kraju da obrises
            if (File.Exists(HttpContext.Current.Server.MapPath("/JSONFajlovi/Komentari.json")) == false)
            {
                UpisKomentara(new List<Komentar>());
                return new List<Komentar>();
            }

            string json = File.ReadAllText(HttpContext.Current.Server.MapPath("/JSONFajlovi/Komentari.json"));
            List<Komentar> komentari = JsonConvert.DeserializeObject<List<Komentar>>(json);
            return komentari;
        }

    }
}
