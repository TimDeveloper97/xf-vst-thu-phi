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
            OnLoad();

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
                    var obj = AnalysisSms(item.Content);

                    var user = Users.FirstOrDefault(x => x.Id == obj.Name);
                    if(user != null)
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

        void OnLoad()
        {
            Model = JsonConvert.DeserializeObject<Form>(ParameterForm);
        }

        Template AnalysisSms(string body)
        {
            var regexSpace = @"\s+";
            var regexMoney = @"(?<=(\+))(.*?)(?=VND)";
            var regexName = @"(?<=(CDP20223 ))(.*?)(?= )";

            var bodyWO = new Regex(regexSpace).Replace(body, " ");
            var money = new Regex(regexMoney).Match(bodyWO).Value;
            var name = new Regex(regexName).Match(bodyWO).Value;
            return new Template { Pay = money, Name = name };
        }

        public DateTime UnixTimeToDateTime(long unixtime)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
        }

        class Template
        {
            public string Name { get; set; }
            public string Pay { get; set; }
        }
        #endregion

    }
}
