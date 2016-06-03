using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using API.Data;
using Domain.Data;
using Domain.Migrations;

namespace API
{
    public class HeavyDbRequestsController : ApiController
    {
        PeriodStartRandomizer randomizer = new PeriodStartRandomizer();

        [HttpGet]
        public void SeedData()
        {
            var seeding = new DataSeeding(new CustomerOrdersContext());
            seeding.Seed(10000, 1000);
        }

        [Route("aheavy")]
        [HttpGet]
        public async Task<PeriodSummaryDto> SingleQueryHeavyComputationAsync()
        {
            var monthPeriod = randomizer.GetPeriod();

            using (var data = new CustomerOrdersEntities())
            {
                var q = from o in data.Orders
                        where o.CreatedDate >= monthPeriod.Start && o.CreatedDate <= monthPeriod.End
                        group o by 1 into og
                        select
                            new
                            {
                                PeriodStart = monthPeriod.Start,
                                PeriodEnd = monthPeriod.End,
                                FirstOrderInPeriodDate = og.Min(m => m.CreatedDate),
                                LastOrderInPeriodDate = og.Max(m => m.CreatedDate),
                                OrdersInPeriod = og.Count(),
                                TotalSpent = og.SelectMany(m => m.OrderLines).Sum(m => m.Price),
                                UniqueCustomers = og.Select(m => m.Customer_Id).Distinct().Count(),
                                UniqueProducts = og.SelectMany(m => m.OrderLines).Select(m => m.Product_Id).Distinct().Count(),
                            };

                var x = await q.FirstOrDefaultAsync();
                return new PeriodSummaryDto
                {
                    PeriodStart = x.PeriodStart,
                    PeriodEnd = x.PeriodEnd,
                    FirstOrderInPeriodDate = x.FirstOrderInPeriodDate,
                    LastOrderInPeriodDate = x.LastOrderInPeriodDate,
                    OrdersInPeriod = x.OrdersInPeriod,
                    TotalSpent = x.TotalSpent,
                    UniqueCustomers = x.UniqueCustomers,
                    UniqueProducts = x.UniqueProducts
                };
            }

        }


        /// <summary>
        /// Gets stastics over period 
        /// </summary>
        [Route("heavy")]
        [HttpGet]
        public PeriodSummaryDto SingleQueryHeavyComputation()
        {
            var monthPeriod = randomizer.GetPeriod();

            using (var data = new CustomerOrdersEntities())
            {
                var q = from o in data.Orders
                        where o.CreatedDate >= monthPeriod.Start && o.CreatedDate <= monthPeriod.End
                        group o by 1 into og
                        select
                            new
                            {
                                PeriodStart = monthPeriod.Start,
                                PeriodEnd = monthPeriod.End,
                                FirstOrderInPeriodDate = og.Min(m => m.CreatedDate),
                                LastOrderInPeriodDate = og.Max(m => m.CreatedDate),
                                OrdersInPeriod = og.Count(),
                                TotalSpent = og.SelectMany(m => m.OrderLines).Sum(m => m.Price),
                                UniqueCustomers = og.Select(m => m.Customer_Id).Distinct().Count(),
                                UniqueProducts = og.SelectMany(m => m.OrderLines).Select(m => m.Product_Id).Distinct().Count(),
                            };

                var x = q.FirstOrDefault();

                return new PeriodSummaryDto
                {
                    PeriodStart = x.PeriodStart,
                    PeriodEnd = x.PeriodEnd,
                    FirstOrderInPeriodDate = x.FirstOrderInPeriodDate,
                    LastOrderInPeriodDate = x.LastOrderInPeriodDate,
                    OrdersInPeriod = x.OrdersInPeriod,
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
            public DateTime FirstOrderInPeriodDate { get; set; }
            public DateTime LastOrderInPeriodDate { get; set; }
            public int OrdersInPeriod { get; set; }
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