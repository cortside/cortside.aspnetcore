using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cortside.AspNetCore.Auditable.Entities {
    [Table("Subject")]
    public class Subject {
        public Subject(Guid subjectId, string givenName, string familyName, string name, string userPrincipalName) {
            SubjectId = subjectId;
            GivenName = givenName;
            FamilyName = familyName;
            Name = name;
            UserPrincipalName = userPrincipalName;
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Subject primary key")]
        public Guid SubjectId { get; private set; }

        [StringLength(100)]
        [Comment("Subject primary key")]
        public string Name { get; private set; }

        [StringLength(100)]
        [Comment("Subject primary key")]
        public string GivenName { get; private set; }

        [StringLength(100)]
        [Comment("Subject Surname ()")]
        public string FamilyName { get; private set; }

        [StringLength(100)]
        [Comment("Username (upn claim)")]
        public string UserPrincipalName { get; private set; }

        [Comment("Date and time entity was created")]
        public DateTime CreatedDate { get; private set; }

        public void Update(string givenName, string familyName, string name, string userPrincipalName) {
            GivenName = givenName;
            FamilyName = familyName;
            Name = name;
            UserPrincipalName = userPrincipalName;
        }
    }
}
