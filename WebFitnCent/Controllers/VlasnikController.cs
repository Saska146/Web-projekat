using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebFitnCent.Models;

namespace WebFitnCent.Controllers
{
    public class VlasnikController : Controller
    {
        UCPomocna upis_citanje = new UCPomocna();

        // GET: Vlasnik
        public ActionResult Opcije()
        {
            return View();
        }

        //kreiraj Fitnes Centar
        public ActionResult Kreiraj()
        {
       
            return View();
        }

        public ActionResult Dodaj()
        {
            var korisnickoImeUlogovanog = ((Korisnik)Session["korisnik"]).Korisnicko_ime;
            StringBuilder id = new StringBuilder();
            Enumerable
                .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(12)
                .ToList().ForEach(e => id.Append(e));

            FitnesCentar fitnesCentar = new FitnesCentar();

            fitnesCentar.Id = id.ToString();
            fitnesCentar.Naziv = Request.Form["nzv"];
            fitnesCentar.Adresa = Request.Form["adresa"];
            fitnesCentar.Godina_otvaranja = Request.Form["god_otv"];
            fitnesCentar.Cena_mesecne = float.Parse(Request.Form["mesecna"]);
            fitnesCentar.Cena_godisnje = float.Parse(Request.Form["godisnja"]);
            fitnesCentar.Cena_jednog = float.Parse(Request.Form["jednog"]);
            fitnesCentar.Cena_grupnog = float.Parse(Request.Form["grupnog"]);
            fitnesCentar.Cena_personalnog = float.Parse(Request.Form["personalni"]);
            fitnesCentar.Vlasnik = ((Korisnik)Session["korisnik"]).Korisnicko_ime;
            fitnesCentar.Obrisan = false;

            var korisnici = upis_citanje.UcitajKorisnike();
            var fitnesCentri = upis_citanje.UcitajFitnesCentre();
            fitnesCentri.Add(fitnesCentar);

            var korisnik = korisnici.First(kor => kor.Korisnicko_ime == korisnickoImeUlogovanog);
            korisnik.FitnesCentri.Add(fitnesCentar);

            upis_citanje.UpisFitnesCentara(fitnesCentri);
            upis_citanje.UpisiKorisnika(korisnici);

            return RedirectToAction("Kreiraj");

        }

        public ActionResult OdaberiFitnesCentar()
        {
        
            ViewData["fitnesCentriZaPrikaz"] = upis_citanje.UcitajFitnesCentre().Where(fitn => fitn.Vlasnik == ((Korisnik)Session["korisnik"]).Korisnicko_ime && fitn.Obrisan == false).ToList();
            
            return View("");
        }


        public ActionResult Modifikuj()
        {
            ViewData["id"] = Request.Form["id"];
            return View();
        }


        public ActionResult Modifikacija()
        {
            var id = Request.Form["id"];
            var fitnesCentri = upis_citanje.UcitajFitnesCentre();
            var fitnesCentar = fitnesCentri.First(fcnt => fcnt.Id == id);
            var korisnickoImeUlogovanog = ((Korisnik)Session["korisnik"]).Korisnicko_ime;


            List<string> modifikovana_polja = new List<string>();

            modifikovana_polja = ProveraPolja();

            if (modifikovana_polja.Contains("nzv"))
            {
                fitnesCentar.Naziv = Request.Form["nzv"];
            }
            if (modifikovana_polja.Contains("adresa"))
            {
                fitnesCentar.Adresa = Request.Form["adresa"];
            }
            if (modifikovana_polja.Contains("god_otv"))
            {
                fitnesCentar.Godina_otvaranja = Request.Form["god_otv"];
            }
            if (modifikovana_polja.Contains("mesecna"))
            {
                fitnesCentar.Cena_mesecne = float.Parse(Request.Form["mesecna"]);
            }
            if (modifikovana_polja.Contains("godisnja"))
            {
                fitnesCentar.Cena_godisnje = float.Parse(Request.Form["godisnja"]);
            }
            if (modifikovana_polja.Contains("jednog"))
            {
                fitnesCentar.Cena_jednog = float.Parse(Request.Form["jednog"]);
            }
            if (modifikovana_polja.Contains("grupnog"))
            {
                fitnesCentar.Cena_grupnog = float.Parse(Request.Form["grupnog"]);
            }
            if (modifikovana_polja.Contains("personalni"))
            {
                fitnesCentar.Cena_personalnog = float.Parse(Request.Form["personalni"]);
            }


            var korisnici = upis_citanje.UcitajKorisnike();
            korisnici.ForEach(kor => { if (kor.FitnesCentar != null && kor.FitnesCentar.Id == fitnesCentar.Id) { kor.FitnesCentar = fitnesCentar; } });

            var korisnik = korisnici.First(kor => kor.Korisnicko_ime == korisnickoImeUlogovanog);
            korisnik.FitnesCentri = (List<FitnesCentar>)fitnesCentri.Where(fcnt => fcnt.Vlasnik == korisnickoImeUlogovanog).ToList();

            upis_citanje.UpisFitnesCentara(fitnesCentri);
            upis_citanje.UpisiKorisnika(korisnici);

            return RedirectToAction("OdaberiFitnesCentar");

        }

