using Excel = Microsoft.Office.Interop.Excel;
using Delo.DAL.Entities;
using ReportTemplates.Templates.Base;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System;
using System.Data.SqlTypes;

namespace ReportTemplates.Templates
{
    public class ResolutionReportOnControlStateDetailed : Template
    {
        public string Name { get; set; } = "Подробный отчёт по поручениям (контролируемые)";

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
                    string personQuery =
                    $@"
                    USE DELO_DB
					/*Имя*/
					SELECT CLASSIF_NAME FROM DEPARTMENT WHERE DUE = '{sqlId}'";


                    string allDocsQuery =
                    $@"
                    USE DELO_DB
                    /*Все*/
                    SELECT DEPARTMENT.CLASSIF_NAME AS 'PERSON', DOC_RC.FREE_NUM AS 'DOCNUM', RESOLUTION.RESOLUTION_TEXT AS 'RESOLUTION_TEXT', RESOLUTION.PLAN_DATE AS 'PLAN', REPLY.REPLY_TEXT AS 'REPLY_TEXT', REPLY.REPLY_DATE AS 'WRITED_DATE', REPLY.UPD_DATE AS 'FACT_DATE' FROM RESOLUTION
                    FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
                    FULL JOIN DOC_RC ON RESOLUTION.ISN_REF_DOC = DOC_RC.ISN_DOC
                    FULL JOIN DEPARTMENT ON REPLY.DUE = DEPARTMENT.DUE
                    WHERE RESOLUTION.RESOLUTION_DATE BETWEEN '{sqlFirstDate}' AND '{sqlLastDate}'
                    AND (RESOLUTION.CONTROL_STATE = 1 or RESOLUTION.CONTROL_STATE = 2)
                    AND REPLY.DUE = '{sqlId}'";

                    string executedDocsQuery =
                    $@"
                    USE DELO_DB
                    /*Исполнено*/
                    SELECT DEPARTMENT.CLASSIF_NAME AS 'PERSON', DOC_RC.FREE_NUM AS 'DOCNUM', RESOLUTION.RESOLUTION_TEXT AS 'RESOLUTION_TEXT', RESOLUTION.PLAN_DATE AS 'PLAN', REPLY.REPLY_TEXT AS 'REPLY_TEXT', REPLY.REPLY_DATE AS 'WRITED_DATE', REPLY.UPD_DATE AS 'FACT_DATE' FROM RESOLUTION
                    FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
                    FULL JOIN DOC_RC ON RESOLUTION.ISN_REF_DOC = DOC_RC.ISN_DOC
                    FULL JOIN DEPARTMENT ON REPLY.DUE = DEPARTMENT.DUE
                      where REPLY.DUE = '{sqlId}' 
                      AND RESOLUTION.RESOLUTION_DATE BETWEEN '{sqlFirstDate}' AND '{sqlLastDate}'
                      AND (RESOLUTION.CONTROL_STATE = 1 or RESOLUTION.CONTROL_STATE = 2)
                      AND	(reply.UPD_DATE is not null
		                     and (RESOLUTION.PLAN_DATE is null OR CONVERT(VARCHAR, reply.UPD_DATE, 102) <= CONVERT(VARCHAR, RESOLUTION.PLAN_DATE, 102))		 
		                    )";

