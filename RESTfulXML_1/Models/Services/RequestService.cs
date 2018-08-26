using System.Collections.Generic;
using RESTfulXML_1.DataAccess;
using System.Web.Http.ModelBinding;
using RESTfulXML_1.Interfaces;
using System;
using RESTfulXML_1.Controllers.API;
using System.Xml;
using System.Web;
using System.Text.RegularExpressions;

namespace RESTfulXML_1.Models.Services
{
    public class RequestService : IRequestService
    {
        private ModelStateDictionary _modelState;
        private IUnitOfWork UoW;

        public RequestService(ModelStateDictionary modelState)
        {
            UoW = new UnitOfWork(new ApplicationContext());
            _modelState = modelState;
        }

        private string GetRequestSummary(Request request)
        {
            return String.Format("Request-> Ix: {0}, Name: {1}, Visits: {2}, Date: {3}",
                    request.Index, request.Name, request.Visits, request.Date);
        }

        protected byte ValidateRequests(IEnumerable<Request> requests)
        {
            foreach (Request request in requests)
            {
                string requestSummary = GetRequestSummary(request);

                if (request.Index == 0)
                    _modelState.AddModelError("Ix", "Ix must be larger than 0! " + requestSummary);
                if (String.IsNullOrEmpty(request.Name))
                    _modelState.AddModelError("Name", "Name is required!" + requestSummary);
                if (request.Date == null || request.Date == DateTime.MinValue)
                    _modelState.AddModelError("Date", "Valid date is required! " + requestSummary);

                if (!_modelState.IsValid)
                    return (byte)ValidationCodes.Bad;

                if (!(UoW.Requests.GetRequest(request.Index, request.Name, request.Date) == null))
                {
                    _modelState.AddModelError("Conflict", "Record already exists! " + requestSummary);
                    return (byte)ValidationCodes.Conflict;
                }
            }

            return 0;
        }

        public IEnumerable<Request> ListEntriesAndPrintToXml()
        {
            IEnumerable<Request> records = ListEntries();
            PrintToXml(records);

            return records;
        }

        private IEnumerable<Request> ListEntries()
        {
            return UoW.Requests.GetAll();
        }

        private void PrintToXml(IEnumerable<Request> requests)
        {
            foreach (Request request in requests)
            {
                string appDataPath= HttpContext.Current.Server.MapPath("~/App_Data/");
                string requestXmlDraftFileName = appDataPath + "Request.xml";
                string shortDate = request.Date.ToString("yyyy-MM-dd");

                XmlDocument xmlDocObj = new XmlDocument();
                xmlDocObj.Load(requestXmlDraftFileName);

                XmlNode rootNode = xmlDocObj.SelectSingleNode("request");
                rootNode.AppendChild(xmlDocObj.CreateNode(XmlNodeType.Element, "ix", "")).InnerText = request.Index.ToString();

                XmlNode contentNode = rootNode.AppendChild(xmlDocObj.CreateNode(XmlNodeType.Element, "content", ""));
                contentNode.AppendChild(xmlDocObj.CreateNode(XmlNodeType.Element, "name", "")).InnerText = request.Name.ToString();//.ToUpper();
                if(request.Visits != null)
                    contentNode.AppendChild(xmlDocObj.CreateNode(XmlNodeType.Element, "visits", "")).InnerText = request.Visits.ToString();
                contentNode.AppendChild(xmlDocObj.CreateNode(XmlNodeType.Element, "dateRequested", "")).InnerText = shortDate;

                string cleanFileName = GetStringNoSpecialChars(GetRequestSummary(request));
                string xmlOutputFolderPath = appDataPath + @"xml\" + shortDate;
                System.IO.Directory.CreateDirectory(xmlOutputFolderPath);
                string savePath = xmlOutputFolderPath + String.Format(@"\{0}.xml", cleanFileName);
                xmlDocObj.Save(savePath);
            }
        }

        private string GetStringNoSpecialChars(string dirtyString)
        {
            return Regex.Replace(dirtyString, @"[^0-9a-zA-Z]+", "");
        }

        public byte CreateEntries(IEnumerable<Request> requests)
        {
            byte validationCode = ValidateRequests(requests);
            if (validationCode != (byte)ValidationCodes.Ok)
                return validationCode;

            try
            {
                UoW.Requests.Add(requests);
            }
            catch
            {
                return (byte)ValidationCodes.Bad;
            }

            return (byte)ValidationCodes.Ok;
        }

        public void Dispose()
        {
            UoW.Dispose();
        }
    }
}