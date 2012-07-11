using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using Repository;

namespace RepositoryController
{
    /// <summary>
    /// A controller that provides generic CRUD capabilities for working with an IRepository.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class RepositoryController<TValue, TKey> : ApiController where TValue : class
    {
        //===============================================================
        protected abstract IRepository<TValue> Repository { get; }
        //===============================================================
        [RequireAuthorization]
        public virtual HttpResponseMessage<IQueryable<TValue>> Get()
        {
            return new HttpResponseMessage<IQueryable<TValue>>(Repository.GetItemsContext().Objects);
        }
        //===============================================================
        [RequireAuthorization]
        public virtual HttpResponseMessage<TValue> Get(TKey id)
        {
            using (var obj = Repository.Find(id))
            {
                if (obj.Object == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                return new HttpResponseMessage<TValue>(obj.Object);
            }
        }
        //===============================================================
        [RequireAuthorization]
        public virtual HttpResponseMessage<String> Post(TValue obj)
        {
            Repository.Store(obj);
            return new HttpResponseMessage<String>(JsonConvert.SerializeObject(obj));
        }
        //===============================================================
        [RequireAuthorization, AcceptVerbs("PATCH")]
        public virtual void Patch(PatchArguments<TKey> args)
        {
            if (args.Key == null)
                throw new ArgumentException("You must specify a key for the object to update.");

            if (args.UpdateType == null)
                args.UpdateType = "set";

            Repository.Update(args.PathToProperty, args.UpdateDescriptor, Utility.ToUpdateType(args.UpdateType), args.Key);
        }
        //===============================================================
        [RequireAuthorization]
        public virtual void Delete(TKey key)
        {
            Repository.Remove(key);
        }
        //===============================================================
    }

    public abstract class RepositoryController<TValue, TKey1, TKey2> : ApiController where TValue : class
    {
        //===============================================================
        protected abstract IRepository<TValue> Repository { get; }
        //===============================================================
        [RequireAuthorization]
        public virtual HttpResponseMessage<IQueryable<TValue>> Get()
        {
            using (var obj = Repository.GetItemsContext())
            {
                return new HttpResponseMessage<IQueryable<TValue>>(obj.Objects);
            }
        }
        //===============================================================
        [RequireAuthorization]
        public virtual HttpResponseMessage<TValue> Get(TKey1 key1, TKey2 key2)
        {
            using (var obj = Repository.Find(key1, key2))
            {
                return new HttpResponseMessage<TValue>(obj.Object);
            }
        }
        //===============================================================
        public virtual HttpResponseMessage<TValue> Post(TValue obj)
        {
            Repository.Store(obj);
            return new HttpResponseMessage<TValue>(obj);
        }
        //===============================================================
        [RequireAuthorization]
        public virtual void Delete(TKey1 key1, TKey2 key2)
        {
            Repository.Remove(key2);
        }
        //===============================================================
        [RequireAuthorization, AcceptVerbs("PATCH")]
        public virtual void Patch(PatchArguments<TKey1, TKey2> args)
        {
            if (args.Key1 == null || args.Key2 == null)
                throw new ArgumentException("You must specify both keys for the object to update.");

            if (args.UpdateType == null)
                args.UpdateType = "set";

            Repository.Update(args.PathToProperty, args.UpdateDescriptor, Utility.ToUpdateType(args.UpdateType), args.Key1, args.Key2);
        }
        //===============================================================
    }
}
