using Application.DTO.User;
using Application.Extensions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Application.Repositories
{
    public interface IUserRepository
    {
        Task<string> CreateTokenAsync(CreateUserDTO loginDTO);
        Task<IdentityResult> CreateUserAsync(CreateUserDTO createUserDTO);
        Task<bool> ValidateUserAsync(CreateUserDTO validateUserDTO);
        Task<UserDTO> GetLoggedInUserAsync();
    }
    public class UserRepository : IUserRepository
    {
        private readonly IArtworkProjectDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IGenericExtension _genericExtension;
        private User _user;

        public UserRepository(IGenericExtension genericExtension, IArtworkProjectDbContext context, UserManager<User> userManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _genericExtension = genericExtension;
            _userManager = userManager;
        }

        public async Task<User> GetCurrentUser()
        {
            return await _genericExtension.GetCurrentUserAsync();
        }

        public async Task<IdentityResult> CreateUserAsync(CreateUserDTO createUserDTO)
        {
            var user = createUserDTO.ToUser();
            user.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var result = await _context.Users.Where(x => x.UserName == user.UserName).FirstOrDefaultAsync();
            if (result != null)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "403 - Forbidden", Description = "Username already exists" });
            }
            return await _userManager.CreateAsync(user, createUserDTO.Password);
        }

        public async Task<bool> ValidateUserAsync(CreateUserDTO loginDTO)
        {
            _user = await _userManager.FindByNameAsync(loginDTO.UserName);
            var result = _user != null && await _userManager.CheckPasswordAsync(_user, loginDTO.Password);
            return result;
        }

        public async Task<string> CreateTokenAsync(CreateUserDTO loginDTO)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(loginDTO);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = _configuration.GetSection("jwtConfig");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(CreateUserDTO loginDTO)
        {
            _user ??= await _userManager.FindByNameAsync(loginDTO.UserName);
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, _user.UserName) };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtConfig");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiresIn"])),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

        public async Task<UserDTO> GetLoggedInUserAsync()
        {
            User currentUser = await GetCurrentUser();
            return new UserDTO { Id = currentUser.Id, CreatedAt = currentUser.CreatedAt, UserName = currentUser.UserName };
        }
    }
}
