using CatViP_API.DTOs.NotificationDTOs;
using Newtonsoft.Json;
using System.Text;

namespace CatViP_API.Helpers
{
    public class OneSignalSendNotiHelper
    {
        public static async Task OneSignalSendCaseReportNoti(List<string> usernames, string message)
        {
            OneSignalNotiDTO oneSignalNotiDTO = new OneSignalNotiDTO();
            oneSignalNotiDTO.app_id = "2c9ce8b1-a075-4864-83a3-009c8497310e";
            oneSignalNotiDTO.include_external_user_ids = usernames;
            oneSignalNotiDTO.contents = new Dictionary<string, string>();
            oneSignalNotiDTO.contents.Add("en", $"{message}");

            var json = JsonConvert.SerializeObject(oneSignalNotiDTO);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Basic MTI3ZjcwYjktNGJiMy00YWViLTljMmQtYjMwNDI5NzBkMjRk");
                client.DefaultRequestHeaders.Add("accept", "application/JSON");

                await client.PostAsync("https://onesignal.com/api/v1/notifications", content);
            }
        }

        public static async Task OneSignalSendChatNoti(List<string> usernames, string sender, string message)
        {
            OneSignalNotiDTO oneSignalNotiDTO = new OneSignalNotiDTO();
            oneSignalNotiDTO.app_id = "2c9ce8b1-a075-4864-83a3-009c8497310e";
            oneSignalNotiDTO.include_external_user_ids = usernames;
            oneSignalNotiDTO.contents = new Dictionary<string, string>();
            oneSignalNotiDTO.contents.Add("en", $"{sender}: {message}");

            var json = JsonConvert.SerializeObject(oneSignalNotiDTO);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Basic MTI3ZjcwYjktNGJiMy00YWViLTljMmQtYjMwNDI5NzBkMjRk");
                client.DefaultRequestHeaders.Add("accept", "application/JSON");

                await client.PostAsync("https://onesignal.com/api/v1/notifications", content);
            }
        }
    }
}
