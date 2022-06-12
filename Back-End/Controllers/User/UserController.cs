﻿using Microsoft.AspNetCore.Mvc;
using AnL.Repository.Abstraction;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using System.Net.Mail;
using System;
using AnL.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using AnL.Constants;
using AnL.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace TestApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    
    public class UserController : Controller
    {
        private readonly IUnitOfWork _UOW;
        private readonly IConfiguration _configuration;

        public UserController(IUnitOfWork UOW, IConfiguration configuration)
        {
            this._UOW = UOW;
            this._configuration = configuration;
          
        }
        /// <summary>
        /// Generates OTP using email address.
        /// </summary>
        /// <param name="mailto"></param>
        /// <returns>OTP</returns>
        // GET: api/Employee
        [HttpPost]
        public async Task<ActionResult> GenerateOTP(string mailto)
        {
            try
            {
                Random rnd = new Random();
                BaseResponse baseResponse = new BaseResponse();
                int otp = rnd.Next(10000, 99999);
                string msg = "Your otp from test.com is " + otp;
                bool result = SendOTP(mailto.Trim(), "Subjected to OTP", msg);
                var details = await _UOW.UserRepository.GetLogin(mailto.Trim());
                
                if (details == null)
                {
                    baseResponse.Data = details;
                    baseResponse.ResponseCode = HTTPConstants.BAD_REQUEST;
                    baseResponse.ResponseMessage = MessageConstants.InvalidEmailAddress;
                    return BadRequest(baseResponse);
                }
                else if (result == false)
                {
                    baseResponse.Data = result;
                    baseResponse.ResponseCode = HTTPConstants.BAD_REQUEST;
                    baseResponse.ResponseMessage = MessageConstants.GenerateOTPFailed;
                    return BadRequest(baseResponse);

                }
                else
                {
                    if (details.IsActive)
                    {
                        var response = await _UOW.UserRepository.UpdateOTP(otp, details.UserId);
                        baseResponse.Data = response;
                        baseResponse.ResponseCode = HTTPConstants.OK;
                        baseResponse.ResponseMessage = MessageConstants.GenerateOTPSuccess;
                        return Ok(baseResponse);

                    }
                    else
                    {
                        baseResponse.Data = details;
                        baseResponse.ResponseCode = HTTPConstants.BAD_REQUEST;
                        baseResponse.ResponseMessage = MessageConstants.UserNotActive;
                        return BadRequest(baseResponse);

                    }
                    
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }

        }
        [HttpGet]
        public bool SendOTP(string mailto, string subject, string body)
        {
            bool f = false;
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(mailto);
                mailMessage.From = new MailAddress(_configuration["Smtp:Sender"]);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient()
                {
                    Host = _configuration["Smtp:Server"],
                    Port = Convert.ToInt32(_configuration["Smtp:Port"]),
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential(_configuration["Smtp:Sender"], _configuration["Smtp:Password"])

                };
                smtp.Send(mailMessage);
                f = true;
            }
            catch (Exception ex)
            {
                f = false;
            }
            return f;
        }
        [HttpGet("{finalDigit}/{username}")]
        public async Task<IActionResult> SubmitOTP(int finalDigit,string username)
        {
            try
            {
                var details = await _UOW.UserRepository.GetLogin(username);
                var EmployeeDetails = _UOW.EmployeeDetailsRepository.GetById(details.UserId);
                

                LoginViewModel loginView = new LoginViewModel();
                loginView.EmployeeId = EmployeeDetails.EmployeeId;
                loginView.Username = details.Username;
                loginView.Token = details.Token;
                loginView.Otp = details.Otp;
                loginView.TokenExpiryDate = details.TokenExpiryDate;
                loginView.OtpexpiryDate = details.OtpexpiryDate;
                loginView.IsActive = details.IsActive;
                loginView.FirstName = EmployeeDetails.FirstName;
                loginView.LastName = EmployeeDetails.LastName;
                loginView.ManagerId = EmployeeDetails.ManagerId;
                loginView.SupervisorFlag = EmployeeDetails.SupervisorFlag;
                
                
                BaseResponse baseResponse = new BaseResponse();
                TimeSpan result = DateTime.UtcNow - details.OtpexpiryDate;
                //int a = Convert.ToInt32(TempData["otp"]);
                if (finalDigit == null)
                {
                    baseResponse.Data = finalDigit;
                    baseResponse.ResponseCode = HTTPConstants.BAD_REQUEST;
                    baseResponse.ResponseMessage = MessageConstants.InvalidOTP;
                    return Ok(baseResponse);
                }
                else if (result.TotalSeconds > 90)
                {
                    baseResponse.Data = result;
                    baseResponse.ResponseCode = HTTPConstants.BAD_REQUEST;
                    baseResponse.ResponseMessage = MessageConstants.OTPTimedOUT;
                    return Ok(baseResponse);
                }
                else if (finalDigit.ToString() == Convert.ToString(details.Otp))
                {
                    baseResponse.Data = loginView;
                    baseResponse.ResponseCode = HTTPConstants.OK;
                    baseResponse.ResponseMessage = MessageConstants.LoginSuccess;
                    return Ok(baseResponse);
                }
                else
                {
                    return BadRequest("Please Enter Valid OTP");
                }
                //return Ok();
            }
            catch(Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }
        }
    }
}

