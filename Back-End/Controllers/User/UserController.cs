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
            Random rnd = new Random();
            BaseResponse baseResponse = new BaseResponse();
            int otp = rnd.Next(10000, 99999);
            string msg = "Your otp from test.com is " + otp;
            bool result = SendOTP(mailto, "Subjected to OTP", msg);
            var details = await _UOW.UserRepository.GetLogin(mailto);
            if(details==null)
            {
                baseResponse.Data = details;
                baseResponse.ResponseCode = HTTPConstants.BAD_REQUEST;
                baseResponse.ResponseMessage = MessageConstants.GenerateOTPFailed;
                return BadRequest(baseResponse);
            }
            else
            {
                var response = await _UOW.UserRepository.UpdateOTP(otp, details.UserId);
                baseResponse.Data = response;
                baseResponse.ResponseCode = HTTPConstants.OK;
                baseResponse.ResponseMessage = MessageConstants.GenerateOTPSuccess;
                return Ok(baseResponse);
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
            var details = await _UOW.UserRepository.GetLogin(username);

            TimeSpan result = DateTime.UtcNow - details.OtpexpiryDate;
            //int a = Convert.ToInt32(TempData["otp"]);
            if (finalDigit == null)
            {
                return NoContent();
            }          
            else if (result.TotalSeconds>30 )
            { 
                return BadRequest("OTP timed OUT");
            }
            else if (finalDigit.ToString() == Convert.ToString(details.Otp))
            {
                return Ok(details);
            }
            else
            {
                return BadRequest("Please Enter Valid OTP");
            }
            //return Ok();
        }

    }
}

