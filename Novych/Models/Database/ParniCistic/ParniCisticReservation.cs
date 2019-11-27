using Novych.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Novych.Models.Database
{
    [Table("ParniCistic_Reservations")]
    public class ParniCisticReservation
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(NovychResources), Name = "DbDate")]
        [Required(ErrorMessageResourceType = typeof(NovychResources), ErrorMessageResourceName = "DbDateValidation")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbEmail")]
        [Required(ErrorMessageResourceType = typeof(NovychResources), ErrorMessageResourceName = "DbEmailValidation")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbPhone")]
        [Required(ErrorMessageResourceType = typeof(NovychResources), ErrorMessageResourceName = "DbPhoneValidation")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbName")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbAddress")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbDateCreated")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss.fff}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbDesc")]
        [DataType(DataType.MultilineText)]
        public string CreateDesc { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbDateDeleted")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss.fff}", ApplyFormatInEditMode = true)]
        public DateTime? DeleteDate { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbDesc")]
        [DataType(DataType.MultilineText)]
        public string DeleteDesc { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbIpAddress")]
        public string IpAddress { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbIdentifier")]
        public string Identifier { get; set; }
    }
}