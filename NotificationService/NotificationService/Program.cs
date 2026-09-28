using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MassTransit;
using Notification.Consumer;
using Notification.Service;
using Notification.ServicesAbstract;
using Notification.Settings;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


namespace NotificationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHealthChecks();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<SendOtpConsumer>();
                x.AddConsumer<ResetPasswordConsumer>();
                x.AddConsumer<AccountPendingConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    // Was hardcoded to "localhost"/"guest"/"guest" — the manifest already
                    // sets RabbitMq__Host/Username/Password, this just reads them.
                    var host = builder.Configuration["RabbitMq:Host"] ?? "localhost";
                    var username = builder.Configuration["RabbitMq:Username"] ?? "guest";
                    var password = builder.Configuration["RabbitMq:Password"] ?? "guest";

                    cfg.Host(host, "/", h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            builder.Services.AddScoped<ISmsService, TwilioSmsService>();
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IFireBaseService, FireBaseService>();

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("Firebase/firebase-adminsdk.json")
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/health", new HealthCheckOptions { Predicate = _ => false });
            app.Run();
        }
    }
}