                    string executedLateDocsQuery =
                    $@"
                    USE DELO_DB
                    /*ОТВЕТЫ С ОПОЗДАНИЕМ*/ 
                    SELECT DEPARTMENT.CLASSIF_NAME AS 'PERSON', DOC_RC.FREE_NUM AS 'DOCNUM', RESOLUTION.RESOLUTION_TEXT AS 'RESOLUTION_TEXT', RESOLUTION.PLAN_DATE AS 'PLAN', REPLY.REPLY_TEXT AS 'REPLY_TEXT', REPLY.REPLY_DATE AS 'WRITED_DATE', REPLY.UPD_DATE AS 'FACT_DATE' FROM RESOLUTION
                    FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
                    FULL JOIN DOC_RC ON RESOLUTION.ISN_REF_DOC = DOC_RC.ISN_DOC
                    FULL JOIN DEPARTMENT ON REPLY.DUE = DEPARTMENT.DUE
                     where REPLY.DUE = '{sqlId}' 
                     AND RESOLUTION.RESOLUTION_DATE BETWEEN '{sqlFirstDate}' AND '{sqlLastDate}'
                     AND (RESOLUTION.CONTROL_STATE = 1 or RESOLUTION.CONTROL_STATE = 2)
                     AND ( REPLY.UPD_DATE IS NOT NULL 
                        AND RESOLUTION.PLAN_DATE IS NOT NULL 
                        AND CONVERT(VARCHAR, REPLY.UPD_DATE, 102) > CONVERT(varchar, RESOLUTION.PLAN_DATE, 102))";

                    string notExequtedDocsQuery =
                    $@"
                    USE DELO_DB
                    /*НЕ ИСПОЛНЕНО*/
                    SELECT DEPARTMENT.CLASSIF_NAME AS 'PERSON', DOC_RC.FREE_NUM AS 'DOCNUM', RESOLUTION.RESOLUTION_TEXT AS 'RESOLUTION_TEXT', RESOLUTION.PLAN_DATE AS 'PLAN', REPLY.REPLY_TEXT AS 'REPLY_TEXT', REPLY.REPLY_DATE AS 'WRITED_DATE', REPLY.UPD_DATE AS 'FACT_DATE' FROM RESOLUTION
                    FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
                    FULL JOIN DOC_RC ON RESOLUTION.ISN_REF_DOC = DOC_RC.ISN_DOC
                    FULL JOIN DEPARTMENT ON REPLY.DUE = DEPARTMENT.DUE 
                      where REPLY.DUE = '{sqlId}' 
                      AND RESOLUTION.RESOLUTION_DATE BETWEEN '{sqlFirstDate}' AND '{sqlLastDate}'
                        AND (RESOLUTION.CONTROL_STATE = 1 or RESOLUTION.CONTROL_STATE = 2)
                      AND REPLY.UPD_DATE IS NULL
                      AND (RESOLUTION.PLAN_DATE IS NULL OR RESOLUTION.PLAN_DATE < GETDATE())";

                    string deadlineIsNotQuery =
                    $@"
                    USE DELO_DB
                    /*СРОК НЕ НАСТУПИЛ*/
                    SELECT DEPARTMENT.CLASSIF_NAME AS 'PERSON', DOC_RC.FREE_NUM AS 'DOCNUM', RESOLUTION.RESOLUTION_TEXT AS 'RESOLUTION_TEXT', RESOLUTION.PLAN_DATE AS 'PLAN', REPLY.REPLY_TEXT AS 'REPLY_TEXT', REPLY.REPLY_DATE AS 'WRITED_DATE', REPLY.UPD_DATE AS 'FACT_DATE' FROM RESOLUTION
                    FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
                    FULL JOIN DOC_RC ON RESOLUTION.ISN_REF_DOC = DOC_RC.ISN_DOC
                    FULL JOIN DEPARTMENT ON REPLY.DUE = DEPARTMENT.DUE
                      where REPLY.DUE = '{sqlId}'
                      AND RESOLUTION.RESOLUTION_DATE BETWEEN '{sqlFirstDate}' AND '{sqlLastDate}'
                      AND REPLY.UPD_DATE IS NULL
                      AND (RESOLUTION.CONTROL_STATE = 1 or RESOLUTION.CONTROL_STATE = 2)
                      AND ( RESOLUTION.PLAN_DATE IS NOT NULL 
		                    AND RESOLUTION.PLAN_DATE >= GETDATE());";

                    SqlDataAdapter personAdapter = new SqlDataAdapter(personQuery, connection);
                    personAdapter.Fill(dataSet, "PERSON");

