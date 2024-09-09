using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RidewareEmployeeMgt.Data;
using RidewareEmployeeMgt.Interfaces;
using RidewareEmployeeMgt.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RidewareEmployeeMgt.Services
{
    public class UserServices : IUserServices
    {
        public readonly DbContextClass _dbcontext;
        public readonly IConfiguration config;
        private readonly ILogger<UserServices> _logger;

        public UserServices(DbContextClass dbcontext, IConfiguration config, ILogger<UserServices> logger)
        {
            _dbcontext = dbcontext;
            this.config = config;
            _logger = logger;
        }
        public async Task<string> RegisterUser(UserDto user)
        {
            try
            {
                var isExist = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (isExist != null)
                {
                    _logger.LogInformation("user already exist.");
                    return null;
                }
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
                var userr = new User { Email = user.Email, Password = hashPassword };

                await _dbcontext.AddAsync(userr);
                await _dbcontext.SaveChangesAsync();
                _logger.LogInformation("user registration completed");
                return "User added successfully";


            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> LogIn(UserDto user)
        {
            try
            {
                var isExist = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (isExist == null)
                {
                    return null;
                }
                bool validatePassword = BCrypt.Net.BCrypt.Verify(user.Password, isExist.Password);
                if (validatePassword)
                {
                    var token = GenerateToken(isExist);
                    return token;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("error occured while login");
                throw;
            }
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Iss,config["jwt:issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud,config["jwt:audience"]),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role),
            };
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1));
            return new JwtSecurityTokenHandler().WriteToken(token);

            //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:key"]));
            //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //    var claims = new[]
            //    {
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(ClaimTypes.Name, user.Email),
            //    new Claim(ClaimTypes.Role, user.Role),
            //};

            //    var token = new JwtSecurityToken(
            //        claims: claims,
            //        signingCredentials: credentials,
            //        expires: DateTime.UtcNow.AddHours(1)

            //    );

            //    return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
