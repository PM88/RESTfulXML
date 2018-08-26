using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Bogus;
using RESTfulXML_1.Models;

namespace RESTfulXML_2
{
    class Program
    {
        static void Main(string[] args)
        {
            char YForRepeating = 'y';
            while (YForRepeating == 'y')
            {
                uint numberOfObjectsToPost = 0;
                bool validInput = false;
                while (!validInput)
                {
                    try
                    {
                        numberOfObjectsToPost = GetNumberOfObjectsToPost(args);
                        validInput = true;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid input!");
                        validInput = false;
                    }
                }
                if (numberOfObjectsToPost == 0)
                    Environment.Exit(0);

                string masterJson = GenerateJsonCollectionString(numberOfObjectsToPost);
                SendPost(masterJson);

                Console.WriteLine(@"Please enter 'y' letter to reset or enter anything else to exit.");
                try
                {
                    YForRepeating = Convert.ToChar(Console.ReadLine());
                }
                catch (Exception)
                {
                    YForRepeating = 'n';
                }
            }
        }

        private static string GenerateJsonCollectionString(uint numberOfObjectsToPost)
        {
            var names = new[] { "Name 1", "Name 2", "Name 3", "Name 4", "Name 5" };
            string masterJson = "[" + System.Environment.NewLine;

            for (uint i = 1; i <= numberOfObjectsToPost; i++)
            {
                var testRequests = new Faker<Request>()
                    .StrictMode(true)
                    .RuleFor(r => r.Index, f => f.Random.Number(1, 1000))
                    .RuleFor(r => r.Visits, f => (int?)f.Random.Number(1, 1000))
                    .RuleFor(r => r.Name, f => f.PickRandom(names))
                    .RuleFor(r => r.Date, DateTime.Now);

                var request = testRequests.Generate();
                masterJson += GetJsonFromRequest(request) + System.Environment.NewLine;
            }

            return masterJson + "]";
        }

        private static uint GetNumberOfObjectsToPost(string[] args)
        {
            uint numberOfObjectsToPost;
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter the number of objects you want to generate. Type 0 if you want to exit. Max value is 4,294,967,295.");
                numberOfObjectsToPost = Convert.ToUInt32(Console.ReadLine());
            }
            else
            {
                numberOfObjectsToPost = Convert.ToUInt32(args[0]);
                Console.WriteLine("Number of objects to generate: " + numberOfObjectsToPost.ToString());
            }
            return numberOfObjectsToPost;
        }

        private static string GetJsonFromRequest(Request request)
        {
            string jsonRequest = string.Empty;
            jsonRequest += "{\"ix\": " + request.Index;
            jsonRequest += ",\"name\":\"" + request.Name + "\"";
            jsonRequest += request.Visits == null ? "" : ",\"visits\": " + request.Visits;
            jsonRequest += ",\"date\":\"" + GetDateTimeString(request.Date) + "\"},";
            return jsonRequest;
        }

        private static void SendPost(string masterJson)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["ApiAddress"].ToString());
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(masterJson);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                Console.WriteLine("Success!");
                Console.WriteLine("Posted Json:");
                Console.WriteLine(masterJson);
                httpResponse.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static string GetDateTimeString(DateTime date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        }
    }
}
