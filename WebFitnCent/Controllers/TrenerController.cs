using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebFitnCent.Models;

namespace WebFitnCent.Controllers
{
    public class TrenerController : Controller
    {
        UCPomocna upis_citanje = new UCPomocna();
        // GET: Trener
        public ActionResult Opcije()
        {
            return View();
        }

        public ActionResult Kreiraj()
        {
            return View();
        }

        public ActionResult Dodaj()
        {
            var korisnickoImeUlogovanog = ((Korisnik)Session["korisnik"]).Korisnicko_ime;
            var id_fitnes_centra = ((Korisnik)Session["korisnik"]).FitnesCentar.Id;
            StringBuilder id = new StringBuilder();
            Enumerable
                .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(12)
                .ToList().ForEach(e => id.Append(e));

            GrupniTrening grupniTrening = new GrupniTrening();

            grupniTrening.Id = id.ToString();
            grupniTrening.Naziv = Request.Form["nzv"];
            grupniTrening.Tip_treninga = Request.Form["tip"];
            grupniTrening.Trajanje_treninga = Int32.Parse(Request.Form["trajanje"]);
            if(DateTime.Parse(Request.Form["datumivr"]) < DateTime.Now.AddDays(3))// nije moguce kreirati u proslosti
            {
                ViewData["Greska"] = "Nije moguce kreirati Grupni Trening u Proslosti";
                return View("Kreiraj");
            }
            grupniTrening.Datum_i_vreme_treninga = Request.Form["datumivr"];
            grupniTrening.Maksimalan_broj_posetilaca = Int32.Parse(Request.Form["makspos"]);
          
            grupniTrening.Fitnes_centar = id_fitnes_centra;
            grupniTrening.Spisak_posetilaca = new List<string>();
            grupniTrening.Obrisan = false;

            var korisnici = upis_citanje.UcitajKorisnike();
            var grupni_trenizni = upis_citanje.UcitajGrupneTreninge();
            grupni_trenizni.Add(grupniTrening);

            var korisnik = korisnici.First(kor => kor.Korisnicko_ime == korisnickoImeUlogovanog);
            korisnik.Spisak_treninga.Add(grupniTrening);

            upis_citanje.UpisGrupnihTreninga(grupni_trenizni);
            upis_citanje.UpisiKorisnika(korisnici);

            return View("Kreiraj");
        }

        public ActionResult PogledajSvojeGrupneTreninge()
        {
            var ulogovani = upis_citanje.UcitajKorisnike().First(ulog => ulog.Korisnicko_ime == ((Korisnik)Session["korisnik"]).Korisnicko_ime);
            ViewData["lista_grupnih_treninga"] = ulogovani.Spisak_treninga.Where(grup => grup.Obrisan == false).ToList();
            return View();
        }

        public ActionResult OdaberiGrupniTrening()
        {
            var ulogovani = upis_citanje.UcitajKorisnike().First(ulog => ulog.Korisnicko_ime == ((Korisnik)Session["korisnik"]).Korisnicko_ime );
            ViewData["grupniTreninziZaPrikaz"] = ulogovani.Spisak_treninga.Where(grup => grup.Obrisan == false && grup.Spisak_posetilaca.Count == 0 && DateTime.Parse(grup.Datum_i_vreme_treninga) > DateTime.Now).ToList();

            return View();
        }

        public ActionResult Modifikuj()
        {
            ViewData["id"] = Request.Form["id"];
            return View();
        }

