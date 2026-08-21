using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;


using MassTransit;
using Notification.Consumer;
using Notification.Service;
using Notification.ServicesAbstract;
using Notification.Settings;

namespace NotificationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<SendOtpConsumer>();
                x.AddConsumer<ResetPasswordConsumer>();
                x.AddConsumer<AccountPendingConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {

                    cfg.Host(
                        "localhost",
                        "/",
                        h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        });


                    cfg.ConfigureEndpoints(context);

                });
            });
            

            builder.Services.AddScoped<ISmsService, TwilioSmsService>();
            builder.Services.Configure<MailSettings>(builder.Configuration .GetSection("MailSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IFireBaseService, FireBaseService>();

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("Firebase/firebase-adminsdk.json")
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
