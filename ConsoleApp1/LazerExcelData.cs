using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	internal class LazerExcelData
	{
		public DateTime Data { get; internal set; }
		public string Phone { get; internal set; }
		public int FullFoot { get; internal set; }
		public int HandFull { get; internal set; }
		public int Bedra { get; internal set; }
		public int Face { get; internal set; }
		public int BikiniFull { get; internal set; }
		public int Podm { get; internal set; }
		public int Areol { get; internal set; }
		public int Guba { get; internal set; }
		public int BikiniClass { get; internal set; }
		public int Goleni { get; internal set; }

		public override string ToString()
		{
			return $"{Data} {Phone} {Podm} {FullFoot} {Goleni} {BikiniFull} {BikiniClass} {HandFull} {Bedra} {Areol} {Face} {Guba}";
		}
	}
}
