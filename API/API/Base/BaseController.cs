using API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;
        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult<Entity> Get()
        {
            var result = repository.Get();
            /*if (result.Count() == 0)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data kosong"});
            }
            else
            {
                return Ok(new { status = HttpStatusCode.OK, message = "Data ditemukan", result });
            }*/
            return Ok(result);
        }

        [HttpPost]
        public ActionResult Insert(Entity entity)
        {
            var result = repository.Insert(entity);
            if (result == 0)
            {
                return Ok(new { status = HttpStatusCode.InternalServerError, message = "Data tidak boleh kosong"});
            }
            else
            {
                return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditambahkan"});
            }
        }

        [HttpGet("{key}")]
        public ActionResult Get(Key key)
        {
            var result = repository.Get(key);
            if (result == null)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak tersedia" });
            }
            return Ok(result);
        }

        [HttpPut("{key}")]
        public ActionResult Update(Entity entity, Key key)
        {
            if (repository.Get(key) == null)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan" });
            }
            else
            {
                var result = repository.Update(entity, key);
                return Ok(new { status = HttpStatusCode.OK, message = "Berhasil mengubah data" });
            }
        }

        [HttpDelete("{key}")]
        public ActionResult Delete(Key key)
        {
            if (repository.Get(key) == null)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan" });
            }
            else
            {
                repository.Delete(key);
                Console.WriteLine();
                return Ok(new { status = HttpStatusCode.OK, message = "Berhasil Menghapus data" });
            }
        }
    }
}
