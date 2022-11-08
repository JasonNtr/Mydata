using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Domain.DTO
{
    public class AppSettings
    {
        public string url { get; set; }
        public string aade_user_id { get; set; }
        public string Ocp_Apim_Subscription_Key { get; set; }
        public double timerSeconds { get; set; }
        public int startupDelayMSeconds { get; set; }
        public string folderPath { get; set; }
        public bool Auto { get; set; }
        public List<string> ConnectionStrings { get; set; }

        public static AppSettings Create(string filePath)
        {
            var json = File.ReadAllText(filePath, Encoding.GetEncoding("windows-1253"));
            var appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
            return appSettings;
        }

    }
}
