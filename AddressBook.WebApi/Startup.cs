using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Logging;

namespace AddressBook.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //  services.Configure<AddressBookDataLib.Interface.IDatabaseSetting>(Configuration.GetSection("Database"));

            AddressBookDataLib.Interface.IDatabaseSetting databaseSetting = new AddressBookDataLib.Settings.Database()
            {
                ConnectionString = Configuration.GetSection("Database").GetSection("ConnectionString").Value
            };

            AddressBookDataLib.Interface.IDBContext<AddressBookDataLib.Context.AddressBook> dbContext = new AddressBookDataLib.Context.AddressBook(databaseSetting);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton(databaseSetting);

            services.AddSingleton(dbContext);

            services.AddTransient<AddressBookDataLib.Interface.IAddressRepository, AddressBookDataLib.Repository.AddressRepository>();

            services.AddTransient<AddressBookBusinessLib.Interface.IAddressRepository, AddressBookBusinessLib.Repository.AddressRepository>();

            services.AddTransient<AddressBookDataLib.Interface.IContactRepository, AddressBookDataLib.Repository.ContactRepository>();

            services.AddTransient<AddressBookBusinessLib.Interface.IContactRepository, AddressBookBusinessLib.Repository.ContactRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0.0", new Info { Title = "AddressBook", Version = "v1.0.0" });
                c.IncludeXmlComments(GetWebApiXMLDocumentPath());
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {

            logger.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1.0.0/swagger.json", "AddressBook");
            });
        }

        private string GetWebApiXMLDocumentPath()
        {
            return string.Format("{0}/AddressBookWebApi.xml", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
