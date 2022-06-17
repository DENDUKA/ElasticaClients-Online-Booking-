using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	public class ExcelFormat
	{
		public string name;
		public List<DateTime> trainingsDates = new List<DateTime>();
		public List<DateTime> notrainingsDates = new List<DateTime>();
		public DateTime? DateStart;
		public DateTime? DateEnd;
		public string Tel;
		public string inst;
		internal int trainingCount;
	}
}
