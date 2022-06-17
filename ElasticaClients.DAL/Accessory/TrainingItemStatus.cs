using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticaClients.DAL.Accessory
{
	public enum TrainingItemStatus
	{
		yes = 0,
		unKnown = 1,
		no = 2,
		canceledByAdmin = 4,
	}
}