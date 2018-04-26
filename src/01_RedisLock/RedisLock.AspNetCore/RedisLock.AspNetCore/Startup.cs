using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedisLock.AspNetCore.Cache;
using RedisLock.AspNetCore.Config;
using RedisLock.AspNetCore.Extensions;
using RedisLock.AspNetCore.Filter;

namespace RedisLock.AspNetCore
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

			services.AddMvc(options =>
            {
				options.UseCentralRoutePrefix("api");
	            options.Filters.Add<XcActionFilter>();
            });
	        services.AddSingleton(typeof(AppSettings));
	        services.AddSingleton<IXcCache, XcRedisCache>();
        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {

			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
	}
}