                    SqlDataAdapter allDocsAdapter = new SqlDataAdapter(allDocsQuery, connection);
                    allDocsAdapter.Fill(dataSet, "DOC");

                    SqlDataAdapter executedDocsAdapter = new SqlDataAdapter(executedDocsQuery, connection);
                    executedDocsAdapter.Fill(dataSet, "EXECUTED_DOCS");

                    SqlDataAdapter executedLateDocs = new SqlDataAdapter(executedLateDocsQuery, connection);
                    executedLateDocs.Fill(dataSet, "EXECUTED_LATE_DOCS");

                    SqlDataAdapter notExecutedDocsAdapter = new SqlDataAdapter(notExequtedDocsQuery, connection);
                    notExecutedDocsAdapter.Fill(dataSet, "NOT_EXECUTED_DOCS");

                    SqlDataAdapter deadlineIsNotAdapter = new SqlDataAdapter(deadlineIsNotQuery, connection);
                    deadlineIsNotAdapter.Fill(dataSet, "DEADLINE_IS_NOT");
                }
                connection.Close();
            }

            string personName;
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

            //заголовок
            workSheet.Cells[1, 1] = $@"{this.Name}  
      {firstDate.ToString("dd MMMM yyyy")} - {lastDate.ToString("dd MMMM yyyy")}";

            //Берем верхнюю границу выгруженных данных
            upperRow = workSheet.Cells[1, 1];

            for (int i = 0; i < dataSet.Tables["PERSON"].Rows.Count; i++)
            {
                row++;

                //формирование строки исполнителя
                c1 = workSheet.Cells[row, 1];
                c2 = workSheet.Cells[row, 5];
                range = workSheet.get_Range(c1, c2);
                range.WrapText = true;
                range.Merge();

                personName = (string)dataSet.Tables["PERSON"].Rows[i].ItemArray[0];

                //вывод имени

                workSheet.Cells[row, 1] = personName;

                row++;

                //выгрузка информации о документах исполнителя
                var allDocs = from output in dataSet.Tables["DOC"].AsEnumerable()
                              where output.Field<string>("PERSON") == personName
                              select new
                              {
                                  docNum = output.Field<string>("DOCNUM") ?? "",
                                  resolutionText = output.Field<string>("RESOLUTION_TEXT") ?? "",
                                  resolutionPlan = output.Field<DateTime?>("PLAN").ToString() ?? "",
                                  replyText = output.Field<string>("REPLY_TEXT") ?? "",
                                  writedDate = output.Field<DateTime?>("WRITED_DATE").ToString() ?? "",
                                  factDate = output.Field<DateTime?>("FACT_DATE").ToString() ?? ""
                              };

                var executedDocs = from output in dataSet.Tables["EXECUTED_DOCS"].AsEnumerable()
                                   where output.Field<string>("PERSON") == personName
                                   select new
                                   {
                                       docNum = output.Field<string>("DOCNUM") ?? "",
                                       resolutionText = output.Field<string>("RESOLUTION_TEXT") ?? "",
                                       resolutionPlan = output.Field<DateTime?>("PLAN").ToString() ?? "",
                                       replyText = output.Field<string>("REPLY_TEXT") ?? "",
                                       writedDate = output.Field<DateTime?>("WRITED_DATE").ToString() ?? "",
                                       factDate = output.Field<DateTime?>("FACT_DATE").ToString() ?? ""
                                   };

                var executedLateDocs = from output in dataSet.Tables["EXECUTED_LATE_DOCS"].AsEnumerable()
                                       where output.Field<string>("PERSON") == personName
                                       select new
                                       {
                                           docNum = output.Field<string>("DOCNUM") ?? "",
                                           resolutionText = output.Field<string>("RESOLUTION_TEXT") ?? "",
                                           resolutionPlan = output.Field<DateTime?>("PLAN").ToString() ?? "",
                                           replyText = output.Field<string>("REPLY_TEXT") ?? "",
                                           writedDate = output.Field<DateTime?>("WRITED_DATE").ToString() ?? "",
                                           factDate = output.Field<DateTime?>("FACT_DATE").ToString() ?? ""
                                       };

                var notExecutedDocs = from output in dataSet.Tables["NOT_EXECUTED_DOCS"].AsEnumerable()
                                      where output.Field<string>("PERSON") == personName
                                      select new
                                      {
                                          docNum = output.Field<string>("DOCNUM") ?? "",
                                          resolutionText = output.Field<string>("RESOLUTION_TEXT") ?? "",
                                          resolutionPlan = output.Field<DateTime?>("PLAN").ToString() ?? "",
                                          replyText = output.Field<string>("REPLY_TEXT") ?? "",
                                          writedDate = output.Field<DateTime?>("WRITED_DATE").ToString() ?? "",
                                          factDate = output.Field<DateTime?>("FACT_DATE").ToString() ?? ""
                                      };

                var deadlineIsNotDocs = from output in dataSet.Tables["DEADLINE_IS_NOT"].AsEnumerable()
                                        where output.Field<string>("PERSON") == personName
                                        select new
                                        {
                                            docNum = output.Field<string>("DOCNUM") ?? "",
                                            resolutionText = output.Field<string>("RESOLUTION_TEXT") ?? "",
                                            resolutionPlan = output.Field<DateTime?>("PLAN").ToString() ?? "",
                                            replyText = output.Field<string>("REPLY_TEXT") ?? "",
                                            writedDate = output.Field<DateTime?>("WRITED_DATE").ToString() ?? "",
                                            factDate = output.Field<DateTime?>("FACT_DATE").ToString() ?? ""
                                        };

                //все
                //формирование строки типа документа
                c1 = workSheet.Cells[row, 1];
                c2 = workSheet.Cells[row, 6];
                range = workSheet.get_Range(c1, c2);
                range.WrapText = true;
                range.Merge();
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                workSheet.Cells[row, 1] = "Все документы";
                row++;

                workSheet.Cells[row, 1] = "№";
                workSheet.Cells[row, 2] = "Текст поручения";
                workSheet.Cells[row, 3] = "Дата поручения";
                workSheet.Cells[row, 4] = "Текст отчёта";
                workSheet.Cells[row, 5] = "Дата в отчёте";
                workSheet.Cells[row, 6] = "Дата в системе";

                row++;

                foreach (var item in allDocs)
                {
                    workSheet.Cells[row, 1] = (string)item.docNum;
                    workSheet.Cells[row, 2] = (string)item.resolutionText;
                    workSheet.Cells[row, 3] = (string)item.resolutionPlan;
                    workSheet.Cells[row, 4] = (string)item.replyText;
                    workSheet.Cells[row, 5] = (string)item.writedDate;
                    workSheet.Cells[row, 6] = (string)item.factDate;
                    row++;
                }

                row += 2;

                //исполненные
                //формирование строки типа документа
                c1 = workSheet.Cells[row, 1];
                c2 = workSheet.Cells[row, 6];
                range = workSheet.get_Range(c1, c2);
                range.WrapText = true;
                range.Merge();
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                workSheet.Cells[row, 1] = "Исполненные";
                row++;

                workSheet.Cells[row, 1] = "№";
                workSheet.Cells[row, 2] = "Текст поручения";
                workSheet.Cells[row, 3] = "Дата поручения";
                workSheet.Cells[row, 4] = "Текст отчёта";
                workSheet.Cells[row, 5] = "Дата в отчёте";
                workSheet.Cells[row, 6] = "Дата в системе";

                row++;

                foreach (var item in executedDocs)
                {
                    workSheet.Cells[row, 1] = (string)item.docNum;
                    workSheet.Cells[row, 2] = (string)item.resolutionText;
                    workSheet.Cells[row, 3] = (string)item.resolutionPlan;
                    workSheet.Cells[row, 4] = (string)item.replyText;
                    workSheet.Cells[row, 5] = (string)item.writedDate;
                    workSheet.Cells[row, 6] = (string)item.factDate;
                    row++;
                }

                row += 2;

                //исполненные с опозданием
                //формирование строки типа документа
                c1 = workSheet.Cells[row, 1];
                c2 = workSheet.Cells[row, 6];
                range = workSheet.get_Range(c1, c2);
                range.WrapText = true;
                range.Merge();
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                workSheet.Cells[row, 1] = "Исполнено с опозданием";
                row++;

                workSheet.Cells[row, 1] = "№";
                workSheet.Cells[row, 2] = "Текст поручения";
                workSheet.Cells[row, 3] = "Дата поручения";
                workSheet.Cells[row, 4] = "Текст отчёта";
                workSheet.Cells[row, 5] = "Дата в отчёте";
                workSheet.Cells[row, 6] = "Дата в системе";

                row++;

                foreach (var item in executedLateDocs)
                {
                    workSheet.Cells[row, 1] = (string)item.docNum;
                    workSheet.Cells[row, 2] = (string)item.resolutionText;
                    workSheet.Cells[row, 3] = (string)item.resolutionPlan;
                    workSheet.Cells[row, 4] = (string)item.replyText;
                    workSheet.Cells[row, 5] = (string)item.writedDate;
                    workSheet.Cells[row, 6] = (string)item.factDate;
                    row++;
                }

                row += 2;

                //не исполненные
                //формирование строки типа документа
                c1 = workSheet.Cells[row, 1];
                c2 = workSheet.Cells[row, 6];
                range = workSheet.get_Range(c1, c2);
                range.WrapText = true;
                range.Merge();
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                workSheet.Cells[row, 1] = "не исполнено";
                row++;

                workSheet.Cells[row, 1] = "№";
                workSheet.Cells[row, 2] = "Текст поручения";
                workSheet.Cells[row, 3] = "Дата поручения";
                workSheet.Cells[row, 4] = "Текст отчёта";
                workSheet.Cells[row, 5] = "Дата в отчёте";
                workSheet.Cells[row, 6] = "Дата в системе";

                row++;

                foreach (var item in notExecutedDocs)
                {
                    workSheet.Cells[row, 1] = (string)item.docNum;
                    workSheet.Cells[row, 2] = (string)item.resolutionText;
                    workSheet.Cells[row, 3] = (string)item.resolutionPlan;
                    workSheet.Cells[row, 4] = (string)item.replyText;
                    workSheet.Cells[row, 5] = (string)item.writedDate;
                    workSheet.Cells[row, 6] = (string)item.factDate;
                    row++;
                }

                row += 2;

                //Срок не наступил
                //формирование строки типа документа
                c1 = workSheet.Cells[row, 1];
                c2 = workSheet.Cells[row, 6];
                range = workSheet.get_Range(c1, c2);
                range.WrapText = true;
                range.Merge();
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                workSheet.Cells[row, 1] = "не наступил";
                row += 2;

                workSheet.Cells[row, 1] = "№";
                workSheet.Cells[row, 2] = "Текст поручения";
                workSheet.Cells[row, 3] = "Дата поручения";
                workSheet.Cells[row, 4] = "Текст отчёта";
                workSheet.Cells[row, 5] = "Дата в отчёте";
                workSheet.Cells[row, 6] = "Дата в системе";

                row++;

                foreach (var item in deadlineIsNotDocs)
                {
                    workSheet.Cells[row, 1] = (string)item.docNum;
                    workSheet.Cells[row, 2] = (string)item.resolutionText;
                    workSheet.Cells[row, 3] = (string)item.resolutionPlan;
                    workSheet.Cells[row, 4] = (string)item.replyText;
                    workSheet.Cells[row, 5] = (string)item.writedDate;
                    workSheet.Cells[row, 6] = (string)item.factDate;
                    row++;
                }
            }

            bottomRow = workSheet.Cells[row, 6];
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
