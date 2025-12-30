using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Dtos.Stock;

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
            var stocks = _context.Stock.ToList()
            .Select(s => s.ToStockDto());
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
            return Ok(stock.ToStockDto());
        }

        // FromBody allows data to be passed as JSON in the body of the request
        // Creating Dto - certain types of data we want to accept from the user - restricted fields
        // DB only accepts Model objects not Dto objects
        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();

            _context.Stock.Add(stockModel);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetById),
                new { id = stockModel.Id },
                stockModel.ToStockDto()
            );
        }
    }
}