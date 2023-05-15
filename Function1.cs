using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using httpTriggerOrderDemo.Model;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace httpTriggerOrderDemo
{
    public static class Function1
    {
        [FunctionName("SingleObj")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            StudentXml student=new StudentXml();
            student.Name = "vishal";
            student.Age = 55;
            student.ClassName = "BEIT";
            student.DOB = "29/06/1999";

            StringBuilder sb =new System.Text.StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(StudentXml)); 
            StringWriter sw=new StringWriter(sb);
            serializer.Serialize(sw, student);
            string responseMessage = sb.ToString();
                
            return new OkObjectResult(responseMessage);
        }


        [FunctionName("MultipleObj")]
        public static async Task<IActionResult> Run1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            Student student=new Student();
            StudentXmls students = new StudentXmls();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var jsons = JsonConvert.DeserializeObject<Students>(requestBody);
            students.Studentxmls = new List<StudentXml>();

            foreach(var json in jsons.students)
            {
                StudentXml studentxml = new StudentXml();
                studentxml.Name = json.name;
                studentxml.Age = json.age;
                studentxml.ClassName = json.classname;
                studentxml.DOB = json.dob;
            }

           

            StringBuilder sb = new System.Text.StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(StudentXml));
            StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, student);
            string responseMessage = sb.ToString();

            return new OkObjectResult(responseMessage);
        }
    }
}
