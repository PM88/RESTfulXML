using System.Collections.Generic;
using RESTfulXML_1.Models;

namespace RESTfulXML_1.Interfaces
{
    public interface IRequestService
    {
        byte CreateEntries(IEnumerable<Request> requests);
        IEnumerable<Request> ListEntriesAndPrintToXml();
        void Dispose();
    }
}