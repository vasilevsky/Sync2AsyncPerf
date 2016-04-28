using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using API.Data;

namespace API
{
    public class HeavyDbRequestsAsyncController
    {
        PeriodStartRandomizer randomizer = new PeriodStartRandomizer();

        /// <summary>
        /// Gets stastics over period 
        /// </summary>
        public async Task<PeriodSummaryDto> SingleQueryHeavyComputationAsync()
        {
            var monthPeriod = randomizer.GetPeriod();

            using (var data = new CustomerOrdersEntities())
            {
                var q = from o in data.Orders
                        where o.CreatedDate >= monthPeriod.Start && o.CreatedDate <= monthPeriod.End
                        group o by o.Id
                            into og
                            select
                                new
                                {
                                    PeriodStart = monthPeriod.Start,
                                    PeriodEnd = monthPeriod.End,
                                    TotalSpent = og.Sum(m => m.Price),
                                    UniqueCustomers = og.Select(m => m.Customer_Id).Distinct().Count(),
                                    UniqueProducts = og.Select(m => m.OrderLines.Select(ol => ol.Product_Id)).Distinct().Count(),
                                };

                var x = await q.FirstOrDefaultAsync();
                return new PeriodSummaryDto
                    {
                        PeriodStart = x.PeriodStart,
                        PeriodEnd = x.PeriodEnd,
                        TotalSpent = x.TotalSpent,
                        UniqueCustomers = x.UniqueCustomers,
                        UniqueProducts = x.UniqueProducts
                    };
            }
        }

        public class PeriodSummaryDto
        {
            public DateTime PeriodStart { get; set; }
            public DateTime PeriodEnd { get; set; }
            public decimal TotalSpent { get; set; }
            public int UniqueCustomers { get; set; }
            public int UniqueProducts { get; set; }

        }

        public class DataSeeding
        {
            public static readonly DateTime StartDate = new DateTime(2006, 1, 1);
            public static readonly DateTime EndDate = new DateTime(2016, 1, 1);
        }

        private class PeriodStartRandomizer
        {
            Random randomizer = new Random();

            public Period GetPeriod()
            {
                var full = DataSeeding.EndDate - DataSeeding.StartDate;
                var startFromDay = randomizer.Next(0, (int)full.TotalDays - 30);

                return new Period()
                {
                    Start = DataSeeding.StartDate.AddDays(startFromDay),
                    End = DataSeeding.StartDate.AddDays(startFromDay + 30)
                };
            }
        }

        private struct Period
        {
            public DateTime Start;
            public DateTime End;
        }
    }
}