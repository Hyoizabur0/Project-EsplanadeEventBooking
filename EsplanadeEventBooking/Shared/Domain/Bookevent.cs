using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EsplanadeEventBooking.Shared.Domain
{
    public class Bookevent : BaseDomainModel, IValidatableObject
    {
		public string Title { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Location { get; set; }
		//public Blob Thumbnail { get; set; }
		public int CreatorId { get; set; }
		public virtual Creator Creator { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //throw new NotImplementedException();
            if (EndDate != null)
            {
                if (EndDate < StartDate)
                {
                    yield return new ValidationResult("End Date must not be lesser than Start Date", new[] { "EndDate" });
                }
            }
        } 
    }
}
