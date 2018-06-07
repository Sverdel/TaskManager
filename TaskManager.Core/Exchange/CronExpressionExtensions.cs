using System;
using System.Threading;
using System.Threading.Tasks;
using Cronos;

namespace TaskManager.Core.Exchange
{
    internal static class CronExpressionExtensions
    {
        private static readonly TimeSpan oneSecond = TimeSpan.FromSeconds(1);

        public static async Task WaitNextOccurrenceAsync(this CronExpression source, DateTime date, CancellationToken cancellation)
        {
            var now = DateTime.UtcNow;
            var nextOccurrence = source.GetNextOccurrence(date);
            if (nextOccurrence == null)
            {
                return;
            }

            var delayToNextOccurrence = nextOccurrence.Value - now;
            if (delayToNextOccurrence > TimeSpan.Zero)
            {
                await Task.Delay(delayToNextOccurrence > oneSecond ? delayToNextOccurrence : oneSecond, cancellation).ConfigureAwait(false);
            }
        }
    }
}
