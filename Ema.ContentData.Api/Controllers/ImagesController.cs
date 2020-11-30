using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ema.ContentData.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        // GET: api/Images/5
        [HttpGet("{id}", Name = "GalleryTopic")]
        public string[] GalleryTopic(int id)
        {
            return new string[] { "value-0.jpg", "value-1.jpg", "value-2.jpg", "value-3.jpg", "value-4.jpg", "value-5.jpg", "value-6.jpg", "value-7.jpg" };
        }

    }
}
