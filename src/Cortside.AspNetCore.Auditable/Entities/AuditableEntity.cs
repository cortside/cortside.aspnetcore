using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cortside.AspNetCore.Auditable.Entities {
    /// <summary>
    /// Auditable entity base class
    /// </summary>
    public abstract class AuditableEntity {
        [Comment("Date and time entity was created")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey("CreatedSubjectId")]
        [Required]
        [Comment("SubjectId that created entity")]
        public virtual Subject CreatedSubject { get; set; }

        [Comment("Date and time entity was last modified")]
        public DateTime LastModifiedDate { get; set; }

        [ForeignKey("LastModifiedSubjectId")]
        [Required]
        [Comment("SubjectId that last modified entity")]
        public virtual Subject LastModifiedSubject { get; set; }
    }
}
