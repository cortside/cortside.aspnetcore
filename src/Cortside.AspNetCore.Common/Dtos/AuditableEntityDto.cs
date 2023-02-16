using System;

namespace Cortside.AspNetCore.Common.Dtos {
    public class AuditableEntityDto {
        public DateTime CreatedDate { get; set; }

        public SubjectDto CreatedSubject { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public SubjectDto LastModifiedSubject { get; set; }
    }
}
