using Novych.Models;
using Novych.Models.Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Novych.Controllers
{
    public class CiterkaController : Controller
    {
        Citerka Citerka;
        NovychDbContext DB;
        List<CiterkaSong> Songs;
        CiterkaSong HolkaModrooka;

        public CiterkaController()
        {
            Citerka = new Citerka(1123, 794, false, false);
            DB = new NovychDbContext();
            Songs = DB.Songs.ToList();
            HolkaModrooka = Songs.Where(item => item.HeaderText == "Holka Modrooká").First();
        }

        public ActionResult Index()
        {
            NoteModel model = new NoteModel();

            model.Songs = Songs;

            if (HolkaModrooka != null)
            {
                model.HeaderString = HolkaModrooka.HeaderText;
                model.NotesString = HolkaModrooka.Notes;
                model.FooterString = HolkaModrooka.FooterText;

                List<Tone> song = model.getNotes();
                Image image = Citerka.drawSong(song, model.HeaderString, model.FooterString);

                model.setImage(image);
            }
            else
            {
                Image image = Citerka.clear();
                model.setImage(image);
            }

            //model.addLog(Citerka.logBuffer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(NoteModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<Tone> song = model.getNotes();
                    Image image = Citerka.drawSong(song, model.HeaderString, model.FooterString);
                    model.setImage(image);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("NotesString", e.Message);
                    Image image = Citerka.clear();
                    model.setImage(image);
                }

                Logger.log(LogClass.INFO, "Generated: [" + model.HeaderString + "] [" + model.NotesString + "] [" + model.FooterString + "]");
            }
            else
            {
                Image image = Citerka.clear();
                model.setImage(image);
            }

            model.Songs = Songs;
            model.addLog(Citerka.logBuffer);

            return View(model);
        }
    }
}