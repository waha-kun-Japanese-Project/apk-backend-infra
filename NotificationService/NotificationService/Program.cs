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

            var firebasePath = Path.Combine(
                            builder.Environment.ContentRootPath,
                            "FireBase",
                            "graduation-project-3c67f-firebase-adminsdk-fbsvc-b88880ea28.json");

            FirebaseApp.Create(new AppOptions
            {
                // k8s mounts the key at /app/FireBase/...; locally fall back to the AuthService copy.
                Credential = CredentialFactory.FromFile<ServiceAccountCredential>(File.Exists(firebasePath)
                    ? firebasePath
                    : "../../AuthService/AuthService/FireBase/graduation-project-3c67f-firebase-adminsdk-fbsvc-b88880ea28.json").ToGoogleCredential()
            });
            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();

            if (!app.Environment.IsProduction())
            {
                app.UseHttpsRedirection();
            }
            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/health", new HealthCheckOptions { Predicate = _ => false });
            app.Run();
        }
    }
}