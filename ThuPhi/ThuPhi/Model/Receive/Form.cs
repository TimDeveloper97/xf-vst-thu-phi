using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using ThuPhi.Domain;

namespace ThuPhi.Model.Receive
{
    public class Form
    {
        [JsonProperty("Ten")]
        public string Name { get; set; }

        [JsonProperty("NoiDung")]
        public string Content { get; set; }

        [JsonProperty("ThoiGian")]
        public DateTime Time { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }

    public class DetailForm : Form
    {
        [JsonProperty("items")]
        public List<Info> Items { get; set; }

        public DetailForm() { }
        public DetailForm(Form form) {
            Id = form.Id;
            Name = form.Name;
            Content = form.Content;
            Time = form.Time;

            Items = new List<Info>();
        }
    }

    public class Info : BaseBinding
    {
        public string Code { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("HoTen")]
        private string name;

        [JsonProperty("SoTK")]
        private string accountNumber;

        [JsonProperty("SoTT")]
        public string Order { get; set; }

        [JsonProperty("No")]
        private string owe;

        [JsonProperty("Tra")]
        private string pay;

        [JsonProperty("ThoiGian")]
        private DateTime? time;

        public DateTime? Time { get => time; set => SetProperty(ref time, value); }
        public string Owe { get => owe; set => SetProperty(ref owe, value); }
        public string Pay { get => pay; set => SetProperty(ref pay, value); }
        public string Name { get => name; set => SetProperty(ref name, value); }
        public string AccountNumber { get => accountNumber; set => SetProperty(ref accountNumber, value); }
    }
}
