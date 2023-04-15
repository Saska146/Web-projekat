using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFitnCent.Models;

namespace WebFitnCent.Controllers
{
    public class NeprijavljeniController : Controller
    {
        UCPomocna upis_citanje = new UCPomocna();

        // GET: Neprijavljeni
        public ActionResult Opcije()
        {
            if(Session["korisnik"] != null)
            {
                ViewData["uloga"] = ((Korisnik)Session["korisnik"]).Uloga;
            }
            
            return View();
        }

        public ActionResult SviFitnesCentri()
        {
            var obicnaLista = upis_citanje.UcitajFitnesCentre().Where(fitn => fitn.Obrisan == false).ToList();
            obicnaLista.Sort((a, b) => a.Naziv.CompareTo(b.Naziv));
            ViewData["fitnesCentriZaPrikaz"] = obicnaLista;
            return View();

        }

        public ActionResult Sortiraj()
        {
            var sortLista = upis_citanje.UcitajFitnesCentre().Where(fitn => fitn.Obrisan == false).ToList();

            var sta = Request["sta"];
            var vrsta = Request["vrsta"];

            if (sta == "Nazivu" && vrsta == "Rastuce")
            {
                sortLista.Sort((a, b) => a.Naziv.CompareTo(b.Naziv));
            }

            if (sta == "Nazivu" && vrsta == "Opadajuce")
            {
                sortLista.Sort((a, b) => b.Naziv.CompareTo(a.Naziv));
            }

            if (sta == "Adresi" && vrsta == "Rastuce")
            {
                sortLista.Sort((a, b) => a.Adresa.CompareTo(b.Adresa));
            }

            if (sta == "Adresi" && vrsta == "Opadajuce")
            {
                sortLista.Sort((a, b) => b.Adresa.CompareTo(a.Adresa));
            }

            if (sta == "Godini" && vrsta == "Rastuce")
            {
                sortLista.Sort((a, b) => a.Godina_otvaranja.CompareTo(b.Godina_otvaranja));
            }

            if (sta == "Godini" && vrsta == "Opadajuce")
            {
                sortLista.Sort((a, b) => b.Godina_otvaranja.CompareTo(a.Godina_otvaranja));
            }

            ViewData["fitnesCentriZaPrikaz"] = sortLista;
            return View("SviFitnesCentri");
        }

        public ActionResult Pretrazi()
        {
            Dictionary<string, string> polja_za_pretragu = new Dictionary<string, string>();
            var pretragaLista = upis_citanje.UcitajFitnesCentre().Where(fitn => fitn.Obrisan == false).ToList();

            polja_za_pretragu = ProveraPoljaZaPretragu();

            if (polja_za_pretragu.ContainsKey("nzv"))
            {
                pretragaLista = pretragaLista.Where(pret => pret.Naziv == polja_za_pretragu["nzv"]).ToList();
            }
            if (polja_za_pretragu.ContainsKey("adr"))
            {
                pretragaLista = pretragaLista.Where(pret => pret.Adresa == polja_za_pretragu["adr"]).ToList();
            }
            if (polja_za_pretragu.ContainsKey("godinaminimum"))
            {
                pretragaLista = pretragaLista.Where(pret => DateTime.Parse(pret.Godina_otvaranja).Year >= Int32.Parse(polja_za_pretragu["godinaminimum"])).ToList();
            }
            if (polja_za_pretragu.ContainsKey("godinamaksimum"))
            {
                pretragaLista = pretragaLista.Where(pret => DateTime.Parse(pret.Godina_otvaranja).Year <= Int32.Parse(polja_za_pretragu["godinamaksimum"])).ToList();
            }

            ViewData["fitnesCentriZaPrikaz"] = pretragaLista;

            return View("SviFitnesCentri");
        }

        public Dictionary<string, string> ProveraPoljaZaPretragu()
        {
            Dictionary<string, string> polja_za_pretragu = new Dictionary<string, string>();

            if (Request.Form["nzv"] != "")
            {
                polja_za_pretragu["nzv"] = Request["nzv"];
            }
            if (Request.Form["adr"] != "")
            {
                polja_za_pretragu["adr"] = Request["adr"];
            }
            if (Request.Form["godinaminimum"] != "")
            {
                polja_za_pretragu["godinaminimum"] = Request["godinaminimum"];
            }
            if (Request.Form["godinamaksimum"] != "")
            {
                polja_za_pretragu["godinamaksimum"] = Request["godinamaksimum"];
            }

            return polja_za_pretragu;
        }

        public ActionResult DetaljnijiPrikaz()
        {
            var id = Request["id"];
            ViewData["OdabraniFitnesCentar"] = upis_citanje.UcitajFitnesCentre().First(fitn => fitn.Id == id);
            ViewData["GrupniTreninziZaPrikaz"] = upis_citanje.UcitajGrupneTreninge().Where(grup => grup.Fitnes_centar == id && grup.Obrisan == false && DateTime.Parse(grup.Datum_i_vreme_treninga) > DateTime.Now).ToList();
            ViewData["KomentariZaPrikaz"] = upis_citanje.UcitajKomentare().Where(kom => kom.Fitnes_Centar_id == id && kom.Vidljiv == true ).ToList();

            return View();

        }
    }
}