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

            if (myobject is null)
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

        [HttpGet]
        public IActionResult GetAll()
        {
            var myobjects = _context.CelestialObjects.ToList();

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

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject co)
        {

            _context.CelestialObjects.Add(co);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { Id = co.Id }, co);


        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject co)
        {
            var existing = _context.CelestialObjects.Find(id);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = co.Name;
            existing.OrbitalPeriod = co.OrbitalPeriod;
            existing.OrbitedObjectId = co.OrbitedObjectId;

            _context.CelestialObjects.Update(existing);
            _context.SaveChanges();

            return NoContent();

        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var existing = _context.CelestialObjects.Find(id);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = name;

            _context.CelestialObjects.Update(existing);
            _context.SaveChanges();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _context.CelestialObjects.Where(x => x.Id == id || id == x.OrbitedObjectId);

            if (!existing.Any())
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(existing);
            _context.SaveChanges();

            return NoContent();

        }
    }
}