        public ActionResult Izbrisi()
        {
            var id = Request.Form["id"];

            var korisnici = (List<Korisnik>)upis_citanje.UcitajKorisnike();
            var ulogovani = korisnici.First(ulog => ulog.Korisnicko_ime == ((Korisnik)Session["korisnik"]).Korisnicko_ime);
            ulogovani.FitnesCentri.First(fitn => fitn.Id == id).Obrisan = true;

            var fitnesCentri = upis_citanje.UcitajFitnesCentre();
            var fitnesCentar = fitnesCentri.First(fcnt => fcnt.Id == id);

            var grupni = (List<GrupniTrening>)upis_citanje.UcitajGrupneTreninge();
            var fi = upis_citanje.UcitajFitnesCentre();
            List<string> vlasnikoviFitnesCentri = upis_citanje.UcitajFitnesCentre().Where(fitn => fitn.Vlasnik == ((Korisnik)Session["korisnik"]).Korisnicko_ime).ToList().Select(o => o.Id).ToList();
            var grupni_uBuducnosti = grupni.Where(grup => vlasnikoviFitnesCentri.Contains(grup.Fitnes_centar) && grup.Obrisan == false && DateTime.Parse(grup.Datum_i_vreme_treninga) > DateTime.Now).ToList();

            if(grupni_uBuducnosti.Count == 0)
            {
                fitnesCentar.Obrisan = true;
                korisnici.ForEach(kor => { if (kor.Uloga == "TRENER" && kor.FitnesCentar.Id == fitnesCentar.Id) { kor.Obrisan = true; } });

                upis_citanje.UpisFitnesCentara(fitnesCentri);
                upis_citanje.UpisiKorisnika(korisnici);
            }

            return View("Opcije");


        }

        public List<string> ProveraPolja()
        {
            List<string> modifikovana_polja = new List<string>();


            if (Request.Form["nzv"] != "")
            {
                modifikovana_polja.Add("nzv");
            }
            if (Request.Form["adresa"] != "")
            {
                modifikovana_polja.Add("adresa");
            }
            if (Request.Form["god_otv"] != "")
            {
                modifikovana_polja.Add("god_otv");
            }
            if (Request.Form["mesecna"] != "")
            {
                modifikovana_polja.Add("mesecna");
            }
            if (Request.Form["godisnja"] != "")
            {
                modifikovana_polja.Add("godisnja");
            }
            if (Request.Form["jednog"] != "")
            {
                modifikovana_polja.Add("jednog");
            }
            if (Request.Form["grupnog"] != "")
            {
                modifikovana_polja.Add("grupnog");
            }
            if (Request.Form["personalni"] != "")
            {
                modifikovana_polja.Add("personalni");
            }

            return modifikovana_polja;
        }

        public ActionResult Regisitruj()
        {
            ViewData["uloga"] = "TRENER";
            ViewData["fitnesCentriZaPrikaz"] = (List<FitnesCentar>)upis_citanje.UcitajFitnesCentre().Where(fitn => fitn.Vlasnik == ((Korisnik)Session["korisnik"]).Korisnicko_ime && fitn.Obrisan == false).ToList();
            if (((List<FitnesCentar>)ViewData["fitnesCentriZaPrikaz"]).Count == 0)
            {
                ViewData["Greska"] = "Nemate Fitnes Centre";
                return View("Opcije");
            }

             return View("../PrijavaRegistracija/Registracija");
        }

        public ActionResult OdaberiTrenera()
        {
            var ulogovani = upis_citanje.UcitajKorisnike().First(ulog => ulog.Korisnicko_ime == ((Korisnik)Session["korisnik"]).Korisnicko_ime);
            var ostali_korisnici = upis_citanje.UcitajKorisnike();

            var fitnes_centri_korisnika = ulogovani.FitnesCentri;
            List<Korisnik> lista_trenera = new List<Korisnik>();

            foreach (var korisnik in ostali_korisnici)
            {
                if (fitnes_centri_korisnika.Exists(fitn => korisnik.Uloga == "TRENER" && fitn.Id == korisnik.FitnesCentar.Id) && korisnik.Obrisan == false)
                {
                    lista_trenera.Add(korisnik);
                }
            }

            ViewData["lista_trenera"] = lista_trenera;
            return View();

        }

        public ActionResult BlokirajTrenera()
        {
            var trener = Request.Form["trener"];
            var korisnici = upis_citanje.UcitajKorisnike();
            korisnici.First(kor => kor.Korisnicko_ime == trener).Obrisan = true;

            upis_citanje.UpisiKorisnika(korisnici);

            return View("../PrijavaRegistracija/Registracija");
        }

        public ActionResult PogledajSvojeFitnesCentre()
        {
            var ulogovani = upis_citanje.UcitajKorisnike().First(ulog => ulog.Korisnicko_ime == ((Korisnik)Session["korisnik"]).Korisnicko_ime);
            ViewData["lista_fitnes_centara"] = ulogovani.FitnesCentri.Where(fitn => fitn.Obrisan == false).ToList();
            return View();
        }

        public ActionResult PogledajKomentare()
        {
            var ulogovani = upis_citanje.UcitajKorisnike().First(ulog => ulog.Korisnicko_ime == ((Korisnik)Session["korisnik"]).Korisnicko_ime);
            var komentariFitnesCentarVlasnik = upis_citanje.UcitajKomentare().Where(kom => ulogovani.FitnesCentri.Select(sel => sel.Id).ToList().Contains(kom.Fitnes_Centar_id)).ToList().Select(o => o.Fitnes_Centar_id);
            ViewData["komentariZaPrikaz"] = upis_citanje.UcitajKomentare().Where(kom => ulogovani.FitnesCentri.Count != 0 && komentariFitnesCentarVlasnik.Contains(kom.Fitnes_Centar_id)).ToList();
            return View();
        }

        public ActionResult VidljivostKomentara()
        {
            var id = Request["id"];
            var odabir = Request["odabir"];
            var komentari = upis_citanje.UcitajKomentare();
            if(odabir == "vid")
            {
               komentari.First(kom => kom.Id == id).Vidljiv = true;
            }
            else
            {
                komentari.First(kom => kom.Id == id).Vidljiv = false;
            }
            upis_citanje.UpisKomentara(komentari);
            return RedirectToAction("Opcije");
        }
           

    }
}