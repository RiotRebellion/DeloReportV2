﻿using Delo.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations;


namespace Delo.DAL.Entities
{
    public class Department : NamedEntity
    {
        public virtual ICollection<Person> People { get; set; }
    }
}
