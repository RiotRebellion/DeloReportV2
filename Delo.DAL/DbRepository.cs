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
        private readonly DeloDB _Db;
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


        public T Get(int id) => Items.SingleOrDefault(items => items.Id == id);

        public async Task<T> GetAsync(int id, CancellationToken cancel = default) => await Items
            .SingleOrDefaultAsync(items => items.Id == id, cancel)
            .ConfigureAwait(false)
            ;

    }

    internal class PersonRepository : DbRepository<Person>
    {
        public override IQueryable<Person> Items => base.Items.Include(items => items.Department);

        public PersonRepository(DeloDB Db) : base(Db)
        {
        }
    }
}
