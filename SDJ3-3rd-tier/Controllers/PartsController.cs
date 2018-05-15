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

        // Get: api/Cars/2/Parts/
        [HttpGet]
        [Route("api/Cars/{id}/Parts")]
        public IQueryable<PartDto> GetPackageParts(String id)
        {
            return db.Parts.Select(AsPartDto).Where(p => p.CarId == id);
        }

        // POST: api/Cars/2/Parts
        [Route("api/Cars/{id}/Parts")]
        [HttpPost]
        [ResponseType(typeof(PartDto))]
        public async Task<IHttpActionResult> PostPart(String id, PartDto partDto)
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

            part = db.Parts.Add(part);
            await db.SaveChangesAsync();

            return Created("", "");
            //return CreatedAtRoute("DefaultApi", new { id = part.Id }, partDto);
        }

        // PUT: api/Cars/2/Parts/5
        [ResponseType(typeof(void))]
        [Route("api/Cars/{id}/Parts/{partId:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutPart(String id, int partId, PartDto part)
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

        // Get: api/Packages/2/Parts/
        [HttpGet]
        [Route("api/Packages/{id:int}/Parts")]
        public IQueryable<PartDto> GetPackageParts(int id)
        {
            return db.Parts.Select(AsPartDto).Where(p => p.PackageId == id);
        }

        // PUT: api/Packages/2/Parts/
        [ResponseType(typeof(void))]
        [Route("api/Packages/{id:int}/Parts")]
        [HttpPut]
        public async Task<IHttpActionResult> PutPartToPackage(int id, PackageParts packageParts)
        {
            Package package = await db.Packages.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parts = db.Parts.Where(p => packageParts.Parts.Contains(p.Id));


            string Name = null;

            foreach (Part part in parts)
            {
                if (String.IsNullOrEmpty(Name))
                {
                    Name = package.Repacking ? part.Name : part.Car.Model;
                }

                if (!Name.Equals(package.Repacking ? part.Name : part.Car.Model)) {
                    return BadRequest("Different part");
                }
            }

            foreach (Part part in parts)
            {
                part.Package = package;
                part.PreviousPallet = part.Pallet;
                part.Pallet = null;

                db.Entry(part).State = EntityState.Modified;   
            }

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

        // Get: api/Pallete/2/Parts/
        [HttpGet]
        [Route("api/Pallete/{id:int}/Parts")]
        public IQueryable<PartDto> GetPalleteParts(int id)
        {
            return db.Parts.Select(AsPartDto).Where(p => p.PalletId == id);
        }

        // PUT: api/Pallete/2/Parts/
        [ResponseType(typeof(void))]
        [Route("api/Pallete/{id:int}/Parts")]
        [HttpPut]
        public async Task<IHttpActionResult> PutPartToPallete(int id, PackageParts packageParts)
        {
            Pallet pallet = await db.Pallets.FindAsync(id);
            if (pallet == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parts = db.Parts.Where(p => packageParts.Parts.Contains(p.Id));


            string Name = null;
            double Weight = 0;

            foreach (Part part in parts)
            {
                if (String.IsNullOrEmpty(Name))
                {
                    Name = part.Name;
                }

                if (!Name.Equals(part.Name))
                {
                    return BadRequest("Different part");
                }
                Weight += part.Weight;
            }

            if (Weight > pallet.MaximumCapacity)
            {
                return BadRequest("Exceeded maximum weight");
            }

            foreach (Part part in parts)
            {
                part.Pallet = pallet;

                db.Entry(part).State = EntityState.Modified;
            }

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