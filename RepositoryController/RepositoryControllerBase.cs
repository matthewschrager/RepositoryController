using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Repository;

namespace RepositoryController
{
    public abstract class RepositoryControllerBase<TValue, TStoredValue> : ApiController where TStoredValue : class
    {
        //===============================================================
        protected abstract IRepository<TStoredValue> Repository { get; }
        //===============================================================
        public virtual HttpResponseMessage Get()
        {
            try
            {
                using (var objects = Repository.GetItemsContext())
                {
                    return CreateSuccessResponse(HttpStatusCode.OK, objects.Objects.Select(ConvertToExternalValue));
                }
            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        protected HttpResponseMessage GetImpl(params Object[] keys)
        {
            try
            {
                using (var obj = Repository.Find(keys))
                {
                    if (obj.Object == null)
                        return CreateFailureResponse(HttpStatusCode.NotFound, "Could not find object with key: { " + String.Join(", ", keys) + " }.");

                    return CreateSuccessResponse(HttpStatusCode.OK, ConvertToExternalValue(obj.Object));
                }
            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        public virtual HttpResponseMessage Post(TValue obj)
        {
            try
            {
                Repository.Store(ConvertToStoredValue(obj));
                return Request.CreateResponse(HttpStatusCode.OK);

            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        protected HttpResponseMessage PatchImpl(PatchArguments args, params Object[] keys)
        {
            if (keys == null)
                return CreateFailureResponse(HttpStatusCode.BadRequest, "You must specify the key for the object to update.");

            if (args.UpdateType == null)
                args.UpdateType = "set";

            try
            {
                var pathToProperty = !String.IsNullOrWhiteSpace(PathToDataProperty) ? PathToDataProperty + "." + args.PathToProperty : args.PathToProperty;
                Repository.Update(pathToProperty, args.UpdateDescriptor, Utility.ToUpdateType(args.UpdateType), keys);
                return CreateSuccessResponse(HttpStatusCode.OK);

            }

            catch (Exception e)
            {
                return CreateFailureResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //===============================================================
        protected HttpResponseMessage DeleteImpl(params Object[] keys)
        {
            try
            {
                Repository.Remove(keys);
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
        protected abstract TStoredValue ConvertToStoredValue(TValue value);
        //===============================================================
        protected abstract TValue ConvertToExternalValue(TStoredValue value);
        //===============================================================
        protected abstract String PathToDataProperty { get; }
        //===============================================================
    }
}
