using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using REST.Engine.Entities;

namespace REST.Tests.Data
{
    public class PayLoadData : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            for (var i = 0; i < 11; i++)
            {
                if(i+1 ==2)
                    continue;
                yield return ReadFile(i + 1);
            }
               
        }

        private object[] ReadFile(int fileNumber)
        {
            var obj = new object[1];

            var jsonFileName = @"JSONFiles\payload" + fileNumber + ".json";
            var content = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                jsonFileName));
            var payLoad = JsonSerializer.Deserialize<PayLoad>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            obj[0] = payLoad;

            return obj;
        }
    }
}