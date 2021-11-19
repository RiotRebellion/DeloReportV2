using System;
using System.Collections.Generic;

namespace Models
{
    public class Report
    {
        public string? Name { get; }

        public Func<ICollection<Person>> OutloadReport;

    }   
}

