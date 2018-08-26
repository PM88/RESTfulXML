using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RESTfulXML_1.DataAccess;
using RESTfulXML_1.Models;
using RESTfulXML_1.Models.Services;
using RESTfulXML_1.Interfaces;
using System.Web;

namespace RESTfulXML_1.Controllers.API
{
    public enum ValidationCodes
    {
        Ok = 0,
        Conflict = 1,
        Bad = 2,
    }

    public class RequestsController : ApiController
    {
        private IRequestService _service;

        public RequestsController()
        {
            _service = new RequestService(this.ModelState);
        }

        // GET: /api/jobs/saveFiles
        public IEnumerable<Request> GetRequests()
        {
            return _service.ListEntriesAndPrintToXml();
        }

        // POST: /api/data
        [ResponseType(typeof(Request))]
        public IHttpActionResult PostRequest([FromBody]IEnumerable<Request> requests)
        {
            if (requests == null || requests.Count() == 0)
                return Content(HttpStatusCode.BadRequest, "Invalid object list detected!");

            switch (_service.CreateEntries(requests))
            {
                case (byte)ValidationCodes.Ok:
                    return StatusCode(HttpStatusCode.OK);
                case (byte)ValidationCodes.Conflict:
                    return Content(HttpStatusCode.Conflict, this.ModelState);
                default:
                    return Content(HttpStatusCode.BadRequest, this.ModelState);
            }
            
            //_service.CreateEntry(request);
            //return (IHttpActionResult)Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}