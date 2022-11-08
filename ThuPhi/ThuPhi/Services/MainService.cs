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
        public async Task<bool> SaveCollection(string token, DetailForm detail)
        {
            detail.Id = null;

            var res = await Service<bool>.Put(new BaseModel
            {
                Token = token,
                Url = Api.UpdateDotThu,
                Value = new
                {
                    code = 100,
                    value = detail,
                }
            });

            return res;
        }
        public async Task<bool> CloneCollection(string token, DetailForm detail)
        {
            detail.Id = null;

            var res = await Service<bool>.Put(new BaseModel 
            { 
                Token = token, 
                Url = Api.UpdateDotThu, 
                Value = new 
                { 
                    code = 100, 
                    value = detail,
                } 
            });

            return res;
        }

        public async Task<DetailForm> FindCollection(string token, string id)
        {
            var res = await Service<DetailForm>.Post(new BaseModel { Token = token, Url = Api.UpdateDotThu, Value = new { code = 3, _id = id } });
            return res;
        }

        public async Task<List<Form>> InfoCollection(string token)
        {
            var res = await Service<Form>.Posts(new BaseModel { Token = token, Url = Api.UpdateDotThu, Value = new { Code = 0 } });
            return res;
        }

        public async Task<Account> Login(string username, string password)
        {
            var res = await Service<Account>.Post(new BaseModel { Url = Api.Login, Value = new { UserName = username, Password = password } });
            return res;
        }
    }
}
