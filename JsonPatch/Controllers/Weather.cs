using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JsonPatch.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.JsonPatch;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JsonPatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Weather : ControllerBase
    {
        private readonly json_weather _context;

        public Weather(json_weather context)
        {
            _context = context;

            if (_context.climate.Count() == 0)
            {
                _context.climate.Add(new weather("chennai", "36°", "3 %", "2%","3km/h"));
                _context.SaveChanges();
            }
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<weather>>> Getclimate()
        {
            return await _context.climate.ToListAsync();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{city}")]
        public async Task<ActionResult<weather>> Getcli(string city)
        {
            var season = await _context.climate.FindAsync(city);

            if (season == null)
            {
                return NotFound();
            }

            return season;
        }


        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<weather>> Postcli(weather weather)
        {
            _context.climate.Add(weather);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getcity", new { city = weather.City }, weather);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{city}")]
        public async Task<IActionResult> Putcli(string city, weather weather)
        {
            if (city != weather.City)
            {
                return BadRequest();
            }

            _context.Entry(weather).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!weatherExists(city))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // DELETE api/<ValuesController>/5
        [HttpDelete("{city}")]
        public async Task<IActionResult> Deletecli(string city)
        {
            var season = await _context.climate.FindAsync(city);
            if (season == null)
            {
                return NotFound();
            }

            _context.climate.Remove(season);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool weatherExists(string city)
        {
            return _context.climate.Any(e => e.City == city);
        }
        [HttpPatch("{city}")]
        public IActionResult Patch(string city, [FromBody] JsonPatchDocument<weather> patchEntity)
        {
            var entity = _context.climate.FirstOrDefault(season => season.City == city);

            if (entity == null)
            {
                return NotFound();
            }

            patchEntity.ApplyTo(entity, ModelState); // Must have Microsoft.AspNetCore.Mvc.NewtonsoftJson installed

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Update(entity); //Update in the database.

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!weatherExists(city))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(entity);
        }
       
    }
}
