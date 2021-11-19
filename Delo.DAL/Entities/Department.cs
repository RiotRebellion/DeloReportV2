using Delo.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delo.DAL.Entities
{
    public class Department : NamedEntity
    {
        public virtual ICollection<Person> People { get; set; }
    }
}
