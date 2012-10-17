using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Model
{
    public class CustomerLetter
    {
            public int Id { get; set; }
            public string LetterCity { get; set; }
            public string LetterCounty { get; set; }
            public string LetterCountry { get; set; }
            public string LetterStreet { get; set; }
            public string LetterZipCode { get; set; }
    }
}
