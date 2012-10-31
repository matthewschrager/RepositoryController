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
    public abstract class RepositoryController<TValue> : RepositoryControllerBase<TValue, TValue> where TValue : class
    {
        //===============================================================
        protected sealed override TValue ConvertToExternalValue(TValue value)
        {
            return value;
        }
        //===============================================================
        protected sealed override TValue ConvertToStoredValue(TValue value)
        {
            return value;
        }
        //===============================================================
        protected override string PathToDataProperty
        {
            get { return String.Empty; }
        }
        //===============================================================
    }

    /// <summary>
    /// A controller that provides generic CRUD capabilities for working with an IRepository.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class RepositoryController<TValue, TKey> : RepositoryController<TValue> where TValue : class
    {
        //===============================================================
        public virtual HttpResponseMessage Get(TKey id)
        {
            return GetImpl(id);
        }
        //===============================================================
        [AcceptVerbs("PATCH")]
        public virtual HttpResponseMessage Patch(TKey id, PatchArguments args)
        {
            return PatchImpl(args, id);
        }
        //===============================================================
        public virtual HttpResponseMessage Delete(TKey key)
        {
            return DeleteImpl(key);
        }
        //===============================================================
    }

    public abstract class RepositoryController<TValue, TKey1, TKey2> : RepositoryController<TValue> where TValue : class
    {
        //===============================================================
        public virtual HttpResponseMessage Get(TKey1 key1, TKey2 key2)
        {
            return GetImpl(key1, key2);
        }
        //===============================================================
        [AcceptVerbs("PATCH")]
        public virtual HttpResponseMessage Patch(TKey1 key1, TKey2 key2, PatchArguments args)
        {
            return PatchImpl(args, key1, key2);
        }
        //===============================================================
        public virtual HttpResponseMessage Delete(TKey1 key1, TKey2 key2)
        {
            return DeleteImpl(key1, key2);
        }
        //===============================================================

    }
}
