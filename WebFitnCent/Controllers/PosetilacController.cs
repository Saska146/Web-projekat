using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebFitnCent.Models;

namespace WebFitnCent.Controllers
{
    public class PosetilacController : Controller
    {
        UCPomocna upis_citanje = new UCPomocna();

        // GET: Posetilac
        public ActionResult Opcije()
        {
            return View();
        }

        public ActionResult SviFitnesCentri()
        {
            ViewData["fitnesCentriZaPrikaz"] = upis_citanje.UcitajFitnesCentre().Where(fitn => fitn.Obrisan == false).ToList();
            return View();
        }

        public ActionResult DetaljnijiPrikaz()
        {
            var korisnickoImeUlogovanog = ((Korisnik)Session["korisnik"]).Korisnicko_ime;
            var id = Request["id"];
            ViewData["OdabraniFitnesCentar"] = upis_citanje.UcitajFitnesCentre().First(fitn => fitn.Id == id);
            ViewData["GrupniTreninziZaPrikaz"] = upis_citanje.UcitajGrupneTreninge().Where(grup => grup.Fitnes_centar == id && grup.Obrisan == false && DateTime.Parse(grup.Datum_i_vreme_treninga) > DateTime.Now && grup.Spisak_posetilaca.Count < grup.Maksimalan_broj_posetilaca && !grup.Spisak_posetilaca.Contains(korisnickoImeUlogovanog)).ToList();
            return View();

        }

        public ActionResult Prijava()
        {
            var id = Request["id"];

            var korisnickoImeUlogovanog = ((Korisnik)Session["korisnik"]).Korisnicko_ime;
            var korisnici = upis_citanje.UcitajKorisnike();
            var ucitaniTreninzi = upis_citanje.UcitajGrupneTreninge();
            var odredjenTrening = ucitaniTreninzi.First(odr => odr.Id == id);
            odredjenTrening.Spisak_posetilaca.Add(korisnickoImeUlogovanog);

            var korisnik = korisnici.First(kor => kor.Korisnicko_ime == korisnickoImeUlogovanog);
            korisnik.Spisak_treninga.Add(odredjenTrening);

            korisnici.ForEach(kor => { if (kor.Spisak_treninga.FirstOrDefault(tr => tr.Id == odredjenTrening.Id) != null && kor.Uloga == "TRENER" && !kor.Spisak_treninga.First(tr => tr.Id == odredjenTrening.Id).Spisak_posetilaca.Contains(korisnickoImeUlogovanog)) { kor.Spisak_treninga.First(tr => tr.Id == odredjenTrening.Id).Spisak_posetilaca.Add(korisnickoImeUlogovanog); } });
            


            upis_citanje.UpisGrupnihTreninga(ucitaniTreninzi);
            upis_citanje.UpisiKorisnika(korisnici);

            return RedirectToAction("SviFitnesCentri");
        }

        public ActionResult RanijiTreninzi()
        {
            var k = (Korisnik)Session["korisnik"];
            ViewData["GrupniTreninziZaPrikaz"] = k.Spisak_treninga.Where(kr => DateTime.Parse(kr.Datum_i_vreme_treninga) < DateTime.Now).ToList();
            return View();
        }

        public ActionResult Sortiraj()
        {
            var ulogovani = upis_citanje.UcitajKorisnike().First(ulog => ulog.Korisnicko_ime == ((Korisnik)Session["korisnik"]).Korisnicko_ime);
            var sortLista = ulogovani.Spisak_treninga.Where(kr => DateTime.Parse(kr.Datum_i_vreme_treninga) < DateTime.Now).ToList();

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

            ViewData["GrupniTreninziZaPrikaz"] = sortLista;
            return View("RanijiTreninzi");
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
            if (polja_za_pretragu.ContainsKey("nzv_fc"))
            {
                pretragaLista = pretragaLista.Where(pret => upis_citanje.UcitajFitnesCentre().First(fcnt => fcnt.Id == pret.Fitnes_centar).Naziv == polja_za_pretragu["nzv_fc"]).ToList();
            }


            ViewData["GrupniTreninziZaPrikaz"] = pretragaLista;

            return View("RanijiTreninzi");
        }

        public Dictionary<string, string> ProveraPoljaZaPretragu()
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
            if (Request.Form["nzv_fc"] != "")
            {
                polja_za_pretragu["nzv_fc"] = Request["nzv_fc"];
            }
           

            return polja_za_pretragu;
        }

        public ActionResult Kreiraj()
        {
            ViewData["id"] = Request["id"];
            return View();
        }

        public ActionResult Komentarisi()
        {
            var id_grupnog = Request["id"];
            var korisnickoImeUlogovanog = ((Korisnik)Session["korisnik"]).Korisnicko_ime;
            var id_fitnes_centra = upis_citanje.UcitajGrupneTreninge().First(grup => grup.Id == id_grupnog).Fitnes_centar;
            StringBuilder id = new StringBuilder();
            Enumerable
                .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(12)
                .ToList().ForEach(e => id.Append(e));

            Komentar kom = new Komentar();

            kom.Id = id.ToString();
            kom.Posetilac_korisnicko = korisnickoImeUlogovanog;
            kom.Fitnes_Centar_id = id_fitnes_centra;
            kom.Tekst_komentara = Request["tks"];
            kom.Ocena = Int32.Parse(Request["ocn"]);
            kom.Vidljiv = false;

            var komentari = upis_citanje.UcitajKomentare();
            komentari.Add(kom);

            upis_citanje.UpisKomentara(komentari);

            return RedirectToAction("RanijiTreninzi");
        }

        
    }
}