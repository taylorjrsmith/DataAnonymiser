using System;
using System.Collections.Generic;
using System.Text;

namespace DataAnonymiser
{
    public class MoverDisqualificationModel
    {
        public int? MoverId { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Domain { get; set; }
        public int? ServiceId { get; set; }
        public string NearestFrom { get; set; }
        public string NearestTo  { get; set; }
        public string IPAddress { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public bool? IsDisqualifiedMover { get; set; }
        public int RegisteredThreeInTwentyFourHours { get; set; }

    }
}
