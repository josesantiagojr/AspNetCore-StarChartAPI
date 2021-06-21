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
           var myobject = _context.CelestialObjects.Find(id);

            if(myobject is null)
            {
                return NotFound();
            }

            myobject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == myobject.OrbitedObjectId).ToList();
                         
            return Ok(myobject);
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var myobjects = _context.CelestialObjects.Where(x => x.Name == name).ToList();

            if (!myobjects.Any())
            {
                return NotFound();
            }
            foreach (var obj in myobjects)
            {
                obj.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == obj.OrbitedObjectId).ToList();
            }

         

            return Ok(myobjects);
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var myobjects = from o in _context.CelestialObjects
                            select o;

            if (myobjects.Count() == 0)
            {
                return NotFound();
            }

            foreach (var obj in myobjects)
            {
                obj.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == obj.OrbitedObjectId).ToList();
            }



            return Ok(myobjects);
        }
    }
}
