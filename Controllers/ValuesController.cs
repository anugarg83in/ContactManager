using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApplicationSample.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public async Task<dynamic> Get()
        {
            dynamic JsonArray = new JArray() { "Aval","Bval"};
            //JsonArray =new()   [{"key":"val1" },{ "key":"val2 }];
            var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes("cb24f9e5-8e0e-4512-b3f0-d9599023f641-bluemix" + ":" + "1dd88df0ecb76a27fb3fce1e8572cea42738d910aedbcb9e87813ac0261fedac"));
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://" + "cb24f9e5-8e0e-4512-b3f0-d9599023f641-bluemix.cloudant.com");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            var response = await client.GetAsync("events" + "/_all_docs?include_docs=true");

            if (response.IsSuccessStatusCode)
            {
                JObject s = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                JToken token = JToken.Parse(response.Content.ReadAsStringAsync().Result);
                JArray rows = (JArray)s["rows"];
                
                int tc = token.SelectToken("total_rows").Value<int>();
                //if (allEvent == "true")
                //{
                for (int i = 0; i < 2; i++)
                {
                    dynamic jsonObject = new JObject();

                    if (rows[i]["doc"]["userId"].ToString() != "_design/userId")
                    {
                        string _title = rows[i]["doc"]["title"].ToString();
                        string _start = rows[i]["doc"]["start"].ToString() + " 10:25:28";
                        string _end = rows[i]["doc"]["end"].ToString() + " 10:25:28";
                        string _id = rows[i]["doc"]["_id"].ToString();
                        string _name = rows[i]["doc"]["name"].ToString();
                        jsonObject.Add("title", _title);
                        jsonObject.start = _start;
                        jsonObject.end = _end;
                        jsonObject.id = _id;
                        jsonObject.url = "";
                        jsonObject.allDay = "1";
                        jsonObject.name = _name;
                        JsonArray.Add(jsonObject);
                    }
                }
                //}
                return JsonArray;
            }            
            else
            {
                if (response.ReasonPhrase == "Conflict")
                {
                  //  ViewData["AbsenceStatus"] = "Absence Type already Saved for the intended Time Duration";
                 //   return "Conflict";
                }
                //return "not ok";
            }

            return JsonArray;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
