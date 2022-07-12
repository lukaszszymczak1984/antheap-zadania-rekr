using System.ComponentModel.DataAnnotations;

namespace NipSearch.Entities
{
    public class Person
    {
        public int IdPerson { get; set; }
        public int? IdSubjectRepresentative { get; set; }
        public Subject? SubjectRepresentative { get; set; }
        public int? IdSubjectAuthorizedClerk { get; set; }
        public Subject? SubjectAuthorizedClerk { get; set; }
        public int? IdSubjectPartner { get; set; }
        public Subject? SubjectPartner { get; set; }

        [Display(Name = "Nazwa firmy")]
        public string? CompanyName { get; set; }

        [Display(Name = "Imię")]
        public string? FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string? LastName { get; set; }

        [Display(Name = "PESEL")]
        public string? Pesel { get; set; }

        [Display(Name = "NIP")]
        public string? Nip { get; set; }
    }
}
