using API.Context;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class GeneralRepository<Context, Entity, Key> : IRepository<Entity, Key>
        where Entity : class
        where Context : MyContext
    {
        private readonly MyContext myContext;
        private readonly DbSet<Entity> entities;

        public GeneralRepository(MyContext myContext)
        {
            this.myContext = myContext;
            entities = myContext.Set<Entity>();
        }

        public IEnumerable<Entity> Get()
        {
            return entities.ToList();
        }

        public Entity Get(Key key)
        {
            var entity = entities.Find(key);
            return entity;
        }

        public int Insert(Entity entity)
        {
            try
            {
                entities.Add(entity);
                var result = myContext.SaveChanges();
                return result;
            }
            catch (Exception)
            {
                return 0;
            }  
        }

        public int Update(Entity entity, Key key)
        {
            var search = entities.Find(key);
            myContext.Entry(search).CurrentValues.SetValues(entity);
            var result = myContext.SaveChanges();
            return result;
        }

        public virtual int Delete(Key key)
        {
            var entity = entities.Find(key);
            myContext.Remove(entity);
            var result = myContext.SaveChanges();
            return result;
        }
    }
}
