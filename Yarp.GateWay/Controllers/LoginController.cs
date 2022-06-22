using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yarp.GateWay.Identity;
using Yarp.GateWay.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Yarp.GateWay.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[EnableCors("any")] //设置跨域处理的 代理
    public class LoginController : ControllerBase
    {
        private readonly IAuthenticateService _authenservice;
        private Users users;
        public LoginController(IAuthenticateService service,Users _Users)
        {
            _authenservice = service;
            users = _Users;
        }
        [HttpGet]
        public string GetUser()
        {
            var aa = JwtUser.GetRequestUser(this.Request);
            return aa;
        }
        [HttpPost]
        public string PostReq(string nameuser)
        {
            var aa = JwtUser.GetRequestUser(this.Request);
            return aa;
        }
        [HttpGet]
        public string GetToken(string UserName,string PWD)
        {
            ///正常来说这里应该是链接数据库查询用户名密码是否正确 
            if (UserName != "lzd" || PWD != "123")
            {
                return "用户名或密码错误";
            }
            users.UserName = UserName;
            users.PWD = PWD;
            //签发token
            string token;
            if (_authenservice.IsAuthenticated(users, out token))
            {
                return token;
            }
            return "用户名或密码错误";
        }
        // GET: api/<LoginController>
        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LoginController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return @$"value={id}";
        }

        // POST api/<LoginController>
        [HttpPost]
        [Authorize]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public void Delete(int id)
        {
        }
    }
}
