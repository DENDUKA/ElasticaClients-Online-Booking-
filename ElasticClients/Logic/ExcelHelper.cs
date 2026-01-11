using System;
using System.Linq;

namespace ElasticaClients.Logic
{
    public class ExcelHelper
    {
        private readonly TrainingB _trainingB;

        public ExcelHelper(TrainingB trainingB)
        {
            _trainingB = trainingB;
        }

        internal string ExcelStats()
        {
            var ObjExcel = new Microsoft.Office.Interop.Excel.Application();
            var ObjWorkBook = ObjExcel.Workbooks.Add(System.Reflection.Missing.Value);
            var ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ObjWorkBook.Sheets[1];

            //ObjWorkSheet.Columns[2].ColumnWidth = 4;

            ObjWorkSheet.Cells[2, 1] = "Дата";
            ObjWorkSheet.Cells[3, 1] = "Кол-во посетителей (Солнечный)";
            ObjWorkSheet.Cells[4, 1] = "Кол-во новичков (Центр)";
            ObjWorkSheet.Cells[5, 1] = "Кол-во посетителей (Солнечный)";
            ObjWorkSheet.Cells[6, 1] = "Кол-во новичков (Центр)";

            var sunTrainings = _trainingB.GetAllForGym(11).OrderBy(x => x.StartTime).ToList();
            var centerTrainings = _trainingB.GetAllForGym(3).OrderBy(x => x.StartTime).ToList();

            int dateCell = 2;

            DateTime dt = new DateTime(2021, 11, 01);


            for (int i = 0; i < sunTrainings.Count; i++)
            {
                sunTrainings[i] = _trainingB.Get(sunTrainings[i].Id);
            }

            for (int i = 0; i < centerTrainings.Count; i++)
            {
                centerTrainings[i] = _trainingB.Get(centerTrainings[i].Id);
            }

            while (dt <= DateTime.Today)
            {
                ObjWorkSheet.Cells[2, dateCell] = dt.ToString();

                var sunTs = sunTrainings.Where(x => x.StartTime.Year == dt.Year && x.StartTime.Month == dt.Month && x.StartTime.Day == dt.Day).ToList();
                var cenTs = centerTrainings.Where(x => x.StartTime.Year == dt.Year && x.StartTime.Month == dt.Month && x.StartTime.Day == dt.Day).ToList();



                ObjWorkSheet.Cells[3, dateCell] = sunTs.Sum(x => x.SeatsTaken);

                var xxxx = sunTs.Select(x => x.TrainingItems.Where(y => y.IsTrial)).ToList();
                ObjWorkSheet.Cells[4, dateCell] = sunTs.Select(x => x.TrainingItems.Where(y => y.IsTrial)).Count();

                ObjWorkSheet.Cells[5, dateCell] = cenTs.Sum(x => x.SeatsTaken);
                ObjWorkSheet.Cells[6, dateCell] = cenTs.Select(x => x.TrainingItems.Where(y => y.IsTrial)).Count();

                dt = dt.AddDays(1);
                dateCell++;
            }

            var filePath = $"C:\\Server\\data\\Excel\\{Guid.NewGuid()}.xlsx";

            ObjWorkBook.SaveAs(filePath);

            ObjWorkBook.Close();

            return filePath;
        }
    }
}
