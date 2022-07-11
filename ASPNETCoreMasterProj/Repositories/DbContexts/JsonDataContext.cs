using System.Collections.Generic;
using System.IO;
using System.Linq;
using DomainModels.Entity;
using Newtonsoft.Json;
using Repositories.Interface;

namespace Repositories.DbContexts
{
    public sealed class JsonDataContext : IDataContext
    {
        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : EntityBase
        {
            using var r = new StreamReader(GetFile<TEntity>());
            var json = r.ReadToEnd();

            //TODO: Null handling

            return JsonConvert.DeserializeObject<List<TEntity>>(json).AsQueryable();
        }

        public TEntity GetById<TEntity>(int id) where TEntity : EntityBase
        {
            using var r = new StreamReader(GetFile<TEntity>());
            var json = r.ReadToEnd();

            //TODO: Null handling

            var collection = JsonConvert.DeserializeObject<List<TEntity>>(json).AsQueryable();

            return collection.FirstOrDefault(_ => _.Id == id);
        }

        public void AddEntity<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            var collection = GetAll<TEntity>().ToList();

            entity.Id = collection.Any() ? collection.Max(_ => _.Id + 1) : 1;

            collection.Add(entity);

            File.WriteAllText(GetFile<TEntity>(), JsonConvert.SerializeObject(collection));
        }

        public void DeleteEntity<TEntity>(int id) where TEntity : EntityBase
        {
            var collection = GetAll<TEntity>().ToList();
            var item = collection.FirstOrDefault(_ => _.Id == id);

            collection.Remove(item);

            File.WriteAllText(GetFile<TEntity>(), JsonConvert.SerializeObject(collection));
        }

        public void UpdateEntity<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            var collection = GetAll<TEntity>().ToList();
            var item = collection.FirstOrDefault(_ => _.Id == entity.Id);

            collection.Remove(item);
            collection.Add(entity);

            File.WriteAllText(GetFile<TEntity>(), JsonConvert.SerializeObject(collection));
        }

        private static string GetFile<TEntity>()
        {
            var filename = @$"..\Repositories\JsonFiles\{typeof(TEntity).Name}.json";

            if (!File.Exists(filename))
            {
                var filestream = File.Create(filename);
                filestream.Close();

                File.WriteAllText(filename, JsonConvert.SerializeObject(new List<TEntity>()));
            }

            return filename;
        }

        public void UpdateAllEntity<TEntity>(IEnumerable<TEntity> entity) where TEntity : EntityBase
        {
            var collection = GetAll<TEntity>().ToList();

            foreach(var item in entity)
            {
                var result = collection.FirstOrDefault(_ => _.Id == item.Id);

                collection.Remove(result);
                collection.Add(item);
            }           

            File.WriteAllText(GetFile<TEntity>(), JsonConvert.SerializeObject(collection));
        }
    }
}
