using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Cortside.AspNetCore.EntityFramework.Interceptors {
    public class QueryHintCommandInterceptor : DbCommandInterceptor {
        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result) {
            ManipulateCommand(command);
            return result;
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default) {
            ManipulateCommand(command);
            return new ValueTask<InterceptionResult<DbDataReader>>(result);
        }

        public static void ManipulateCommand(DbCommand command) {
            // expose constant

            // log if called with what string

            const string prefix = "-- Use option: ";
            var s = command.CommandText.Split(Environment.NewLine).FirstOrDefault();

            if (!s!.StartsWith(prefix, StringComparison.Ordinal)) {
                return;
            }

            var option = s.Replace(prefix, "");

            // check to make sure it's not added already
            // add to new line

            command.CommandText += $" OPTION ({option})";
        }
    }
}
