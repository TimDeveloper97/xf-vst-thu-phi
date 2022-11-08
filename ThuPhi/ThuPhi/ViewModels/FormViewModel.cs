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
using ThuPhi.Pages.Popup;
using ThuPhi.Resources;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace ThuPhi.ViewModels
{
    class Template
    {
        public string Name { get; set; }
        public string Pay { get; set; }
    }

    [QueryProperty(nameof(ParameterForm), nameof(ParameterForm))]
    class FormViewModel : BaseViewModel
    {
        #region Property
        private Form model;
        private string parameterForm;
        private ObservableCollection<Info> usersPay, usersNotPay, users;
        private string payCount, notPayCount, sumPay;

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
        public ObservableCollection<Info> UsersPay { get => usersPay; set => SetProperty(ref usersPay, value); }
        public ObservableCollection<Info> UsersNotPay { get => usersNotPay; set => SetProperty(ref usersNotPay, value); }

        public string PayCount { get => payCount; set => SetProperty(ref payCount, value); }
        public string NotPayCount { get => notPayCount; set => SetProperty(ref notPayCount, value); }
        public string SumPay { get => sumPay; set => SetProperty(ref sumPay, value); }
        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(() =>
        {

        });

        public ICommand LongPressUserCommand => new Command<Info>(async (obj) =>
        {
            Info old = new Info
            {
                AccountNumber = obj.AccountNumber,
                Code = obj.Code,
                Name = obj.Name,
                Pay = obj.Pay,
                Time = obj.Time,
                Order = obj.Order,
                Id = obj.Id,
            };
            var update = await Shell.Current.ShowPopupAsync(new UserPopup(obj));

            if (update != null)
            {
                #region Remove
                if(update.Code == null)
                {
                    UsersPay.Remove(obj);
                    UsersNotPay.Remove(obj);
                    return;
                }    
                #endregion

                #region OK
                if ((old.Pay == null || string.IsNullOrEmpty(old.Pay) || long.Parse(old.Pay) == 0)
                && (update.Pay != null && long.Parse(update.Pay) != 0))
                {
                    UsersPay.AddByOrder(update);
                    UsersNotPay.Remove(obj);
                } 
                else if ((old.Pay != null && long.Parse(old.Pay) != 0)
                && (update.Pay == null || string.IsNullOrEmpty(update.Pay) || long.Parse(update.Pay) == 0))
                {
                    UsersNotPay.AddByOrder(update);
                    UsersPay.Remove(obj);
                }
                #endregion
            }

            PayCount = UsersPay.Count.ToString();
            NotPayCount = UsersNotPay.Count.ToString();
        });

        public ICommand SaveCommand => new Command<Info>(async (obj) =>
        {
        });

        public ICommand NewUserCommand => new Command(async () =>
        {
            var user = await Shell.Current.ShowPopupAsync(new UserPopup(null));
            if (user != null)
            {
                user.Code = Guid.NewGuid().ToString();
                users.Add(user);

                if (user.Pay == null || long.Parse(user.Pay) == 0)
                {
                    UsersNotPay.AddByOrder(user);
                }
                else
                    UsersPay.AddByOrder(user);
            }
        });

        public ICommand BackCommand => new Command(async () => await Shell.Current.GoToAsync($".."));

        public ICommand HomeCommand => new Command(async () => await Shell.Current.Navigation.PopToRootAsync());

        public ICommand LoadUserCommand => new Command(async () =>
        {
            IsBusy = true;

            try
            {
                #region Get data
                var res = await Service.FindCollection(Token, Model.Id);
                if (res == null) return;

                res.Items = res.Items.OrderBy(x => x.Name).ToList();
                users?.Clear();
                foreach (var item in res.Items)
                {
                    users.Add(item);
                }

                var msgs = Sms.GetByContentAndDateTime(res.Content, res.Time);
                foreach (var item in msgs)
                {
                    var obj = AnalysisSms(item.Content, res.Content);
                    //var obj = Split(item.Content, res.Content);

                    var user = users.FirstOrDefault(x => x.Id == obj.Name);
                    if (user != null)
                    {
                        user.Pay = obj.Pay;
                        user.Time = UnixTimeToDateTime(long.Parse(item.LongTime));
                    }
                }
                #endregion

                #region Update view
                UsersPay?.Clear();
                UsersNotPay?.Clear();
                SumPay = "0";

                foreach (var item in users)
                {
                    item.Code = Guid.NewGuid().ToString();

                    if (string.IsNullOrEmpty(item.Pay) || long.Parse(item.Pay) == 0)
                        UsersNotPay.Add(item);
                    else
                    {
                        UsersPay.Add(item);
                        SumPay = (long.Parse(item.Pay) + long.Parse(SumPay)).ToString();
                    }
                }

                PayCount = UsersPay.Count.ToString();
                NotPayCount = UsersNotPay.Count.ToString();
                #endregion
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
            UsersPay = new ObservableCollection<Info>();
            UsersNotPay = new ObservableCollection<Info>();
            users = new ObservableCollection<Info>();
        }

        Template AnalysisSms(string body, string content)
        {
            var regexSpace = @"\s+";
            var regexMoney = @"(?<=(\+))(.*?)(?=VND)";
            var regexName = $"(?<=({content} ))(.*?)(?= )";

            var bodyWO = new Regex(regexSpace).Replace(body, " ");
            var money = new Regex(regexMoney).Match(bodyWO).Value;
            var name = new Regex(regexName).Match(bodyWO).Value;
            return new Template { Pay = money.Replace(",", ""), Name = name };
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
        #endregion
    }
}
