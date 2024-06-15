using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using TestWebApiNet8Docker.Services;

namespace TestWebApiNet8Docker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ODataQueryService _service;
        public StudentsController(ODataQueryService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> Get(ODataQueryOptions<Student> options)
        {

            return Ok(_service.GetStudents(options));
        }
    }
}
