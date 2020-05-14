using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordLabelApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecordLabelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReleaseController : ControllerBase
    {
        private readonly Db _db;

        public ReleaseController(Db db)
        {
            _db = db;
        }

        // GET: api/Release
        [EnableCors("CustomPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Release>>> GetReleases()
        {
            return await _db.Releases.OrderBy(r => r.CatalogNr.Length).ThenBy(r => r.CatalogNr).ToListAsync();
        }


        // GET: api/Release/5
        [EnableCors("CustomPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Release>> GetRelease(int id)
        {
            var release = await _db.Releases.FindAsync(id);

            if (release == null)
            {
                return NotFound();
            }

            return release;
        }

        // PUT: api/Release/5
        [EnableCors("CustomPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelease(int id, Release release)
        {
            //Funkar men behöver fixas till!!!!!!!
            
            var currentRelease = await _db.Releases.FirstOrDefaultAsync(r => r.Id == id);
            if (currentRelease == null)
            {
                return BadRequest(new { message = string.Format("Something went wrong. Release with id {0} doesn't exist.", release.Id) });
            }

            if(await _db.Releases.CountAsync(r => r.CatalogNr == release.CatalogNr && r.Id != release.Id) > 0) {
                return BadRequest(new { message = "Catalognumber already exist for another release" });
            }

            //Validate quantity containing only numbers
            if (!int.TryParse(release.Quantity, out int quantity))
            {
                return BadRequest(new { message = "Quantity must contain only numbers 0-9", release });
            }



            var catNumber = release.CatalogNr;

            currentRelease.Artist = release.Artist;
            currentRelease.Title = release.Title;
            currentRelease.Description = release.Description;
            currentRelease.Format = release.Format;
            currentRelease.ReleaseDate = release.ReleaseDate;
            currentRelease.CatalogNr = release.CatalogNr;
            currentRelease.Quantity = release.Quantity;
            currentRelease.Price = release.Price;
            currentRelease.FrontCover = release.FrontCover;

            await _db.SaveChangesAsync();


            return Ok(release);

        }

        // POST: api/Release
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [EnableCors("CustomPolicy")]
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<Release>> PostRelease(Release release)
        {
            

            //Validate that CatalogNumber doesn't already exist
            if (await _db.Releases.CountAsync(r => r.CatalogNr == release.CatalogNr && r.Id != release.Id) > 0)
            {
                return BadRequest(new { message = "Catalognumber already exist for another release" });
            }

            //Validate quantity containing only numbers
            if (!int.TryParse(release.Quantity, out int quantity))
            {
                return BadRequest(new { message = "Quantity must contain only numbers 0-9" });
            }

            if (release.ReleaseDate == null || !DateTime.TryParse(release.ReleaseDate.ToString(), out DateTime result)) {
                return BadRequest(new { message = "Please enter a correct date" });
            }





            _db.Releases.Add(release);
            await _db.SaveChangesAsync();

            return Ok(new { message = string.Format("{0} - {1} was added to the database", release.Artist, release.Title), release });
        }

        // DELETE: api/Release/5
        [EnableCors("CustomPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Release>> DeleteRelease(int id)
        {
            var release = await _db.Releases.FindAsync(id);
            if (release == null)
            {
                return NotFound(new { message = string.Format("Could not delete release with id: {0}", release.Id) });
            }

            _db.Releases.Remove(release);
            await _db.SaveChangesAsync();


            return Ok();
        }

        private bool ReleaseExists(int id)
        {
            return _db.Releases.Any(e => e.Id == id);
        }
    }
}
