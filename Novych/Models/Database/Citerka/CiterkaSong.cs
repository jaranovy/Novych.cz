using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Novych.Models.Database
{
    [Table("Citerka_Songs")]
    public class CiterkaSong
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Název")]
        [DataType(DataType.Text)]
        public string HeaderText { get; set; }

        [Display(Name = "Noty")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Display(Name = "Popis")]
        [DataType(DataType.Text)]
        public string FooterText { get; set; }
    }
}