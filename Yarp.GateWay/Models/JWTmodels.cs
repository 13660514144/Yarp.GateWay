using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yarp.GateWay.Models
{
    public class JWTmodels
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 发布者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 接收者
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int AccessExpiration { get; set; }
        /// <summary>
        /// 刷新时间
        /// </summary>
        public int RefreshExpiration { get; set; }
    }
}
