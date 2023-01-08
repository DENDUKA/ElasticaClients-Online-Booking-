using ElasticaClients.DAL.Migrations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace ElasticaClients.Models
{
	public static class ServisesHelper
	{
		public static int GetCost(string servisesList, int discount)
		{
			var servises = servisesList.Split(' ');
			string selectedComplex = null;
			var cost = 0;

			if (servises.Length > 0 && СomplexesName.ContainsKey(servises[0]))
			{
				selectedComplex = servises[0];
			}


			if (selectedComplex != null)
			{
				cost += СomplexesPrice[selectedComplex];
			}

			foreach (var s in servises)
			{
				if (ServisesName.ContainsKey(s))
				{
					if (selectedComplex != null && СomplexesInclude[selectedComplex].Contains(s))
					{ 
					}
					else
					{
						cost += ServisesPrice[s];
					}
				}
			}

			cost = cost * (100 - discount) / 100;

			return cost;
		}

		public static List<string> ServisesInComplexNames(string complexId)
		{
			if (СomplexesInclude.ContainsKey(complexId))
			{
				return СomplexesInclude[complexId].Select(x=> ServisesName[x]).ToList();				
			}

			return new List<string>();
		}

		public static List<string> ServisesIdNotInComplex(string servisesList)
		{
			var servises = servisesList.Split(' ');
			string selectedComplex = null;

			if (servises.Length > 0 && СomplexesName.ContainsKey(servises[0]))
			{
				selectedComplex = servises[0];
			}

			if (selectedComplex is null)
			{
				return servises.ToList();
			}

			var result = new List<string>();

			foreach (var s in servises)
			{
				if (ServisesName.ContainsKey(s) && !СomplexesInclude[selectedComplex].Contains(s))
				{
					result.Add(s);
				}
			}

			return result;

		}

		public static string GetServiserIdsString()
		{
			return $"\"{string.Join("\",\"", ServisesName.Keys)}\"";
		}

		public static string GetSelectedComplex(string servisesList)
		{
			var servises = servisesList.Split(' ');

			if (servises.Length > 0 && СomplexesName.ContainsKey(servises[0]))
			{
				return servises[0];
			}

			return null;
		}

		public static string GetComplexesIdsString()
		{
			return $"\"{string.Join("\",\"", СomplexesName.Keys)}\"";
		}

		public static Dictionary<string, int> СomplexesPrice =
			new Dictionary<string, int>
			{
				{"c1", 1600},
				{"c2", 2500},
				{"c3", 2600},
				{"c4", 3200},
				{"c5", 4300},
			};

		public static Dictionary<string, string> СomplexesName =
			new Dictionary<string, string>
			{
				{"c1", "Базовый" },
				{"c2", "MIDI" },
				{"c3", "Стандарт" },
				{"c4", "Premium" },
				{"c5", "Gold" },
			};

		public static Dictionary<string, List<string>> СomplexesInclude =
			new Dictionary<string, List<string>>
			{
				{"c1", new List<string>{"s8","s2" } },
				{"c2", new List<string>{"s8","s15" } },
				{"c3", new List<string>{"s8","s2", "s13" } },
				{"c4", new List<string>{"s8","s2", "s15" } },
				{"c5", new List<string>{"s8","s2", "s15", "s10" } },
			};

		public static Dictionary<string, string> ServisesName =
			new Dictionary<string, string>
			{
				{ "s1",  "Бикини классическое"},
				{ "s2",  "Бикини глубокое"},
				{ "s3",  "Ареолы"},
				{ "s4",  "Живот полностью"},
				{ "s5",  "Линия живота"},
				{ "s6",  "Спина полностью"},
				{ "s7",  "Поясница"},
				{ "s8",  "Подмышечные впадины"},
				{ "s9",  "Руки до локтей"},
				{ "s10",  "Руки полностью"},
				{ "s11",  "Кисти рук"},
				{ "s12",  "Пальцы рук"},
				{ "s13",  "Голени"},
				{ "s14",  "Бедра"},
				{ "s15",  "Ноги полностью"},
				{ "s16",  "Ягодицы"},
				{ "s17",  "Пальцы ног"},
				{ "s18",  "Верхняя губа(Усы)"},
				{ "s19",  "Бакенбарды"},
				{ "s20",  "Подбородок"},
				{ "s21",  "Скулы (Щеки)"},
				{ "s22",  "Лоб"},
				{ "s23",  "Шея полностью"},
				{ "s24",  "Лицо полностью"},
			};

		public static Dictionary<string, int> ServisesPrice =
			new Dictionary<string, int>
			{
				{ "s1",  1000},
				{ "s2", 1400},
				{ "s3",  500},
				{ "s4", 1100},
				{ "s5",  500 },
				{ "s6",   1500},
				{ "s7",  1000},
				{ "s8",  600},
				{ "s9",   900},
				{ "s10",  1400},
				{ "s11",  400},
				{ "s12",  300},
				{ "s13",  1000},
				{ "s14",  1600},
				{ "s15",  2200},
				{ "s16",  1000},
				{ "s17",  300},
				{ "s18",  400},
				{ "s19",  400},
				{ "s20",  400},
				{ "s21",  400},
				{ "s22",  400},
				{ "s23",  500},
				{ "s24",  1000},
			};
	}
}