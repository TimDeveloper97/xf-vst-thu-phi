using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThuPhi.Model.Receive;
using ThuPhi.Model.Send;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThuPhi.Pages.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewFormPopup : Popup<FormCode>
    {
        FormCode _formCode;
        public NewFormPopup(FormCode fc)
        {
            InitializeComponent();
            
            if(fc == null)
            {
                title.Text = "Thêm đợt thu phí";
            }    
            else
            {
                title.Text = "Sửa đợt thu phí";
                name.Text = fc.Name;
                content.Text = fc.Content;
            }    

            _formCode = fc;
        }

        private void okBtn_Clicked(object sender, EventArgs e)
        {
            _formCode.Name = name.Text;
            _formCode.Content = content.Text;

            Dismiss(_formCode);
        }

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }
    }
}