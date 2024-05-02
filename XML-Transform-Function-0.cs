using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Moonman.Function
{

    public class Employee {
        public int Id {get; set;}
        public string Name {get; set;}
    }

    public class Company {
        public List<Employee> Employees {get; set;}
        public Company() {
            Employees = new List<Employee>();
        }
    }

    public static class XML_Transform_Function_0
    {

        [FunctionName("XML_Transform_Function_0")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            log.LogInformation(req.Body.ToString());

            var stringWriter = new StringWriter();

            List<Employee> empList = new();

            for (int i = 0; i < 5; i++) {
                Employee emp = new()
                {
                    Id = i,
                    Name = $"Emp{i}"
                };
                empList.Add(emp);
            }

            Company comp = new()
            {
                Employees = empList
            };

            System.Xml.Serialization.XmlSerializer x = new(comp.GetType());
            x.Serialize(stringWriter, comp);

            log.LogInformation(stringWriter.ToString());

            // string name = req.Query["name"];

            // string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // dynamic data = JsonConvert.DeserializeObject(requestBody);
            // name ??= data?.name;

            // string responseMessage = string.IsNullOrEmpty(name)
            //     ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //     : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new HttpResponseMessage
            {
                Content = new StringContent(stringWriter.ToString(), Encoding.Default, @"application/xml"),
            };
        }
    }
}
