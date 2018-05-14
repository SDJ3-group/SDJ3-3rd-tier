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
    public class PalletsController : ApiController
    {
        private FactoryContext db = new FactoryContext();

        private static readonly Expression<Func<Pallet, PalletDto>> AsPalleteDto =
            x => new PalletDto
            {
                Id = x.Id,
                MaximumCapacity = x.MaximumCapacity
            };

        private static readonly Expression<Func<PalletDto, Pallet>> AsPallete =
            x => new Pallet
            {
                Id = x.Id,
                MaximumCapacity = x.MaximumCapacity
            };

        // GET: api/Pallets
        public IQueryable<PalletDto> GetPallets()
        {
            return db.Pallets.Select(AsPalleteDto);
        }

        // GET: api/Pallets/5
        [ResponseType(typeof(PalletDto))]
        public async Task<IHttpActionResult> GetPallet(int id)
        {
            PalletDto pallet = await db.Pallets.Where(p => p.Id == id).Select(AsPalleteDto).FirstOrDefaultAsync();

            if (pallet == null)
            {
                return NotFound();
            }

            return Ok(pallet);
        }

        // PUT: api/Pallets/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPallet(int id, PalletDto pallet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pallet.Id)
            {
                return BadRequest();
            }

            Pallet oldPallet = await db.Pallets.FindAsync(id);

            if (oldPallet == null)
            {
                return NotFound();
            }

            db.Entry(this.MergePalletDtoToPaller(pallet, oldPallet)).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PalletExists(id))
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

        // POST: api/Pallets
        [ResponseType(typeof(PalletDto))]
        public async Task<IHttpActionResult> PostPallet(PalletDto pallet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pallets.Add(this.PalletFromPalletDto(pallet));
            await db.SaveChangesAsync();

            return Created("", "");
            //return CreatedAtRoute("DefaultApi", new { id = pallet.Id }, pallet);
        }

        // DELETE: api/Pallets/5
        [ResponseType(typeof(PalletDto))]
        public async Task<IHttpActionResult> DeletePallet(int id)
        {
            Pallet pallet = await db.Pallets.FindAsync(id);
            if (pallet == null)
            {
                return NotFound();
            }

            db.Pallets.Remove(pallet);
            await db.SaveChangesAsync();

            return Ok(this.PalletDtoFromPallet(pallet));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PalletExists(int id)
        {
            return db.Pallets.Count(e => e.Id == id) > 0;
        }

        private PalletDto PalletDtoFromPallet(Pallet pallet)
        {
            return new PalletDto
            {
                Id = pallet.Id,
                MaximumCapacity = pallet.MaximumCapacity
            };
        }

        private Pallet PalletFromPalletDto(PalletDto pallet)
        {
            return new Pallet
            {
                Id = pallet.Id,
                MaximumCapacity = pallet.MaximumCapacity
            };
        }

        private Pallet MergePalletDtoToPaller(PalletDto palletDto, Pallet pallet)
        {
            pallet.Id = palletDto.Id;
            pallet.MaximumCapacity = palletDto.MaximumCapacity;

            return pallet;
        }
    }
}