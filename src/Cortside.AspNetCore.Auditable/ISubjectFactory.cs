using Cortside.AspNetCore.Auditable.Entities;

namespace Cortside.AspNetCore.Auditable {
    public interface ISubjectFactory<TSubject> where TSubject : Subject {
        TSubject CreateSubject(Common.Security.ISubjectPrincipal subjectPrincipal);
    }
}
