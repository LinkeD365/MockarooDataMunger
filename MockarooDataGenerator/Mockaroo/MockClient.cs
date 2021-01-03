using LinkeD365.MockDataGen.Mock;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace LinkeD365.MockDataGen
{
    internal class MockClient
    {
        private const string MockarooApiUrl = @"https://www.mockaroo.com/api/generate.json?key={0}&count={1}";
        private string _apiKey;

        public MockClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        public List<ExpandoObject> GetData(ExpandoObject exo, int count)
        {
            if (count == 0)
                return new List<ExpandoObject>();

            List<Dictionary<string, object>> fields = new List<Dictionary<string, object>>();

            foreach (var property in exo)
            {
                BaseMock mock = (BaseMock)property.Value;
                var field = mock.GetField();
                if (!field.ContainsKey("Name"))
                    field.Add("Name", property.Key);

                fields.Add(field);
            }

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var jsonString = JsonConvert.SerializeObject(
                fields,
                Formatting.None,
                jsonSettings);
            var url = string.Format(MockarooApiUrl, _apiKey, count);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Content = new StringContent(jsonString)
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string responseContent;
            var handler = new HttpClientHandler();
            using (var client = new HttpClient(handler))
            {
                var response = client.SendAsync(request).Result;
                responseContent = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var errObj = JsonConvert.DeserializeObject<JObject>(
                        response.Content.ReadAsStringAsync().Result);
                    throw new Exception(errObj.GetValue("error").ToString());
                }


                // data = count == 1
                //   ? new[] { JsonConvert.DeserializeObject<T>(responseContent) }.AsEnumerable()
                // : JsonConvert.DeserializeObject<IEnumerable<T>>(responseContent);
            }

            var expConverter = new ExpandoObjectConverter();
            return JsonConvert.DeserializeObject<List<ExpandoObject>>(responseContent, expConverter);
            //  return JsonConvert.DeserializeObject(responseContent);
        }

        private static object GetValueOrArray(CustomAttributeTypedArgument argument)
        {
            if (argument.Value.GetType() == typeof(ReadOnlyCollection<CustomAttributeTypedArgument>))
                return (
                    from cataElement in (ReadOnlyCollection<CustomAttributeTypedArgument>)argument.Value
                    select cataElement.Value.ToString()
                    ).ToArray();

            return argument.Value;
        }

        private object GetFields<T>()
        {
            throw new NotImplementedException();
        }
    }
}