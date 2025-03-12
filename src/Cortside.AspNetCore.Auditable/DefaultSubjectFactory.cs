using System;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.Common.Security;

namespace Cortside.AspNetCore.Auditable {
    public class DefaultSubjectFactory : ISubjectFactory<Subject> {
        public Subject CreateSubject(ISubjectPrincipal subjectPrincipal) {
            return new Subject(Guid.Parse(subjectPrincipal.SubjectId), subjectPrincipal.GivenName,
                subjectPrincipal.FamilyName, subjectPrincipal.Name, subjectPrincipal.UserPrincipalName);
        }
    }
}
