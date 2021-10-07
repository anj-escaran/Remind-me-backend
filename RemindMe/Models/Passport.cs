using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.Models
{
    public class Passport
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string CellNumber { get; set; }
        public string PassportNumber { get; set; }
        public DateTime ValidUntil { get; set; }
        public int SpanPriorExpi{ get; set; }
        public string SpanCalendarType{ get; set; }
        public DateTime RemindDate { get; set; }
        public Boolean IsAlarm { get; set; }
    }
}
