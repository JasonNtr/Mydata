using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class AppSettings
    {
        public string url { get; set; }
        public string aade_user_id { get; set; }
        public string Ocp_Apim_Subscription_Key { get; set; }
        public string folderPath { get; set; }
        public bool Auto { get; set; }
    }
}
