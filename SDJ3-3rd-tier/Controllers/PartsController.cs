using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SDJ3_3rd_tier.DAL;
using SDJ3_3rd_tier.Models;
using SDJ3_3rd_tier.Models.DTOs;

namespace SDJ3_3rd_tier.Controllers
{
    public class PartsController : ApiController
    {
        private FactoryContext db = new FactoryContext();

        private static readonly Expression<Func<Part, PartDto>> AsPartDto =
            x => new PartDto
            {
                Id = x.Id,
                Name = x.Name,
                Weight = x.Weight,
                CarId = x.CarId,
                PalletId = x.PalletId,
                PreviusPalletId = x.PreviusPalletId,
                PackageId = x.PackageId
            };

        private static readonly Expression<Func<PartDto, Part>> AsPart =
            x => new Part
            {
                Id = x.Id,
                Name = x.Name,
                Weight = x.Weight,
                CarId = x.CarId,
                PalletId = x.PalletId,
                PreviusPalletId = x.PreviusPalletId,
                PackageId = x.PackageId
            };


        // GET: api/Parts
        public IQueryable<PartDto> GetParts()
        {
            return db.Parts.Select(AsPartDto);
        }

        // GET: api/Parts/5
        [ResponseType(typeof(PartDto))]
        public async Task<IHttpActionResult> GetPart(int id)
        {
            PartDto part = await db.Parts.Where(p => p.Id == id).Select(AsPartDto).FirstOrDefaultAsync();
            if (part == null)
            {
                return NotFound();
            }

            return Ok(part);
        }

        // PUT: api/Cars/2/Parts/5
        [ResponseType(typeof(void))]
        [Route("api/Cars/{id:int}/Parts/{partId:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutPart(int id, int partId, PartDto part)
        {
            Car car = await db.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!id.Equals(part.CarId))
            {
                return BadRequest();
            }

            if (partId != part.Id)
            {
                return BadRequest();
            }

            Part oldPart = await db.Parts.Where(c => c.Id == partId).FirstOrDefaultAsync();

            if (oldPart == null)
            {
                return NotFound();
            }

            db.Entry(this.MergePartDtoToPart(part, oldPart)).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Cars/2/Parts
        [Route("api/Cars/{id:string}/Parts")]
        [HttpPost]
        [ResponseType(typeof(PartDto))]
        public async Task<IHttpActionResult> PostPart(int id, PartDto partDto)
        {
            Car car = await db.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Part part = this.PartFromPartDto(partDto);
            part.Car = car;        

            db.Parts.Add(part);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = part.Id }, partDto);
        }

        // DELETE: api/Parts/5
        [ResponseType(typeof(PartDto))]
        public async Task<IHttpActionResult> DeletePart(int id)
        {
            Part part = await db.Parts.FindAsync(id);
            if (part == null)
            {
                return NotFound();
            }

            db.Parts.Remove(part);
            await db.SaveChangesAsync();

            return Ok(this.PartDtoFromPart(part));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PartExists(int id)
        {
            return db.Parts.Count(e => e.Id == id) > 0;
        }

        private PartDto PartDtoFromPart(Part part)
        {
            return new PartDto
            {
                Id = part.Id,
                Name = part.Name,
                Weight = part.Weight,
                CarId = part.CarId,
                PackageId = part.PackageId,
                PalletId = part.PalletId,
                PreviusPalletId = part.PreviusPalletId
                
            };
        }

        private Part PartFromPartDto(PartDto part)
        {
            return new Part
            {
                Id = part.Id,
                Name = part.Name,
                Weight = part.Weight,
                CarId = part.CarId,
                PackageId = part.PackageId,
                PalletId = part.PalletId,
                PreviusPalletId = part.PreviusPalletId
            };
        }

        private Part MergePartDtoToPart(PartDto partDto, Part part)
        {
            part.Id = partDto.Id;
            part.Name = partDto.Name;
            part.Weight = partDto.Weight;

            return part;
        }
    }
}