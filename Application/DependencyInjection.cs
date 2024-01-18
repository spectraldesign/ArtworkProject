using Application.Extensions;
using Application.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddTransient<UserManager<User>>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ILikeRepository, LikeRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IGenericExtension, GenericExtensions>();
            return services;
        }
    }
}
