using Novych.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Novych.Models
{
    public class NoteModel
    {
        [Display(Name = "Název: ")]
        [StringLength(20, MinimumLength = 0, ErrorMessage = "Překročena maximální délka Názvu: 20 znaků")]
        public String HeaderString { get; set; }

        [Display(Name = "Noty: ")]
        [DataType(DataType.MultilineText)]
        [StringLength(300, MinimumLength = 0, ErrorMessage = "Překročena maximální délka Not: 300 znaků")]
        public String NotesString { get; set; }

        [Display(Name = "Popis: ")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Překročena maximální délka Popisu: 50 znaků")]
        public String FooterString { get; set; }

        public List<Tone> Notes { get; set; }

        public String Log { get; set; }

        public Image Image { get; set; }

        public String ImageData { get; set; }
        public String ImageDataURL { get; set; }

        public List<CiterkaSong> Songs { get; set; }

        public void log(String msg)
        {
            Log += DateTime.Now.ToString("[hh:mm:ss.fff]") + " [NoteModel] " + msg + "\n";
        }

        public void addLog(string log)
        {
            this.Log += log;
        }

        public void setImage(Image image)
        {
            log("setImage");

            Image = image;

            convertImage();
        }

        private void convertImage()
        {
            log("convertImage");

            if (Image != null)
            {
                ImageConverter _imageConverter = new ImageConverter();
                byte[] imageByteData = (byte[])_imageConverter.ConvertTo(Image, typeof(byte[]));

                ImageDataURL = "data:image/png;base64," + Convert.ToBase64String(imageByteData);
            }
        }

        public List<Tone> getNotes()
        {
            log("getNotes");

            char[] delimiters = { ';', ',' };
            Notes = new List<Tone>();

            if (NotesString == null || NotesString.Length <= 0)
            {
                log("NotesString is empty");

                throw new Exception("Zadej nějaké noty");
            }

            foreach (string noteString in NotesString.Replace("+", "_").ToUpper().Split(delimiters))
            {
                try
                {
                    string noteStr = noteString.Trim();

                    if (noteStr.Length == 0)
                    {
                        continue;
                    }

                    Tone tone = Citerka.allToneList.Where(item => item.ToString().Equals(noteStr)).First();
                    Notes.Add(tone);
                }
                catch (Exception)
                {
                    log("Undefined note [" + noteString.Trim() + "]");
                    throw new Exception("Nerozpoznaná nota [" + noteString.Trim() + "]");
                }
            }

            return Notes;
        }

    }
}