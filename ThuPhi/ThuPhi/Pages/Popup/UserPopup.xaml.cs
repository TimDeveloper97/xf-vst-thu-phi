using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThuPhi.Model.Receive;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThuPhi.Pages.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPopup : Popup<Info>
    {
        Info _info;
        public UserPopup(Info i)
        {
            InitializeComponent();

            if (i == null)
            {
                title.Text = "Tạo người nộp phí";
                i = new Info();
            }    
            else
            {
                title.Text = "Sửa người nộp phí";
                name.Text = i.Name;
                stk.Text = i.AccountNumber;
                pay.Text = i.Pay;
            }    

            _info = i;
        }

        private void okBtn_Clicked(object sender, EventArgs e)
        {
            _info.Name = name.Text;
            _info.Pay = pay.Text;
            _info.AccountNumber = stk.Text;

            Dismiss(_info);
        }

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }
    }
}