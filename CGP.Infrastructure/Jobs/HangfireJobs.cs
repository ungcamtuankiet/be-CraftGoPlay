using CGP.Application.Interfaces;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Jobs
{
    public static class HangfireJobs
    {
        public static void RegisterJobs()
        {
            RecurringJob.AddOrUpdate<IWalletService>(
                "release-wallet-job",
                x => x.ReleasePendingTransactionsAsync(),
                "*/5 * * * *" // cron expression: mỗi 5 phút
            );
        }
    }
}
