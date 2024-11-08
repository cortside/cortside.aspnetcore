using Cortside.AspNetCore.Auditable.Entities;

namespace Cortside.AspNetCore.EntityFramework.Tests.Contexts {
    public class Person : AuditableEntity {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
