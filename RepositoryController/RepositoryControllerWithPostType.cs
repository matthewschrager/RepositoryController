﻿using System;
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
            return Request.CreateResponse(HttpStatusCode.OK, Repository.GetItemsContext().Objects);
        }
        //===============================================================
        public virtual HttpResponseMessage Get(TKey id)
        {
            using (var obj = Repository.Find(id))
            {
                if (obj.Object == null)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

                return Request.CreateResponse(HttpStatusCode.OK, obj.Object);
            }
        }
        //===============================================================
        protected abstract TValue PostedObjectToStoredObject(TPost postedObj);
        //===============================================================
        public virtual HttpResponseMessage Post(TPost obj)
        {
            TValue storedObj = null;
            try
            {
                storedObj = PostedObjectToStoredObject(obj);
            }

            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Could not create " + typeof(TValue).Name + ". Error: " + e.Message);
            }
           
            Repository.Store(storedObj);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        //===============================================================
        [AcceptVerbs("PATCH")]
        public virtual HttpResponseMessage Patch(PatchArguments<TKey> args)
        {
            if (args.Key == null)
                Request.CreateResponse(HttpStatusCode.BadRequest, "You must specify the key for the object to update.");

            if (args.UpdateType == null)
                args.UpdateType = "set";

            Repository.Update(args.PathToProperty, args.UpdateDescriptor, Utility.ToUpdateType(args.UpdateType), args.Key);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        //===============================================================
        public virtual HttpResponseMessage Delete(TKey key)
        {
            Repository.Remove(key);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        //===============================================================
    }

    public abstract class RepositoryController<TValue, TPost, TKey1, TKey2> : ApiController where TValue : class
    {
        //===============================================================
        protected abstract IRepository<TValue> Repository { get; }
        //===============================================================
        public virtual HttpResponseMessage Get()
        {
            using (var obj = Repository.GetItemsContext())
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj.Objects);
            }
        }
        //===============================================================
        public virtual HttpResponseMessage Get(TKey1 key1, TKey2 key2)
        {
            using (var obj = Repository.Find(key1, key2))
            {
                if (obj.Object == null)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

                return Request.CreateResponse(HttpStatusCode.OK, obj.Object);
            }
        }
        //===============================================================
        public abstract TValue PostedObjectToStoredObject(TPost postedObj);
        //===============================================================
        public virtual HttpResponseMessage Post(TPost obj)
        {
            TValue storedObj = null;
            try
            {
                storedObj = PostedObjectToStoredObject(obj);
            }

            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Could not create " + typeof(TValue).Name + ". Error: " + e.Message);
            }

            Repository.Store(storedObj);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        //===============================================================
        public virtual HttpResponseMessage Delete(TKey1 key1, TKey2 key2)
        {
            Repository.Remove(key2);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        //===============================================================
        [AcceptVerbs("PATCH")]
        public virtual HttpResponseMessage Patch(PatchArguments<TKey1, TKey2> args)
        {
            if (args.Key1 == null || args.Key2 == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "You must specify both keys for the object to update.");

            if (args.UpdateType == null)
                args.UpdateType = "set";

            Repository.Update(args.PathToProperty, args.UpdateDescriptor, Utility.ToUpdateType(args.UpdateType), args.Key1, args.Key2);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        //===============================================================
    }
}
