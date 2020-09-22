using HMACAuthenticationWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HMACAuthenticationWebApi.Controllers
{
    [HMACAuthentication]
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(Order.GetOrders());
        }
        [Route("")]
        public IHttpActionResult Post(Order order)
        {
            return Ok(order);
        }
    }
}
