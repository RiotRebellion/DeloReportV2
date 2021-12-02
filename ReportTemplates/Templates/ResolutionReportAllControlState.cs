using Excel = Microsoft.Office.Interop.Excel;
using Delo.DAL.Entities;
using ReportTemplates.Templates.Base;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.ObjectModel;
using System;

namespace ReportTemplates.Templates
{
    public class ResolutionReportAllControlState : Template
    {   
        public string Name { get; set; } = "Отчёт по поручениям (все)";

        public override void Outputing(ObservableCollection<Person> personCollection, DateTime firstDate, DateTime lastDate)
        {
            DataSet dataSet = new DataSet();
            string connectionString = "Data Source=172.27.100.56;Initial Catalog=DELO_DB;Persist Security Info=True;User ID=luda;Password=Mm2O6N#dE7";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDateTime sqlFirstDate = firstDate;
                SqlDateTime sqlLastDate = lastDate;

                foreach (var person in personCollection)
                {
                    SqlString sqlId = person.Id;
                    string query =
                    $@"USE DELO_DB

					DECLARE @PERSON_ID NVARCHAR(64)
					DECLARE @FIRST_DATE DATETIME
					DECLARE @LAST_DATE DATETIME
					SET @PERSON_ID = '{sqlId}'
					SET @FIRST_DATE = '{sqlFirstDate}'
					SET @LAST_DATE = '{sqlLastDate}'
					
					SELECT
					/*Имя*/
					PERSON_NAME = (SELECT CLASSIF_NAME FROM DEPARTMENT WHERE DUE = @PERSON_ID),
						
					/*Все*/
					ALL_COUNT = (SELECT COUNT(*) AS 'ALL' FROM RESOLUTION
								FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
								WHERE RESOLUTION.RESOLUTION_DATE >= @FIRST_DATE
								AND REPLY.DUE = @PERSON_ID), 

					/*Исполнено*/
					EXECUTED_COUNT = (SELECT COUNT(*) AS 'EXECUTED' FROM RESOLUTION
										FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
										  where REPLY.DUE = @PERSON_ID 
										  AND RESOLUTION.RESOLUTION_DATE BETWEEN @FIRST_DATE and @LAST_DATE
										  AND	(REPLY.UPD_DATE IS NOT NULL
												 AND (RESOLUTION.PLAN_DATE IS NULL 
												 OR CONVERT(VARCHAR, reply.UPD_DATE, 102) <= CONVERT(VARCHAR, RESOLUTION.PLAN_DATE, 102))		 
												 )),

                    /*исполнены с измененной датой*/
                    EXECUTED_WITH_CHANGED_DATE = (SELECT COUNT(*) from RESOLUTION
							                    FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
							                    FULL JOIN DOC_RC ON RESOLUTION.ISN_REF_DOC = DOC_RC.ISN_DOC
							                    FULL JOIN DEPARTMENT ON REPLY.DUE = DEPARTMENT.DUE
							                      where REPLY.DUE = @PERSON_ID 
							                      AND RESOLUTION.RESOLUTION_DATE BETWEEN @FIRST_DATE AND @LAST_DATE
							                      AND ( REPLY.UPD_DATE IS NOT NULL 
									                    AND CONVERT(VARCHAR, REPLY.UPD_DATE, 102) > CONVERT(varchar, RESOLUTION.PLAN_DATE, 102)
									                    AND CONVERT(VARCHAR, REPLY.REPLY_DATE, 102) != CONVERT(VARCHAR, REPLY.UPD_DATE, 102))),

					/*ОТВЕТЫ С ОПОЗДАНИЕМ*/ 
					EXECUTED_LATE_COUNT = (SELECT COUNT(*) AS 'EXECUTED_LATE' FROM RESOLUTION
											FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
											  where REPLY.DUE = @PERSON_ID 
											  AND RESOLUTION.RESOLUTION_DATE BETWEEN @FIRST_DATE AND @LAST_DATE
											  AND ( REPLY.UPD_DATE IS NOT NULL 
													AND RESOLUTION.PLAN_DATE IS NOT NULL 
													AND CONVERT(VARCHAR, REPLY.UPD_DATE, 102) > CONVERT(varchar, RESOLUTION.PLAN_DATE, 102))),

					/*НЕ ИСПОЛНЕНО*/
					NOT_EXECUTED_COUNT = (SELECT COUNT(*) AS 'NOT_EXECUTED' FROM RESOLUTION
											FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
											  where REPLY.DUE = @PERSON_ID 
											  AND RESOLUTION.RESOLUTION_DATE BETWEEN @FIRST_DATE AND @LAST_DATE
											  AND REPLY.UPD_DATE IS NULL
											  AND (RESOLUTION.PLAN_DATE IS NULL OR RESOLUTION.PLAN_DATE < GETDATE())),

					/*СРОК НЕ НАСТУПИЛ*/
					DEADLINE_IS_NOT_COUNT = (SELECT COUNT(*) AS 'DEADLINE_IS_NOT' FROM RESOLUTION
											FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
											  where REPLY.DUE = @PERSON_ID 
											  AND RESOLUTION.RESOLUTION_DATE BETWEEN @FIRST_DATE AND @LAST_DATE
											  AND REPLY.UPD_DATE IS NULL
											  AND ( RESOLUTION.PLAN_DATE IS NOT NULL 
													AND RESOLUTION.PLAN_DATE >= GETDATE()))";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(dataSet, "REPORT");
                }
                connection.Close();
            }

