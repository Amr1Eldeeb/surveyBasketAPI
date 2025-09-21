

using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using surveyBasket.Api.Authenciation;
using surveyBasket.Api.Date;
using surveyBasket.Api.Date.Migrations;
using surveyBasket.Api.Settings;
using System.Text;


namespace surveyBasket.Api
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddDependences(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHybridCache();
            // to read array of origins from appsettings as localhost.......
            // var AllowOrigins = configuration.GetSection("AllowOrigins").Get<string[]>();    
            //Put the AllowOrigins in the withOrigins

            //services.AddCors
            //    (Options => Options.AddPolicy("Allow All",
            //    builder => builder.AllowAnyOrigin().
            //    AllowAnyMethod().AllowAnyHeader()
            //    //.WithMethods("PUT","DELETE","POST")
            //   // .WithOrigins("take domin as localHost//2092")
            //   //withheaders



            //    ));
            services.AddCors(Options => Options.
            AddDefaultPolicy(builder => builder.
            AllowAnyOrigin().
            AllowAnyHeader()
            .AllowAnyMethod()
            //  .WithOrigins(AllowOrigins )
            ));

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddSwagger();

            services.AddMapster();
            services.AddAuthorazationsConfig(configuration);
            //
            services.AddFluentValidation();
            var connencationString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection String is Not Found");

            services.AddDbContext
                <ApplicationDbContext>
                (options => options.UseSqlServer(connencationString));
            //
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IVoteServices, VoteServices>();

            services.AddScoped<IResultServices, ResultService>();
            //services.AddScoped<ICacheServices, CacheServices>();

            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddProblemDetails();
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
            services.AddScoped<IEmailSender, EmailServecies>();

            services.AddHttpContextAccessor();
            services.AddBackgroundjobs(configuration);
            services.AddScoped<INotificationsServices, NotificationServices>();
            return services;
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();


            return services;
        }
        public static IServiceCollection AddMapster(this IServiceCollection services)
        {
            //services.AddMapster();
            var mappingconfig = TypeAdapterConfig.GlobalSettings;
            mappingconfig.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton<IMapper>(new Mapper(mappingconfig));




            return services;
        }
        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {


            services.AddScoped<IPollServices, PollServices>();//For Di injection 
            services.AddScoped<IQuestionService, QuestionService>();
            //servicesAddScoped<IValidator<CreatePollRequest>,CreatePollRequestValidator>(); 
            //servicesAddValidatorsFromAssemblyContaining<Program>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddFluentValidationAutoValidation();


            return services;
        }
        public static IServiceCollection AddAuthorazationsConfig(this IServiceCollection services,
            IConfiguration confg)
        {


            services.AddIdentity<ApplicationUser, IdentityRole>().

                   AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.Configure<JwtOptions>(confg.GetSection(JwtOptions.SectionName));//for program to meean this y ares

            var jwtsettings = confg.GetSection(JwtOptions.SectionName).Get<JwtOptions>(); //to get section of json 

            services.AddSingleton<IJwtProvider, JwtProvider>();

            services.AddAuthentication(options =>

            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsettings?.key!)),
                    ValidIssuer = jwtsettings?.Issuer,
                    ValidAudience = jwtsettings?.Audience
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(confg["Jwt:key"]!)),
                    //ValidIssuer = confg["Jwt:Issuer"],
                    //ValidAudience = confg["Jwt:Audience"]
                };

            }
            );
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8; // its deafult 6
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;

            });





            return services;
        }
        public static IServiceCollection AddBackgroundjobs(this IServiceCollection services,
        IConfiguration config)
        {
            services.AddHangfire(configuration => configuration
                 .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                 .UseSimpleAssemblyNameTypeSerializer()
                 .UseRecommendedSerializerSettings()
                 .UseSqlServerStorage(config.GetConnectionString("HangfireConnection")));

            // Add the processing server as IHostedService
            services.AddHangfireServer();



            return services;

        }

    }
}