        public ActionResult Modifikacija()
        {
            var id = Request.Form["id"];
            var grupniTreninzi = upis_citanje.UcitajGrupneTreninge();
            var grupniTrening = grupniTreninzi.First(grup => grup.Id == id);
            var korisnickoImeUlogovanog = ((Korisnik)Session["korisnik"]).Korisnicko_ime;


            List<string> modifikovana_polja = new List<string>();

            modifikovana_polja = ProveraPolja();

            if (modifikovana_polja.Contains("nzv"))
            {
                grupniTrening.Naziv = Request.Form["nzv"];
            }
            if (modifikovana_polja.Contains("tip"))
            {
                grupniTrening.Tip_treninga = Request.Form["tip"];
            }
            if (modifikovana_polja.Contains("trajanje"))
            {
                grupniTrening.Trajanje_treninga = Int32.Parse(Request.Form["trajanje"]);
            }
            if (modifikovana_polja.Contains("datumivr"))
            {
                grupniTrening.Datum_i_vreme_treninga = Request.Form["datumivr"];
            }
            if (modifikovana_polja.Contains("makspos"))
            {
                grupniTrening.Maksimalan_broj_posetilaca = Int32.Parse(Request.Form["makspos"]);
            }
          
            var korisnici = upis_citanje.UcitajKorisnike();
            var index = 0;
            korisnici.ForEach(
                kor => {

                if (kor.Spisak_treninga != null && kor.Spisak_treninga.Where(grup => grup.Id == grupniTrening.Id).ToList().Count != 0)
                {
                        index = kor.Spisak_treninga.FindIndex(ind => ind.Id == grupniTrening.Id);
                        kor.Spisak_treninga.RemoveAt(index);
                        kor.Spisak_treninga.Insert(index, grupniTrening);
                }

            });

       
            upis_citanje.UpisGrupnihTreninga(grupniTreninzi);
            upis_citanje.UpisiKorisnika(korisnici);

            return RedirectToAction("OdaberiGrupniTrening");

        }

        public ActionResult Izbrisi()
        {
            var id = Request.Form["id"];

            var korisnici = upis_citanje.UcitajKorisnike();
            var ulogovani = korisnici.First(ulog => ulog.Korisnicko_ime == ((Korisnik)Session["korisnik"]).Korisnicko_ime);
            ulogovani.Spisak_treninga.First(grup => grup.Id == id).Obrisan = true;

            var grupniTreninzi = upis_citanje.UcitajGrupneTreninge();
            var grupniTrening = grupniTreninzi.First(grup => grup.Id == id);


            grupniTrening.Obrisan = true;
        
            upis_citanje.UpisGrupnihTreninga(grupniTreninzi);
            upis_citanje.UpisiKorisnika(korisnici);

            return View("Opcije");

        }

        public List<string> ProveraPolja()
        {
            List<string> modifikovana_polja = new List<string>();


            if (Request.Form["nzv"] != "")
            {
                modifikovana_polja.Add("nzv");
            }
            if (Request.Form["tip"] != "")
            {
                modifikovana_polja.Add("tip");
            }
            if (Request.Form["trajanje"] != "")
            {
                modifikovana_polja.Add("trajanje");
            }
            if (Request.Form["datumivr"] != "")
            {
                modifikovana_polja.Add("datumivr");
            }
            if (Request.Form["makspos"] != "")
            {
                modifikovana_polja.Add("makspos");
            }
     
            return modifikovana_polja;
        }

        public ActionResult PogledajProsleGrupneTreninge()
        {
            var ulogovani = upis_citanje.UcitajKorisnike().First(ulog => ulog.Korisnicko_ime == ((Korisnik)Session["korisnik"]).Korisnicko_ime);
            ViewData["lista_grupnih_treninga"] = ulogovani.Spisak_treninga.Where(grup => grup.Obrisan == false && DateTime.Parse(grup.Datum_i_vreme_treninga) < DateTime.Now).ToList();
            return View();
        }

