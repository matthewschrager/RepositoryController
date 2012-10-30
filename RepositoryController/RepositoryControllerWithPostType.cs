using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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
    public abstract class RepositoryControllerWithPostType<TValue, TPost, TKey> : ApiController where TValue : class
    {
        //===============================================================
        protected abstract IRepository<TValue> Repository { get; }
        //===============================================================
        public virtual HttpResponseMessage Get()
        {
            try
            {
                using (var objects = Repository.GetItemsContext())
                {
                    return CreateSuccessResponse(HttpStatusCode.OK, objects.Objects);
                }
            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        public virtual HttpResponseMessage Get(TKey id)
        {

            try
            {
                using (var obj = Repository.Find(id))
                {
                    if (obj.Object == null)
                        return CreateFailureResponse(HttpStatusCode.NotFound, "Could not find object with key " + id);

                    return CreateSuccessResponse(HttpStatusCode.OK, obj.Object);
                }
            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        public virtual HttpResponseMessage Post(TPost obj)
        {
            try
            {
                Repository.Store(PostedObjectToStoredObject(obj));
                return Request.CreateResponse(HttpStatusCode.OK);

            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        [AcceptVerbs("PATCH")]
        public virtual HttpResponseMessage Patch(TKey id, PatchArguments args)
        {
            if (id == null)
                return CreateFailureResponse(HttpStatusCode.BadRequest, "You must specify the key for the object to update.");

            if (args.UpdateType == null)
                args.UpdateType = "set";

            try
            {
                Repository.Update(args.PathToProperty, args.UpdateDescriptor, Utility.ToUpdateType(args.UpdateType), id);
                return CreateSuccessResponse(HttpStatusCode.OK);

            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        public virtual HttpResponseMessage Delete(TKey key)
        {
            try
            {
                Repository.Remove(key);
                return CreateSuccessResponse(HttpStatusCode.OK);
            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        protected virtual HttpResponseMessage CreateSuccessResponse(HttpStatusCode statusCode, Object responseObj = null)
        {
            return Request.CreateResponse(statusCode, responseObj);
        }
        //===============================================================
        protected virtual HttpResponseMessage CreateFailureResponse(HttpStatusCode statusCode, String errorMessage, Object responseObj = null)
        {
            return Request.CreateResponse(statusCode, responseObj);
        }
        //===============================================================
        protected abstract TValue PostedObjectToStoredObject(TPost postedObj);
        //===============================================================
    }

    public abstract class RepositoryController<TValue, TPost, TKey1, TKey2> : ApiController where TValue : class
    {
        //===============================================================
        protected abstract IRepository<TValue> Repository { get; }
        //===============================================================
        public virtual HttpResponseMessage Get()
        {
            try
            {
                using (var objects = Repository.GetItemsContext())
                {
                    return CreateSuccessResponse(HttpStatusCode.OK, objects.Objects);
                }
            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        public virtual HttpResponseMessage Get(TKey1 key1, TKey2 key2)
        {

            try
            {
                using (var obj = Repository.Find(key1, key2))
                {
                    if (obj.Object == null)
                        return CreateFailureResponse(HttpStatusCode.NotFound, "Could not find object with keys {" + key1 + ", " + key2 + " }.");

                    return CreateSuccessResponse(HttpStatusCode.OK, obj.Object);
                }
            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        public virtual HttpResponseMessage Post(TPost obj)
        {
            try
            {
                Repository.Store(PostedObjectToStoredObject(obj));
                return Request.CreateResponse(HttpStatusCode.OK);

            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        [AcceptVerbs("PATCH")]
        public virtual HttpResponseMessage Patch(TKey1 key1, TKey2 key2, PatchArguments args)
        {
            if (key1 == null || key2 == null)
                return CreateFailureResponse(HttpStatusCode.BadRequest, "You must specify the keys for the object to update.");

            if (args.UpdateType == null)
                args.UpdateType = "set";

            try
            {
                Repository.Update(args.PathToProperty, args.UpdateDescriptor, Utility.ToUpdateType(args.UpdateType), key1, key2);
                return CreateSuccessResponse(HttpStatusCode.OK);

            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        public virtual HttpResponseMessage Delete(TKey1 key1, TKey2 key2)
        {
            try
            {
                Repository.Remove(key1, key2);
                return CreateSuccessResponse(HttpStatusCode.OK);
            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        protected virtual HttpResponseMessage CreateSuccessResponse(HttpStatusCode statusCode, Object responseObj = null)
        {
            return Request.CreateResponse(statusCode, responseObj);
        }
        //===============================================================
        protected virtual HttpResponseMessage CreateFailureResponse(HttpStatusCode statusCode, String errorMessage, Object responseObj = null)
        {
            return Request.CreateResponse(statusCode, responseObj);
        }
        //===============================================================
        protected abstract TValue PostedObjectToStoredObject(TPost postedObj);
        //===============================================================
    }
}
