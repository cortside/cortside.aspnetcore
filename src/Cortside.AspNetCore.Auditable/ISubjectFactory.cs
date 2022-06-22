using Cortside.AspNetCore.Auditable.Entities;
using Cortside.Common.Security;

namespace Cortside.AspNetCore.Auditable {
    public interface ISubjectFactory<TSubject> where TSubject : Subject {
        TSubject CreateSubject(ISubjectPrincipal subjectPrincipal);
    }
}
