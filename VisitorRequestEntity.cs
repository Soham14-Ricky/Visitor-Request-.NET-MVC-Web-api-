using System;
using System.Collections.Generic;
using System.Text;

namespace VisitorWebAPI.Core.Entities
{
    public class VisitorRequestEntity
    {
        public int RequestId { get; set; }

        public string VisitorName { get; set; }

        public string MobileNumber { get; set; }

        public string CompanyName { get; set; }

        public string PersonToMeet { get; set; }

        public string PurposeOfVisit { get; set; }

        public DateTime VisitDate { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
