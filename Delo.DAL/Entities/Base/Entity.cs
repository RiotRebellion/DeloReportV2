﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Delo.DAL.Entities.Base
{
    public abstract class Entity
    {
        [Key]
        public string ID { get; set; }
    }
}
