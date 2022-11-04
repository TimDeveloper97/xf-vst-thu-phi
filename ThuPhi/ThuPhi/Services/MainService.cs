using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThuPhi.Domain;
using ThuPhi.Model.Receive;
using ThuPhi.Model.Send;

namespace ThuPhi.Services
{
    class MainService : IMainService
    {
        public async Task<List<Form>> InfoCollection(string token)
        {
            var res = await Service<Form, string>.Posts(new BaseModel { Code = 0, Token = token, Url = Api.UpdateDotThu });
            return res;
        }

        public async Task<Account> Login(string username, string password)
        {
            var res = await Service<Account, Login>.Post(Api.Login, new Login { UserName = username, Password = password });
            return res;
        }
    }
}
