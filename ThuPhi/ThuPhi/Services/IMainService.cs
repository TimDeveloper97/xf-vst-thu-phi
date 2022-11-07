using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThuPhi.Model.Receive;
using ThuPhi.Model.Send;

namespace ThuPhi.Services
{
    interface IMainService
    {
        Task<Account> Login(string username, string password);
        Task<List<Form>> InfoCollection(string token);
        Task<DetailForm> FindCollection(string token, string id);
    }
}
