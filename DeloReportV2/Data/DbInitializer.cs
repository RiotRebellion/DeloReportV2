using Delo.DAL.Context;
using Delo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DeloReportV2.Data
{
    public class DbInitializer
    {
        private readonly DeloDB _db;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(DeloDB db, ILogger<DbInitializer> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public async Task InitializeAsync() 
        {
            var timer = Stopwatch.StartNew();

            await _db.Database.EnsureDeletedAsync();
            _logger.LogInformation("Инициализация БД...");
            await _db.Database.MigrateAsync();

            _logger.LogInformation("проверка наличия БД...");
            if (await _db.Persons.AnyAsync()) return;

            await InitializeDepartments();
            await InitializePersons();           

            _logger.LogInformation("Инициализация бд выполнена за {0} мс", timer.ElapsedMilliseconds);
        }
        //Инициализация отделов
        private const int _DepartmentsCount = 3;

        private Department[] _Departments;
        private async Task InitializeDepartments()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация таблицы Departments...");

            _Departments = Enumerable.Range(1, _DepartmentsCount)
                .Select(i => new Department { Id = i++, Name = $"Отдел {i}" })
                .ToArray();

            await _db.Departments.AddRangeAsync(_Departments);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Инициализация таблицы Departments завершена за {0} мс", timer.ElapsedMilliseconds);
        }

        //Инициализация сотрудников
        private const int _PeopleCount = 10;

        private Person[] _Persons;
        private async Task InitializePersons()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация таблицы Persons...");

            var rnd = new Random();
            _Persons = Enumerable.Range(1, _PeopleCount)
                .Select(i => new Person
                {
                    Id = i,
                    Name = $"Работник {i + 1}",
                    Position = "Должность {i + 1}",
                    Department = rnd.NextItem(_Departments)
                })
                .ToArray();


            await _db.Persons.AddRangeAsync(_Persons);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Инициализация таблицы Persons завершена за {0} мс", timer.ElapsedMilliseconds);
        }
    }
}
