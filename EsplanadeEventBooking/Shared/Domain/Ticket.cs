using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsplanadeEventBooking.Shared.Domain
{
    public class Ticket : BaseDomainModel
	{
		public int? RowNo { get; set; }
		public int? ColumnNo { get; set; }
		public bool VIP { get; set; }
		public int EuserId { get; set; }
		public virtual Euser Euser { get; set; }
		public int BookeventId { get; set; }
		public virtual Bookevent Bookevent { get; set; }

    }
}