        public ActionResult Sortiraj()
        {
            var ulogovani = upis_citanje.UcitajKorisnike().First(ulog => ulog.Korisnicko_ime == ((Korisnik)Session["korisnik"]).Korisnicko_ime);
            var sortLista = ulogovani.Spisak_treninga.Where(grup => grup.Obrisan == false && DateTime.Parse(grup.Datum_i_vreme_treninga) < DateTime.Now).ToList();

            var sta = Request["sta"];
            var vrsta = Request["vrsta"];

            if(sta == "Nazivu" && vrsta == "Rastuce")
            {
                sortLista.Sort((a, b) => a.Naziv.CompareTo(b.Naziv));
            }

            if (sta == "Nazivu" && vrsta == "Opadajuce")
            {
                sortLista.Sort((a, b) => b.Naziv.CompareTo(a.Naziv));
            }

            if (sta == "Tipu" && vrsta == "Rastuce")
            {
                sortLista.Sort((a, b) => a.Tip_treninga.CompareTo(b.Tip_treninga));
            }

            if (sta == "Tipu" && vrsta == "Opadajuce")
            {
                sortLista.Sort((a, b) => b.Tip_treninga.CompareTo(a.Tip_treninga));
            }

            if (sta == "Datumu" && vrsta == "Rastuce")
            {
                sortLista.Sort((a, b) => a.Datum_i_vreme_treninga.CompareTo(b.Datum_i_vreme_treninga));
            }

            if (sta == "Datumu" && vrsta == "Opadajuce")
            {
                sortLista.Sort((a, b) => b.Datum_i_vreme_treninga.CompareTo(a.Datum_i_vreme_treninga));
            }

            ViewData["lista_grupnih_treninga"] = sortLista;
            return View("PogledajProsleGrupneTreninge");
        }

        public ActionResult Pretrazi()
        {
            Dictionary<string, string> polja_za_pretragu = new Dictionary<string, string>();
            var ulogovani = upis_citanje.UcitajKorisnike().First(ulog => ulog.Korisnicko_ime == ((Korisnik)Session["korisnik"]).Korisnicko_ime);
            var pretragaLista = ulogovani.Spisak_treninga.Where(grup => grup.Obrisan == false && DateTime.Parse(grup.Datum_i_vreme_treninga) < DateTime.Now).ToList();

            polja_za_pretragu = ProveraPoljaZaPretragu();

            if (polja_za_pretragu.ContainsKey("nzv"))
            {
                pretragaLista = pretragaLista.Where(pret => pret.Naziv == polja_za_pretragu["nzv"]).ToList();
            }
            if (polja_za_pretragu.ContainsKey("tip"))
            {
                pretragaLista = pretragaLista.Where(pret => pret.Tip_treninga == polja_za_pretragu["tip"]).ToList();
            }
            if (polja_za_pretragu.ContainsKey("datumivrminimum"))
            {
                pretragaLista = pretragaLista.Where(pret => DateTime.Parse(pret.Datum_i_vreme_treninga) >= DateTime.Parse(polja_za_pretragu["datumivrminimum"])).ToList();
            }
            if (polja_za_pretragu.ContainsKey("datumivrmaksimum"))
            {
                pretragaLista = pretragaLista.Where(pret => DateTime.Parse(pret.Datum_i_vreme_treninga) <= DateTime.Parse(polja_za_pretragu["datumivrmaksimum"])).ToList();
            }

            ViewData["lista_grupnih_treninga"] = pretragaLista;

            return View("PogledajProsleGrupneTreninge");
        }

        public Dictionary<string,string> ProveraPoljaZaPretragu()
        {
            Dictionary<string, string> polja_za_pretragu = new Dictionary<string, string>();

            if (Request.Form["nzv"] != "")
            {
                polja_za_pretragu["nzv"] = Request["nzv"];
            }
            if (Request.Form["tip"] != "")
            {
                polja_za_pretragu["tip"] = Request["tip"];
            }
            if (Request.Form["datumivrminimum"] != "")
            {
                polja_za_pretragu["datumivrminimum"] = Request["datumivrminimum"];
            }
            if (Request.Form["datumivrmaksimum"] != "")
            {
                polja_za_pretragu["datumivrmaksimum"] = Request["datumivrmaksimum"];
            }

            return polja_za_pretragu;
        }
    }
}