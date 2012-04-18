using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OAuth2DotNet;
using Repository;

namespace RepositoryController.OAuth
{
    /// <summary>
    /// A controller that provides generic CRUD capabilities for working with an IRepository.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class OAuthRepositoryController<TValue, TKey> : RepositoryController<TValue, TKey> where TValue : class
    {
        //===============================================================
        [OAuthorize]
        public override HttpResponseMessage<IQueryable<TValue>> Get()
        {
            return new HttpResponseMessage<IQueryable<TValue>>(Repository.GetItemsContext().Object.AsQueryable());
        }
        //===============================================================
        [OAuthorize]
        public override HttpResponseMessage<TValue> Get(TKey id)
        {
            using (var obj = Repository.Find(id))
            {
                if (obj.Object == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                return new HttpResponseMessage<TValue>(obj.Object);
            }
        }
        //===============================================================
        [OAuthorize]
        public override HttpResponseMessage<TValue> Put(TValue obj)
        {
            Repository.Store(obj);
            return new HttpResponseMessage<TValue>(obj);
        }
        //===============================================================
        [OAuthorize]
        public override void Delete(TKey key)
        {
            Repository.Remove(key);
        }
        //===============================================================
    }

    public abstract class OAuthRepositoryController<TValue, TKey1, TKey2> : RepositoryController<TValue, TKey1, TKey2> where TValue : class
    {
        //===============================================================
        [OAuthorize]
        public override HttpResponseMessage<IQueryable<TValue>> Get()
        {
            using (var obj = Repository.GetItemsContext())
            {
                return new HttpResponseMessage<IQueryable<TValue>>(obj.Object);
            }
        }
        //===============================================================
        [OAuthorize]
        public override HttpResponseMessage<TValue> Get(TKey1 key1, TKey2 key2)
        {
            using (var obj = Repository.Find(key1, key2))
            {
                return new HttpResponseMessage<TValue>(obj.Object);
            }
        }
        //===============================================================
        [OAuthorize]
        public override HttpResponseMessage<TValue> Put(TValue obj)
        {
            Repository.Store(obj);
            return new HttpResponseMessage<TValue>(obj);
        }
        //===============================================================
        [OAuthorize]
        public override void Delete(TKey1 key1, TKey2 key2)
        {
            Repository.Remove(key2);
        }
        //===============================================================
    }
}
