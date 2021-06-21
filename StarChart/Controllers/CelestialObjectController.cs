﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {

        public CelestialObjectController(ApplicationDbContext _context)
        {
            Context = _context;
        }

        public ApplicationDbContext Context { get; }

    }
}
