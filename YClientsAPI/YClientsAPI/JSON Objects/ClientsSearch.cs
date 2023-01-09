using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YClientsAPI.JSON_Objects
{
    public class ClientsSearch
    {
        public int page { get; set; }
        public int page_size { get; set; }
        public string[] fields { get; set; }
        public string order_by { get; set; }
        public string order_by_direction { get; set; }
        public string operation { get; set; }
        public string[] filters { get; set; }

        public ClientsSearch(int page, int page_size)
        {
            this.page = page;
            this.page_size = page_size;
            fields = new string[] { "id" };
            order_by = "id";
            order_by_direction = "ASC";
            operation = "AND";
            filters = new string[0];
        }


        /*
		page	
		number
		номер страницы

		page_size	
		number
		Default: 25
		количество выводимых строк на странице. Максимум 200

		fields	
		Array of any
		Default: ["id"]

		order_by	
		string
		Default: "id"
		Enum: "id" "name" "phone" "email" "discount" "first_visit_date" "last_visit_date" "sold_amount" "visits_count"
		по какому полю сортировать

		order_by_direction	
		string
		Default: "ASC"
		Enum: "DESC" "ASC"
		как сортировать

		operation	
		string
		Default: "AND"
		Enum: "AND" "OR"
		тип операции

		filters	
		Array of any
		фильтры для поиска по клиентам*/
    }
}
