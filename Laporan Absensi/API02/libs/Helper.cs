using System.Data;
using System.Net;
using Newtonsoft.Json.Linq;

namespace API02.Libs
{
    public class Helper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Helper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static DataTable JsonToDatatable(JArray sampleData)
        {
            DataTable result = new DataTable();

            try
            {
                if (sampleData == null || sampleData.Count == 0)
                    throw new ArgumentException("sampleData must array object and must atleast 1 row inside.");


                if (sampleData.Count > 0)
                {
                    JObject firstItem = sampleData[0] as JObject;
                    foreach (var prop in firstItem.Properties())
                    {
                        result.Columns.Add(prop.Name);
                    }
                }

                foreach (JObject row in sampleData)
                {
                    var newRow = result.NewRow();

                    foreach (DataColumn col in result.Columns)
                    {
                        newRow[col.ColumnName] = row[col.ColumnName]?.ToString();
                    }

                    result.Rows.Add(newRow);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }

            return result;
        }


        public bool IsLocalhost()
        {
            var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress;

            if (ip == null)
                return false;

            // Check for both IPv4 and IPv6 loopback
            return IPAddress.IsLoopback(ip);

        }

        public static DataTable GetDummyLapAbsensiData()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "impala.json");
            var jsonContent = File.ReadAllText(filePath);
            var rootObject = JObject.Parse(jsonContent);
            var list = rootObject["lapabsensiharian"] as JArray;

            return JsonToDatatable(list);
        }
    }
}
