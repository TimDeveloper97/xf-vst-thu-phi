using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThuPhi.iOS.Renderers;
using ThuPhi.Model.Send;
using ThuPhi.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SmsIOS))]
namespace ThuPhi.iOS.Renderers
{
    public class SmsIOS : ISms
    {
        public List<User> GetAll()
        {
            return null;
        }

        public List<User> GetByAddress(string[] addresss)
        {
            return null;
        }

        public List<User> GetByContent(string para)
        {
            return null;
        }

        public List<User> GetByContentAndDateTime(string para, DateTime start)
        {
            return null;
        }

        public List<User> GetByDateTime(DateTime start)
        {
            return null;
        }
    }
}