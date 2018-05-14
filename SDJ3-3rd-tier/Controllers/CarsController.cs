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
    public class CarsController : ApiController
    {
        private FactoryContext db = new FactoryContext();

        private static readonly Expression<Func<Car, CarDto>> AsCarDto =
            x => new CarDto
            {
                VIN = x.VIN,
                Model = x.Model,
                Weight = x.Weight
            };

        private static readonly Expression<Func<CarDto, Car>> AsCar =
            x => new Car
            {
                VIN = x.VIN,
                Model = x.Model,
                Weight = x.Weight
            };


        // GET: api/Cars
        public IQueryable<CarDto> GetCars()
        {
            return db.Cars.Select(AsCarDto);
        }

        // GET: api/Cars/5
        [ResponseType(typeof(CarDto))]
        public async Task<IHttpActionResult> GetCar(string id)
        {
            CarDto car = await db.Cars.Where(c => c.VIN == id).Select(AsCarDto).FirstOrDefaultAsync();

            if (car == null)
            {
                return NotFound();
            }

            return Ok(car);
        }

        // PUT: api/Cars/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCar(string id, CarDto car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != car.VIN)
            {
                return BadRequest();
            }

            Car oldCar = await db.Cars.Where(c => c.VIN == id).FirstOrDefaultAsync();

            if (oldCar == null)
            {
                return NotFound();
            }

            db.Entry(this.MergeCarDtoToCar(car, oldCar)).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
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

        // POST: api/Cars
        [ResponseType(typeof(CarDto))]
        public async Task<IHttpActionResult> PostCar(CarDto car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cars.Add(this.CarFromCarDto(car));

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CarExists(car.VIN))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created("", "");
            //return CreatedAtRoute("DefaultApi", new { id = car.VIN }, car);
        }

        // DELETE: api/Cars/5
        [ResponseType(typeof(CarDto))]
        public async Task<IHttpActionResult> DeleteCar(string id)
        {
            Car car = await db.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            db.Cars.Remove(car);
            await db.SaveChangesAsync();

            return Ok(this.CarDtoFromCar(car));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarExists(string id)
        {
            return db.Cars.Count(e => e.VIN == id) > 0;
        }


        private CarDto CarDtoFromCar(Car car)
        {
            return new CarDto
            {
                VIN = car.VIN,
                Model = car.Model,
                Weight = car.Weight
            };
        }

        private Car CarFromCarDto(CarDto car)
        {
            return new Car
            {
                VIN = car.VIN,
                Model = car.Model,
                Weight = car.Weight
            };
        }

        private Car MergeCarDtoToCar(CarDto carDto, Car car)
        {
            car.VIN = carDto.VIN;
            car.Model = carDto.Model;
            car.Weight = carDto.Weight;

            return car;
        }
    }
}