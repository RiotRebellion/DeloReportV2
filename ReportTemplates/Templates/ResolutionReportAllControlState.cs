using Delo.DAL.Entities;
using ReportTemplates.Templates.Base;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.ObjectModel;
using System;
using OfficeOpenXml;
using OfficeOpenXml.Table;

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
					PERSON_NAME = (SELECT SURNAME FROM DEPARTMENT WHERE DUE = @PERSON_ID),
						
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
												 AND (reply.PLAN_DATE IS NULL 
												 OR CONVERT(VARCHAR, reply.UPD_DATE, 102) <= CONVERT(VARCHAR, REPLY.PLAN_DATE, 102))		 
												 )),

					/*ОТВЕТЫ С ОПОЗДАНИЕМ*/ 
					EXECUTED_LATE_COUNT = (SELECT COUNT(*) AS 'EXECUTED_LATE' FROM RESOLUTION
											FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
											  where REPLY.DUE = @PERSON_ID 
											  AND RESOLUTION.RESOLUTION_DATE BETWEEN @FIRST_DATE AND @LAST_DATE
											  AND ( REPLY.UPD_DATE IS NOT NULL 
													AND REPLY.PLAN_DATE IS NOT NULL 
													AND CONVERT(VARCHAR, REPLY.UPD_DATE, 102) > CONVERT(varchar, REPLY.PLAN_DATE, 102))),

					/*НЕ ИСПОЛНЕНО*/
					NOT_EXECUTED_COUNT = (SELECT COUNT(*) AS 'NOT_EXECUTED' FROM RESOLUTION
											FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
											  where REPLY.DUE = @PERSON_ID 
											  AND RESOLUTION.RESOLUTION_DATE BETWEEN @FIRST_DATE AND @LAST_DATE
											  AND REPLY.UPD_DATE IS NULL
											  AND (REPLY.PLAN_DATE IS NULL OR REPLY.PLAN_DATE < GETDATE())),

					/*СРОК НЕ НАСТУПИЛ*/
					DEADLINE_IS_NOT_COUNT = (SELECT COUNT(*) AS 'DEADLINE_IS_NOT' FROM RESOLUTION
											FULL JOIN REPLY ON RESOLUTION.ISN_RESOLUTION = REPLY.ISN_RESOLUTION
											  where REPLY.DUE = @PERSON_ID 
											  AND RESOLUTION.RESOLUTION_DATE BETWEEN @FIRST_DATE AND @LAST_DATE
											  AND REPLY.UPD_DATE IS NULL
											  AND ( REPLY.PLAN_DATE IS NOT NULL 
													AND REPLY.PLAN_DATE >= GETDATE()))";

					SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
					adapter.Fill(dataSet, "REPORT");
				}
				connection.Close();
			}
		}

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
