using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ema.ContentData.Domain.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ema.ContentData.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private Topic[] _temp = new Topic[] {
          new Topic() {
              Id=1,
              Header ="Topic 00001",
              Topic_Detail="asdsadsadasdasd asdasdsadsadsadsa Ïdasdasdasdasdasdas dasdasdasdasdasdasdasdasdasdsa"
          },
            new Topic() {  Id=2,
            Header ="Topic 00002",
              Topic_Detail="asdsadsadasdasd asdasdsadsadsadsa Ïdasdasdasdasdasdas dasdasdasdasdasdasdasdasdasdsa"}
        };

        // GET: api/News
        [HttpGet]
        public IEnumerable<Topic> Get()
        {
            return _temp;
        }

        // GET: api/News/5
        [HttpGet("{id}", Name = "Get")]
        public Topic Get(int id)
        {
            return new Topic()
            {
                Id = id,
                Topic_Detail = "asdsadsadasdasd asdasdsadsadsadsa Ïdasdasdasdasdasdas dasdasdasdasdasdasdasdasdasdsa"
            };
        }

        //// POST: api/News
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/News/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/News/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
