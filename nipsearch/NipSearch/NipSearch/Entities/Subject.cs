using System.ComponentModel.DataAnnotations;

namespace NipSearch.Entities
{
    public class Subject
    {
        public int IdSubject { get; set; }

        [Display(Name= "Firma (nazwa) lub imię i nazwisko")]
        public string? Name { get; set; }

        [Display(Name = "NIP")]
        public string? Nip { get; set; }

        [Display(Name = "Status podatnika VAT")]
        public string? StatusVat { get; set; }

        [Display(Name = "Numer identyfikacyjny REGON")]
        public string? Regon { get; set; }

        [Display(Name = "PESEL")]
        public string? Pesel { get; set; }

        [Display(Name = "Numer KRS jeżeli został nadany")]
        public string? Krs { get; set; }

        [Display(Name = "Adres siedziby działalności gospodarczej (Adres siedziby OSOBY FIZYCZNEJ prowadzącej działalność gospodarczą)")]
        public string? ResidenceAddress { get; set; }

        [Display(Name = "Adres rejestracyjny (Adres zamieszkania OSOBY FIZYCZNEJ lub adres siedziby ORGANIZACJI.)")]
        public string? WorkingAddress { get; set; }

        [Display(Name = "Imiona i nazwiska osób wchodzących w skład organu uprawnionego do reprezentowania podmiotu oraz ich numery NIP i/lub PESEL")]
        //public Person[]? Representatives { get; set; }
        public virtual ICollection<Person> Representatives { get; set; }

        [Display(Name = "Imiona i nazwiska prokurentów oraz ich numery NIP i/lub PESEL")]
        //public Person[]? AuthorizedClerks { get; set; }
        public virtual ICollection<Person> AuthorizedClerks { get; set; }

        [Display(Name = "Imiona i nazwiska lub firmę (nazwa) wspólnika oraz jego numeryNIP i/lub PESEL")]
        //public Person[]? Partners { get; set; }
        public virtual ICollection<Person> Partners { get; set; }

        [Display(Name = "Data rejestracji jako podatnika VAT")]
        public string? RegistrationLegalDate { get; set; }

        [Display(Name = "Data odmowy rejestracji jako podatnika VAT")]
        public string? RegistrationDenialDate { get; set; }

        [Display(Name = "Podstawa prawna odmowy rejestracji")]
        public string? RegistrationDenialBasis { get; set; }

        [Display(Name = "Data przywrócenia jako podatnika VAT")]
        public string? RestorationDate { get; set; }

        [Display(Name = "Podstawa prawna przywrócenia jako podatnika VAT")]
        public string? RestorationBasis { get; set; }

        [Display(Name = "Data wykreślenia odmowy rejestracji jako podatnika VAT")]
        public string? RemovalDate { get; set; }

        [Display(Name = "Podstawa prawna wykreślenia odmowy rejestracji jako podatnika VAT")]
        public string? RemovalBasis { get; set; }

        [Display(Name = "Numery rachunków rozliczeniowych lub imiennych rachunków w SKOK")]
        //public string[]? AccountNumbers { get; set; }
        public virtual List<string> AccountNumbers { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public bool HasVirtualAccounts { get; set; }

        [Display(Name = "Podmiot posiada maski kont wirtualnych")]
        public string HasVirtualAccountsValue => HasVirtualAccounts ? "Tak" : "Nie";

        public Subject()
        {
            Representatives = new List<Person>();
            AuthorizedClerks = new List<Person>();
            Partners = new List<Person>();
            AccountNumbers = new List<string>();
            Accounts = new List<Account>();
        }

        public void CreateAccounts()
		{
            Accounts = AccountNumbers.Select(e => new Account() { Number = e }).ToList();
		}
    }
}
