using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ThuPhi.Domain;
using ThuPhi.Model.Receive;
using Xamarin.Forms;

namespace ThuPhi.ViewModels
{
    [QueryProperty(nameof(ParameterForm), nameof(ParameterForm))]
    class FormViewModel : BaseViewModel
    {
        #region Property
        private Form model;
        private string parameterForm;
        private ObservableCollection<Info> users;

        public string ParameterForm
        {
            get => parameterForm; set
            {
                parameterForm = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameterForm, value);
                Model = JsonConvert.DeserializeObject<Form>(ParameterForm);

                IsBusy = true;
            }
        }
        public Form Model { get => model; set => SetProperty(ref model, value); }
        public ObservableCollection<Info> Users { get => users; set => SetProperty(ref users, value); }
        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(() =>
        {

        });

        public ICommand BackCommand => new Command(async () => await Shell.Current.GoToAsync($".."));

        public ICommand HomeCommand => new Command(async () => await Shell.Current.Navigation.PopToRootAsync());

        public ICommand LoadUserCommand => new Command(async () =>
        {
            IsBusy = true;

            try
            {
                var res = await Service.FindCollection(Token, Model.Id);
                if (res == null) return;

                res.Items = res.Items.OrderBy(x => x.Name).ToList();
                Users?.Clear();
                foreach (var item in res.Items)
                {
                    Users.Add(item);
                }

                var msgs = Sms.GetByContentAndDateTime(res.Content, res.Time);
                foreach (var item in msgs)
                {
                    var obj = AnalysisSms(item.Content, res.Content);
                    //var obj = Split(item.Content, res.Content);

                    var user = Users.FirstOrDefault(x => x.Id == obj.Name);
                    if (user != null)
                    {
                        user.Pay = obj.Pay;
                        user.Time = UnixTimeToDateTime(long.Parse(item.LongTime));
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        });
        #endregion

        public FormViewModel()
        {
            Init();
        }

        #region Method
        void Init()
        {
            Title = "Form";
            Users = new ObservableCollection<Info>();
        }

        Template AnalysisSms(string body, string content)
        {
            var regexSpace = @"\s+";
            var regexMoney = @"(?<=(\+))(.*?)(?=VND)";
            var regexName = $"(?<=({content} ))(.*?)(?= )";

            var bodyWO = new Regex(regexSpace).Replace(body, " ");
            var money = new Regex(regexMoney).Match(bodyWO).Value;
            var name = new Regex(regexName).Match(bodyWO).Value;
            return new Template { Pay = money, Name = name };
        }

        DateTime UnixTimeToDateTime(long unixtime)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
        }

        Template Split(string input, string content)
        {
            var numberPos = input.IndexOf('+') + 1;
            if (numberPos == 0) return null;

            var contentPos = input.IndexOf(content, numberPos);
            if (contentPos < 0) return null;

            long a = 0;
            for (int i = numberPos; i < input.Length; i++)
            {
                char c = input[i];
                if (i == ',' || c == '.') continue;
                if (!char.IsDigit(c)) break;

                a = (a << 1) + (a << 3) + (c & 15);
            }

            var namePos = contentPos + content.Length;
            var s = "";

            for (int i = namePos; i < input.Length; i++)
            {
                char c = input[i];
                if (c != ' ') s += c;
            }
            return new Template { Name = s.ToUpper(), Pay = a.ToString() };
        }

        class Template
        {
            public string Name { get; set; }
            public string Pay { get; set; }
        }
        #endregion

    }
}
