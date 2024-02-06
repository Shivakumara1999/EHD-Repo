using EHD.BAL.Domain_Models;
using EHD.BAL.Interface;
using EHD.DAL.DataContext;
using EHD.DAL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static EHD.BAL.Domain_Models.AccountsDTO;
using static EHD.BAL.Exceptions.AllExceptions;

namespace EHD.BAL.Implementations
{
    public class AccountRepo : IAccount
    {
        private readonly EHDContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMailTemplate _mail;

        public AccountRepo(EHDContext dbContext, IConfiguration configuration, IMailTemplate mail)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _mail = mail;
        }
        public string Login(LoginDTO model)
        {
            var user = _dbContext.employees.FirstOrDefault(i => i.OfficialMailId == model.OfficialMailId && i.IsActive==true);

            if (user == null)
            {
                throw new UserNotFound();
            }
            var query = (from d in _dbContext.departments
                         join r in _dbContext.roles on d.DepartmentId equals r.DepartmentId
                         join e in _dbContext.employees on r.RoleId equals e.RoleId
                         where r.RoleId == user.RoleId
                         select new DepartmentIdNameDto
                         {
                             DepartmentId = d.DepartmentId,
                             DepartmentName = d.DepartmentName
                         }).ToList();
            var hashedPasswordFromDB = user.Password;
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, hashedPasswordFromDB);

            if (!isPasswordValid)
            {
                throw new InvalidPassw();
            }
            else
            {
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Email, model.OfficialMailId),
                    new Claim("OfficialMailId", user.OfficialMailId),
                    new Claim("RoleId", user.RoleId),
                    new Claim("EmployeeId",user.EmployeeId)
                };
                if (query != null && query.Count > 0)
                {
                    claims.Add(new Claim("DepartmentId", query[0].DepartmentId));
                }
                else
                {
                    claims.Add(new Claim("DepartmentId", " "));
                }
                var tokenKey = _configuration.GetSection("azure:secretkey").Value!;
                var keyBytes = Encoding.UTF8.GetBytes(tokenKey);
                var sha512 = SHA512.Create();
                var hashedKeyBytes = sha512.ComputeHash(keyBytes);
                var newKey = new SymmetricSecurityKey(hashedKeyBytes);
                var creds = new SigningCredentials(newKey, SecurityAlgorithms.HmacSha512);
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwt = jwtHandler.WriteToken(token);
                return jwt;

            }
        }

        public async Task OtpGeneration(string email)
        {

            var emailCheck =  _dbContext.employees.FirstOrDefault(e => e.OfficialMailId.Equals(email));
            Random random = new Random();
            int otp = (random.Next(1000, 9999));

            if (emailCheck == null)
            {
                throw new UserNotFound();
            }
            if (emailCheck.OfficialMailId != null)
            {
                emailCheck.Otp = otp;
                await _dbContext.SaveChangesAsync();

                var otpData = new MailTemplateDTO
                {
                    ToAddress = emailCheck.OfficialMailId,
                    Subject = "HelpDesk Password Reset OTP",
                    MailHeader = emailCheck.Otp.ToString(),
                    MailBody = $"Dear, {emailCheck.FirstName + " " + emailCheck.LastName}, <br></br> This is the Otp to reset your password",
                    MailFooter = "From Joy Help Desk team! "
                };
                await _mail.SendMail(otpData);
            }

            await Task.Delay(TimeSpan.FromMinutes(1));
            emailCheck.Otp = null;
            await _dbContext.SaveChangesAsync();    
        }

        public void ForgotPassword(ForgotPasswordDTO forgot)
        {
            var data = _dbContext.employees.FirstOrDefault(i => i.Otp == forgot.Otp && i.OfficialMailId == forgot.OfficialMailId);
            if (data != null)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(forgot.newPassword);
                data.Password = passwordHash;
                data.Otp = null;
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Invalidotp();
            }
        }
        public void ChangePassword(ChangePasswordDTO change)
        {
            var data = _dbContext.employees.FirstOrDefault(i => i.OfficialMailId == change.OfficialMailId);
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(change.oldpassword, data.Password);
            if (!isPasswordValid)
            {
                throw new InvalidPassw();
            }
            else
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(change.newPassword);
                data.Password = passwordHash;
                _dbContext.SaveChanges();
            }
        }
    }
}
