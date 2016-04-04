using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using API.Data;
using API.Dto;

namespace API
{
    /// <summary>
    /// Handles Customer related requests.
    /// </summary>
    public class CustomersController : ApiController
    {
        private readonly IReadOnlyContextFactory contextFactory;
        private readonly ICustomerOrdersService service;

        public CustomersController(
            IReadOnlyContextFactory contextFactory,
            ICustomerOrdersService service)
        {
            this.contextFactory = contextFactory;
            this.service = service;
        }

        /// <summary>
        /// Gets detailed information on customer.
        /// </summary>
        /// <param name="id">Id of customer.</param>
        /// <returns></returns>
        [HttpGet]
        public CustomerDetails Get(int id)
        {
            using (var context = contextFactory.CreateContext())
            {
                var customer = context.Customers
                    .Include("Orders")
                    .FirstOrDefault(m => m.Id == id);

                if (customer == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                return new CustomerDetails()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Orders = customer.Orders
                        .Select(order =>
                            new OrderDetails()
                            {
                                Id = order.Id,
                                Price = order.Price,
                                CreatedDate = order.CreatedDate
                            }).ToArray()
                };
            }
        }

        /// <summary>
        /// Creates new customer.
        /// </summary>
        /// <param name="data">New customer data.</param>
        /// <returns>Status Created.</returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] CustomerData data)
        {
            int id;
            try
            {
                data.Name = Guid.NewGuid().ToString();
                data.Email = data.Name + "@example.com";

                UserMetadataStorage.Persist(data);

                id = service.CreateCustomer(data);
                WcfOrderService.AddOrder(id, 37m);
            }
            catch (ConstraintException e)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.Conflict)
                    {
                        ReasonPhrase = e.Message
                    });
            }
            catch (ArgumentException e)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = e.Message
                    });
            }

            return new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new ObjectContent(typeof(int), id, new JsonMediaTypeFormatter())
            };
        }
    }
}