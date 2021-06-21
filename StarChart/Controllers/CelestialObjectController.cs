using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]       
        public IActionResult GetById(int id)
        {
           CelestialObject myobject = _context.CelestialObjects.Find(id);

            if(myobject is null)
            {
                return NotFound();
            }

            myobject.Satellites = ((CelestialObject)_context.CelestialObjects.Select(x => x.OrbitedObjectId == myobject.OrbitedObjectId)).Satellites;
                         
            return Ok(myobject);
        }

        [HttpGet("{name}", Name = "GetByName")]
        public ActionResult<CelestialObject[]> GetByName(string name)
        {
            CelestialObject myobjects = _context.CelestialObjects.Find(name);

            if (myobjects is null)
            {
                return NotFound();
            }

            myobjects.Satellites = ((CelestialObject)_context.CelestialObjects.Select(x => x.OrbitedObjectId == myobjects.OrbitedObjectId && x.Name == myobjects.Name)).Satellites;

            return Ok(myobjects);
        }

        [HttpGet(Name = "GetAll")]
        public ActionResult<CelestialObject[]> GetAll()
        {
            var myobjects = from o in _context.CelestialObjects
                            select o;
                            

            if (myobjects.Count() == 0)
            {
                return NotFound();
            }

            return Ok(myobjects);
        }
    }
}
