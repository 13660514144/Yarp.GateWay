using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yarp.GateWay.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Yarp.GateWay.Identity;

namespace Yarp.GateWay
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Yarp.GateWay", Version = "v1" });
            });

            services.Configure<JWTmodels>(Configuration.GetSection("JWTTokenconfig"));
            var token = Configuration.GetSection("JWTTokenconfig").Get<JWTmodels>();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,//�Ƿ���֤Issuer
                    ValidateAudience = false,//�Ƿ���֤Audience
                    ValidateIssuerSigningKey = true,//�Ƿ���֤SigningKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Secret)),//�õ�SigningKey
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ClockSkew = TimeSpan.FromMinutes(1)//���û���ʱ�䣬token��������Чʱ��������ʱ�����jwt�Ĺ���ʱ�䣬���������Ĭ����5����
                };
            });
            //services.AddReverseProxy().LoadFromConfig(Configuration.GetSection("ReverseProxy"));//yarp

            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<Users>();

        }
        /*/// <summary>
        /// JWT��Ȩ
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomJWT(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(option =>
                    {
                        option.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF")),
                            ValidateIssuer = false,
                            ValidIssuer = "http://localhost:6060/",
                            ValidateAudience = false,
                            ValidAudience = "http://localhost:5000/",
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.FromMinutes(60)
                        };
                    });
            return services;
        }*/
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Yarp.GateWay v1"));
            }

            app.UseRouting();
            //����������������
            app.UseAuthentication();//��֤
            app.UseAuthorization();//��Ȩ

            app.UseEndpoints(endpoints =>
            {                
                endpoints.MapControllers();
                //endpoints.MapReverseProxy();
            });
        }
    }
}
