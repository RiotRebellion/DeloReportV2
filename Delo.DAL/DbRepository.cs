using Delo.DAL.Context;
using Delo.DAL.Entities;
using Delo.DAL.Entities.Base;
using Delo.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Delo.DAL
{
    internal class DbRepository<T> : IRepository<T> where T : Entity, new()
    {
        protected readonly DeloDB _Db;
        private readonly DbSet<T> _Set;
        private readonly bool _ConnectionState;

        public DbRepository(DeloDB Db)
        {
            _Db = Db;
            _Set = Db.Set<T>();
            _ConnectionState = Db.Database.CanConnect();
        }

        public virtual IQueryable<T> Items => _Set;

        public bool CanConnect<T>() => _ConnectionState;

    }

    internal class PersonRepository : DbRepository<Person>
    {
        private string query =
                      "SELECT E.DUE AS Id, D.CLASSIF_NAME AS Department, E.SURNAME as Name, E.DUTY as Duty " +
                      "FROM DEPARTMENT E " +
                      "JOIN DEPARTMENT D ON D.DUE = E.DEPARTMENT_DUE " +
                      "where " +
                      "e.DELETED = 0 " +
                      "and e.IS_NODE = 1 " +
                      "order by Name";

        public override IQueryable<Person> Items => base._Db.Persons.FromSqlRaw(query);

        public PersonRepository(DeloDB Db) : base(Db)
        {
        }
    }
}
