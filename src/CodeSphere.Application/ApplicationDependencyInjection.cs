using CodeSphere.Application.Common;
using CodeSphere.Application.Service;
using CodeSphere.Application.Service.Impl;
using CodeSphere.DataAccess.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MinioSettings>(configuration.GetSection("MinioSettings"));
            services.AddServices(configuration);
            return services;
        }

        private static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFileStoreageService, MinioFileStorageService>();

            services.AddSingleton<IMinioClient>(sp =>
            {
                var minioSettings = sp.GetRequiredService<IOptions<MinioSettings>>().Value;

                if (string.IsNullOrEmpty(minioSettings?.Endpoint))
                    throw new Exception("MinioSettings.Endpoint is required.");

                var client = new MinioClient()
                    .WithEndpoint(minioSettings.Endpoint)
                    .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey);

                if (minioSettings.UseSsl)
                {
                    client = client.WithSSL();
                }

                return client.Build();
            });
        }
    }
}
