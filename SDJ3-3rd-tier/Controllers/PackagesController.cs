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
    public class PackagesController : ApiController
    {
        private FactoryContext db = new FactoryContext();

        private static readonly Expression<Func<Package, PackageDto>> AsPackageDto =
            x => new PackageDto
            {
                Id = x.Id,
                Repacking = x.Repacking,
                Content = x.Content
            };

        private static readonly Expression<Func<PackageDto, Package>> AsPackage =
            x => new Package
            {
                Id = x.Id,
                Repacking = x.Repacking,
                Content = x.Content
            };

        // GET: api/Packages
        public IQueryable<PackageDto> GetPackages()
        {
            return db.Packages.Select(AsPackageDto);
        }

        // GET: api/Packages/5
        [ResponseType(typeof(PackageDto))]
        public async Task<IHttpActionResult> GetPackage(int id)
        {
            PackageDto package = await db.Packages.Where(p => p.Id == id).Select(AsPackageDto).FirstOrDefaultAsync();
            if (package == null)
            {
                return NotFound();
            }

            return Ok(package);
        }

        // PUT: api/Packages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPackage(int id, PackageDto package)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != package.Id)
            {
                return BadRequest();
            }

            Package oldPackage = await db.Packages.Where(c => c.Id == id).FirstOrDefaultAsync();


            db.Entry(this.MergePackageDtoToPackage(package, oldPackage)).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PackageExists(id))
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

        // POST: api/Packages
        [ResponseType(typeof(PackageDto))]
        public async Task<IHttpActionResult> PostPackage(PackageDto package)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Packages.Add(this.PackageFromPackageDto(package));
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = package.Id }, package);
        }

        // DELETE: api/Packages/5
        [ResponseType(typeof(PackageDto))]
        public async Task<IHttpActionResult> DeletePackage(int id)
        {
            Package package = await db.Packages.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }

            db.Packages.Remove(package);
            await db.SaveChangesAsync();

            return Ok(this.PackageDtoFromPackage(package));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PackageExists(int id)
        {
            return db.Packages.Count(e => e.Id == id) > 0;
        }

        private PackageDto PackageDtoFromPackage(Package package)
        {
            return new PackageDto
            {
                Id = package.Id,
                Content = package.Content,
                Repacking = package.Repacking
            };
        }

        private Package PackageFromPackageDto(PackageDto package)
        {
            return new Package
            {
                Id = package.Id,
                Content = package.Content,
                Repacking = package.Repacking
            };
        }

        private Package MergePackageDtoToPackage(PackageDto packageDto, Package package)
        {
            package.Id = packageDto.Id;
            package.Content = packageDto.Content;

            return package;
        }
    }
}