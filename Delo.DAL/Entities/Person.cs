using Delo.DAL.Entities.Base;

namespace Delo.DAL.Entities
{
    public class Person : NamedEntity
    {
        public virtual string Department { get; set; }
        public virtual string Duty { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Duty}";
        }
    }
}
