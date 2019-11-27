using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Novych.Models.Database
{
    [Table("Visitors")]
    public class Visitor
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Datum")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss.fff}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "IP Adresa")]
        public string IpAddress { get; set; }

        [Display(Name = "Identifikator")]
        public string Identifier { get; set; }
    }
}