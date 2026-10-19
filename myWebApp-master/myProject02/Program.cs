using Microsoft.EntityFrameworkCore;
using myProject02.Models;
using myProject02.Services;

using QuestPDF.Infrastructure;




namespace myProject02
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<Ivoterservice, VoterService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IPdfService, PdfService>();

            builder.Services.AddScoped<ICandidateService, CandidateService>();
            builder.Services.AddScoped<IPartyService, PartyService>();

            builder.Services.AddScoped<IElectionService, ElectionService>();

            QuestPDF.Settings.License = LicenseType.Community;


            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("VoterConnection")));

            //connect user ui with api

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactPolicy", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.UseRouting();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //connect ui with ui
            app.UseCors("ReactPolicy");

            app.MapControllers();

            app.Run();
        }
    }
}
