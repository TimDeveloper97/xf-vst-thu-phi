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
    public partial class NewFormPopup : BasePopup
    {
        public NewFormPopup()
        {
            InitializeComponent();
        }
    }

    //public class Popup: Popup<object>
    //{ }
}