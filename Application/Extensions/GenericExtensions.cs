using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Extensions
{
    public interface IGenericExtension
    {
        //Task<Type> NameOfExtensionFunctionAsync()
    }
    public class GenericExtensions : IGenericExtension
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IArtworkProjectDbContext _context;
        public GenericExtensions(IHttpContextAccessor httpContextAccessor, IArtworkProjectDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
    }
}
