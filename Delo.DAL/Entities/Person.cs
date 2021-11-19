using Delo.DAL.Entities.Base;

namespace Delo.DAL.Entities
{
    public class Person : NamedEntity
    {
        public virtual Department Department { get; set; }
        public virtual string Position { get; set; }
    }
}
