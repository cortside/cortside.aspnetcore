using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Cortside.AspNetCore.EntityFramework.Conventions {
    public class BlankTriggerAddingConvention : IModelFinalizingConvention {
        public virtual void ProcessModelFinalizing(
            IConventionModelBuilder modelBuilder,
            IConventionContext<IConventionModelBuilder> context) {
            foreach (var entityType in modelBuilder.Metadata.GetEntityTypes()) {
                var table = StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);
                if (table != null
                    && entityType.GetDeclaredTriggers().All(t => t.GetDatabaseName(table.Value) == null)
                    && (entityType.BaseType == null
                        || entityType.GetMappingStrategy() != RelationalAnnotationNames.TphMappingStrategy)) {
                    entityType.Builder.HasTrigger("tr" + table.Value.Name);
                }

                foreach (var fragment in entityType.GetMappingFragments(StoreObjectType.Table)) {
                    if (entityType.GetDeclaredTriggers().All(t => t.GetDatabaseName(fragment.StoreObject) == null)) {
                        entityType.Builder.HasTrigger("tr" + fragment.StoreObject.Name);
                    }
                }
            }
        }
    }
}
