using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThuPhi.Model.Receive;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThuPhi.Pages.Popup
{
    public class Template
    {
        public bool IsChecked { get; set; }
        public string Name { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListUserPopup : Popup<DetailForm>
    {
        DetailForm _detail;
        public ObservableCollection<Template> Users { get; set; }

        public ListUserPopup(DetailForm detail)
        {
            InitializeComponent();
            BindingContext = this;

            Users = new ObservableCollection<Template>();
            _detail = detail;

            foreach (var user in _detail.Items)
            {
                user.Code = null;
                user.Pay = null;
                user.Time = null;

                Users.Add(new Template { IsChecked = true, Name = user.Name });
            }
            list.ItemsSource = Users;
        }

        private void okBtn_Clicked(object sender, EventArgs e)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if(!Users[i].IsChecked)
                {
                    _detail.Items.RemoveAt(i);
                }    
            }

            Dismiss(_detail);
        }

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }
    }
}