using Newtonsoft.Json;
using System.Text;

namespace CatViP_API.Helpers
{
    public class CatDetectionHelper
    {
        public static async Task<bool> CheckIfPhotoContainCat(byte[] image)
        {
            using (HttpClient client = new HttpClient())
            {
                var obj = new { image = image };
                var json = JsonConvert.SerializeObject(obj);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync("http://127.0.0.1:5000/predict", content);

                if (res.IsSuccessStatusCode)
                {
                    string responseContent = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    return result!.result;
                }
            }

            return false;
        }
    }
}
