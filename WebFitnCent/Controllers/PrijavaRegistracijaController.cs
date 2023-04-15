using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFitnCent.Models;

namespace WebFitnCent.Controllers
{
    public class PrijavaRegistracijaController : Controller
    {
        UCPomocna upis_citanje = new UCPomocna();
        // GET: PrijavaRegistracija
        public ActionResult Prijava()
        {
            return View();
        }

        public ActionResult Registracija()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistrujSe()
        {
            if(ProveraKorisnickogImena(Request.Form["kime"]))
            {
                ViewData["Greska"] = "Korisnicko ime vec postoji";
                return View("Registracija");
            }

            Korisnik k = new Korisnik()
            {
                Korisnicko_ime = Request.Form["kime"],
                Lozinka = Request.Form["sifra"],
                Ime = Request.Form["ime"],
                Prezime = Request.Form["prezime"],
                Pol = Request.Form["pol"],
                Email = Request.Form["email"],
                Datum = Request.Form["datum"],
                Uloga = Request.Form["uloga"],
                FitnesCentri = new List<FitnesCentar>(),
                Spisak_treninga = new List<GrupniTrening>()

            };

            if(Request.Form["id_fitnes_centra"] != null)
            {
                k.FitnesCentar = upis_citanje.UcitajFitnesCentre().First(fitn => fitn.Id == Request.Form["id_fitnes_centra"]);
            }

            var korisnici = upis_citanje.UcitajKorisnike();
            korisnici.Add(k);
            upis_citanje.UpisiKorisnika(korisnici);

            return View("Prijava");
        }

        private bool ProveraKorisnickogImena(string kime)
        {
            if(!upis_citanje.UcitajKorisnike().Exists(x=> x.Korisnicko_ime == kime))
            {
                return false;
            }
            return true;
        }
        
        [HttpPost]
        public ActionResult PrijaviSe()
        {
            if(upis_citanje.UcitajKorisnike().Exists(x=> x.Obrisan == false && x.Korisnicko_ime == Request.Form["kime"] && x.Lozinka == Request.Form["sifra"]) == false )
            {
                ViewData["Greska"] = "Pogresno uneseno Korisnicko ime ili Lozinka";
                return View("Prijava");
            }

            Session["korisnik"] = upis_citanje.UcitajKorisnike().First(x => x.Korisnicko_ime == Request.Form["kime"]);

            return RedirectToAction("SviFitnesCentri", "Neprijavljeni");

        }

        
    }
}