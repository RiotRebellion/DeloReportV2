using System.ComponentModel.DataAnnotations;

namespace Delo.DAL.Entities.Base
{
    public abstract class NamedEntity : Entity
    {
        public virtual string Name { get; set; }
    }

}
