using Amazon_Replica.Areas.Identity.Data;
using Amazon_Replica.Data;
using Amazon_Replica.Models;
using Amazon_Replica.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Configuration;

namespace Amazon_Replica
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<IProductRepo, ProductRepo>();
            builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
            builder.Services.AddScoped<ICartRepo, CartRepo>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();

            builder.Services.AddControllersWithViews();

            //for session
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
            });

            //Strip
            builder.Services.Configure<StripSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = "sk_test_51MzpgEGVTyUjKHngk6bd1FbFSsDzUdeotl4ZifGmDOFThnjcXf4oqQ1oh8wtqmmBDgkKuwBl8Z2X5PYQI7mr5qEK00C2OOYwyo";
            //strip


            //Google & Face Book
            builder.Services.AddAuthentication()
                .AddGoogle("google", opt =>
                {
                    var googleAuth = builder.Configuration.GetSection("Authentication:Google");
                    opt.ClientId = googleAuth["ClientId"];
                    opt.ClientSecret = googleAuth["ClientSecret"];
                    opt.SignInScheme = IdentityConstants.ExternalScheme;
                }).AddFacebook(options =>
                {
                    var FBAuth = builder.Configuration.GetSection("Authentication:FB");
                    options.ClientId = FBAuth["ClientId"];
                    options.ClientSecret = FBAuth["ClientSecret"];
                });
            //Google & Face Book

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Store}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.UseSession();

            using ( var scope = app.Services.CreateScope())
            {
                var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new List<string> { "Admin", "Moderator", "Customer" };

                foreach (var role in roles)
                {
                    if (!await roleManger.RoleExistsAsync(role))
                        await roleManger.CreateAsync(new IdentityRole(role));
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var email = "admin@admin.com";
                var password = "Test@1234";

                if (await userManger.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser() {FirstName = "admin", LastName = "admin", Birthdate = DateTime.Now, UserName = email, Email = email };

                    await userManger.CreateAsync(user, password);
                    await userManger.AddToRoleAsync(user, "Admin");
                }
            }

            app.Run();
        }
    }
}