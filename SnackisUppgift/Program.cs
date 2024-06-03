using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SnackisUppgift.Areas.Identity.Data;
using SnackisUppgift.Data;
namespace SnackisUppgift
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
   var connectionString = builder.Configuration.GetConnectionString("SnackisUppgiftContextConnection") ?? throw new InvalidOperationException("Connection string 'SnackisUppgiftContextConnection' not found.");

   builder.Services.AddDbContext<SnackisUppgiftContext>(options => options.UseSqlServer(connectionString));


			builder.Services.AddDefaultIdentity<SnackisUppgiftUser>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddRoles<IdentityRole>() 
			.AddEntityFrameworkStores<SnackisUppgiftContext>();

			builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy("AdminKrav",
					policy => policy.RequireRole("Admin"));
			});
			
			builder.Services.AddRazorPages(options =>
			{
				options.Conventions.AuthorizeFolder("/Secret");
				options.Conventions.AuthorizeFolder("/Secret", "AdminKrav");
			});

			var app = builder.Build();

			
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}


			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
	}
}