using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Novych.Models.Database
{
    [Table("Log")]
    public class Log
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Datum")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss.fff}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Trida")]
        public string Class { get; set; }

        [Display(Name = "Zprava")]
        public string Message { get; set; }
    }
}