            int previousRow;
            int row = 1;
            Excel.Range c1;
            Excel.Range c2;
            Excel.Range upperRow;
            Excel.Range bottomRow;
            Excel.Range range;

            var excelApp = new Excel.Application
            {
                Visible = true
            };
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;

            //Высота столбцов
            workSheet.Rows["1, 1"].RowHeight = 50.00;

            //ширина ячеек  
            workSheet.Columns["A:A"].ColumnWidth = 45.00;
            workSheet.Columns["B:B"].ColumnWidth = 12.00;
            workSheet.Columns["C:C"].ColumnWidth = 14.00;
            workSheet.Columns["D:D"].ColumnWidth = 14.00;
            workSheet.Columns["E:E"].ColumnWidth = 12.00;
            workSheet.Columns["F:F"].ColumnWidth = 12.00;
            workSheet.Columns["G:G"].ColumnWidth = 14.00;

            //заголовок
            workSheet.Cells[1, 1] = $@"{this.Name}  
      {firstDate.ToString("dd MMMM yyyy")} - {lastDate.ToString("dd MMMM yyyy")}";

            //Перенос на другую строку
            c1 = workSheet.Cells[row, 1];
            c2 = workSheet.Cells[row, 7];
            range = workSheet.get_Range(c1, c2);
            range.WrapText = true;
            range.Merge();

            //Берем верхнюю границу выгруженных данных
            upperRow = workSheet.Cells[1, 1];

            //далее берем все данные одной строки
            var result = from output in dataSet.Tables["REPORT"].AsEnumerable()
                            select new
                            {
                                personName = output.Field<string>("PERSON_NAME"),
                                allCount = output.Field<int>("ALL_COUNT"),
                                executedCount = output.Field<int>("EXECUTED_COUNT"),
                                executedWithChangedDateCount = output.Field<int>("EXECUTED_WITH_CHANGED_DATE"),
                                executedLateCount = output.Field<int>("EXECUTED_LATE_COUNT"),
                                notExecutedCount = output.Field<int>("NOT_EXECUTED_COUNT"),
                                deadlineIsNotCount = output.Field<int>("DEADLINE_IS_NOT_COUNT")
                            };
            //сохраняем первую строку с номенклатурами отдела
            previousRow = row + 1;

            row++;
            workSheet.Cells[row, 1] = "Сотрудник - должность";
            workSheet.Cells[row, 2] = "Всего поручений";
            workSheet.Cells[row, 3] = "Исполнено поручений";
            workSheet.Cells[row, 4] = "Исполнено с изменением даты";
            workSheet.Cells[row, 5] = "Исполнено с опозданием";
            workSheet.Cells[row, 6] = "Не исполнено";
            workSheet.Cells[row, 7] = "Срок не наступил";

            //заполняем содержание
            var j = 0;
            foreach (var info in result)
            {
                row++;
                workSheet.Cells[row, 1] = (string)info.personName;                    
                workSheet.Cells[row, 2] = (int)info.allCount;
                workSheet.Cells[row, 3] = (int)info.executedCount;
                workSheet.Cells[row, 4] = (int)info.executedWithChangedDateCount;
                workSheet.Cells[row, 5] = (int)info.executedLateCount - (int)info.executedWithChangedDateCount;
                workSheet.Cells[row, 6] = (int)info.notExecutedCount;
                workSheet.Cells[row, 7] = (int)info.deadlineIsNotCount;
            }

            bottomRow = workSheet.Cells[row, 7];
            range = workSheet.get_Range(upperRow, bottomRow);
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            range.WrapText = true;
            range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders.get_Item(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders.get_Item(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
            
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
