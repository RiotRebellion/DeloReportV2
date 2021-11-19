using System.Collections.Generic;

namespace Models
{
    public class Department
    {
        public string Name { get; set; }

        public ICollection<Person>? Persons { get; set; }
    }
}

