using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/Stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        // Making it read only
        // here to get things in and out of the database
        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var stocks  = _context.Stock.ToList();
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stock.Find(id);

            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock);
        }
    }
}