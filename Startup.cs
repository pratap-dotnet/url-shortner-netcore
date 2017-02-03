using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using UrlShortnerNetCore.Models;

namespace UrlShortnerNetCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var storageAccount = CloudStorageAccount.Parse(Configuration.GetValue<string>("TableConnectionString"));
            // Add framework services.
            services.AddMvc();

            services.AddSingleton<CloudStorageAccount>(storageAccount);
            services.AddTransient<ITableRepository<CounterEntity>,TableRepository<CounterEntity>>();
            services.AddTransient<ITableRepository<UrlEntity>, TableRepository<UrlEntity>>();


            //Add counter value if not found
            var sp = services.BuildServiceProvider();
            var counterRepo = sp.GetService<ITableRepository<CounterEntity>>();

            if(counterRepo.GetByPartitionKeyAndRowKey
                (Constants.DefaultPartitionKey, Constants.DefaultRowKey).GetAwaiter().GetResult() == null)
            {
                counterRepo.Insert(new CounterEntity(1000000)).Wait();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
