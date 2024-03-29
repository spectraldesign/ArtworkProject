﻿using Application.DTO.User;
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
        Task<bool> CheckTokenValidity(VerifyTokenDTO verifyTokenDTO);
        Task<UserDTO> GetLoggedInUserAsync();
        Task<int> DeleteUserAsync(string id);
        Task<IdentityResult> UpdateUserAsync(UpdateUserDTO updateUserDTO);
        Task<UserDTO> GetUserByUsername(string username);
    }
    public class UserRepository : IUserRepository
    {
        private readonly IArtworkProjectDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IGenericExtension _genericExtension;
        private User _user;

        public UserRepository(
            IGenericExtension genericExtension,
            IArtworkProjectDbContext context,
            UserManager<User> userManager,
            IConfiguration configuration
            )
        {
            _configuration = configuration;
            _dbContext = context;
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
            var result = await _dbContext.Users.Where(x => x.UserName == user.UserName).FirstOrDefaultAsync();
            if (result != null)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "403", Description = "Username already exists" });
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

            _user.ActiveToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            await _dbContext.SaveChangesAsync();

            return _user.ActiveToken;
        }

        public async Task<bool> CheckTokenValidity(VerifyTokenDTO verifyDTO)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(verifyDTO.Username)
                    ?? throw new Exception($"No user with username: '{verifyDTO.Username}' exists!");
                return user.ActiveToken!.Equals(verifyDTO.Token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
            return new UserDTO { Id = currentUser.Id, CreatedAt = currentUser.CreatedAt, UserName = currentUser.UserName, ProfilePicture = currentUser.ProfilePicture };
        }

        public async Task<int> DeleteUserAsync(string id)
        {
            User? dbUser = await _dbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (dbUser == null)
            {
                return -1;
            }
            //In case we want admin accounts in the future this check can be changed, for now you should only be able to delete own account.
            User loggedInUser = await GetCurrentUser();
            if (loggedInUser.Id != dbUser.Id)
            {
                return -2;
            }
            //Need to delete all the content belonging to User
            _dbContext.Likes.RemoveRange(_dbContext.Likes.Where(x => x.User.Id == id));
            _dbContext.Comments.RemoveRange(_dbContext.Comments.Where(x => x.Creator.Id == id));
            _dbContext.Images.RemoveRange(_dbContext.Images.Where(x => x.Creator.Id == id));

            //Remove user
            _dbContext.Users.Remove(dbUser);

            //Save db changes
            var res = await _dbContext.SaveChangesAsync();
            return res;
        }

        public async Task<IdentityResult> UpdateUserAsync(UpdateUserDTO updateUserDTO)
        {
            User? dbUser = await _dbContext.Users.Where(x => x.Id == updateUserDTO.Id).FirstOrDefaultAsync();
            if (dbUser == null)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "404", Description = $"User with id: {updateUserDTO.Id} was not found in database" });
            }
            User loggedInUser = await GetCurrentUser();
            if (loggedInUser.Id != dbUser.Id)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "403", Description = "Can only update your own user." });
            }
            if (updateUserDTO.UserName != null)
            {
                dbUser.UserName = updateUserDTO.UserName;
            }
            if (updateUserDTO.Password != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(dbUser);
                IdentityResult passwordChangeResult = await _userManager.ResetPasswordAsync(dbUser, resetToken, updateUserDTO.Password);
            }
            if (updateUserDTO.ProfilePicture != null)
            {
                dbUser.ProfilePicture = updateUserDTO.ProfilePicture;
            }
            IdentityResult result = await _userManager.UpdateAsync(dbUser);
            return result;
        }

        public async Task<UserDTO> GetUserByUsername(string username)
        {
            User dbUser = await _dbContext.Users
                .Include(u => u.Images)
                .FirstOrDefaultAsync(u => u.UserName == username)
                ?? throw new ArgumentException($"No user with name: {username}");
            UserDTO userDTO = new()
            {
                CreatedAt = dbUser.CreatedAt,
                Id = dbUser.Id,
                ProfilePicture = dbUser.ProfilePicture,
                UserName = dbUser.UserName!,
            };
            return userDTO;
        }
    }
}
