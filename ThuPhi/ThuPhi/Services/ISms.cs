using System;
using System.Collections.Generic;
using System.Text;
using ThuPhi.Model.Send;

namespace ThuPhi.Services
{
    public interface ISms
    {
        List<User> GetAll();
        List<User> GetByDateTime(DateTime start);
        List<User> GetByAddress(string[] addresss);
        List<User> GetByContent(string para);
        List<User> GetByContentAndDateTime(string para, DateTime start);
    }
}
