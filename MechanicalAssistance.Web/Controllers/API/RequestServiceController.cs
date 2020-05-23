﻿using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Web.Data;
using MechanicalAssistance.Web.Data.Entities;
using MechanicalAssistance.Web.Helpers;
using MechanicalAssistance.Web.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Controllers.API
{
    [Route("api/[Controller]")]
    public class RequestServiceController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;

        public RequestServiceController(DataContext dataContext, IUserHelper userHelper, IConverterHelper converterHelper,
            IImageHelper imageHelper, IMailHelper mailHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("NewRequest")]
        public async Task<IActionResult> NewRequest([FromBody] RequestServiceRequest RequestService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }


            CultureInfo cultureInfo = new CultureInfo(RequestService.CultureInfo);
            Resource.Culture = cultureInfo;

            MechanicalServiceEntity serviceEntity = await _dataContext.Services.Include(u => u.User).FirstOrDefaultAsync(s=> s.Id == RequestService.ServiceId);

            if (serviceEntity == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = Resource.ServiceValidation
                });
            }

            UserEntity userEntity = await _userHelper.GetUserAsync(RequestService.UserId);
            if (userEntity == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = Resource.userValidator
                });
            }

            string picturePath = string.Empty;
            if (RequestService.Photo != null && RequestService.Photo.Length > 0)
            {
                picturePath = _imageHelper.UploadImage(RequestService.Photo, "Request");
            }

            RequestServiceEntity reService = new RequestServiceEntity
            {
                DateAndTime = RequestService.DateAndTime,
                RequestPhoto = picturePath,
                Observation = RequestService.Observation,
                Status = RequestService.Status,
                Service = await _dataContext.Services.FirstOrDefaultAsync(s => s.Id == RequestService.ServiceId),
                User = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == RequestService.UserId.ToString()),
            };

            _dataContext.RequestServices.Add(reService);
            await _dataContext.SaveChangesAsync();

            InternetAddressList list = new InternetAddressList();
            list.Add(new MailboxAddress(userEntity.Email));
            list.Add(new MailboxAddress(serviceEntity.User.Email));
            
            _mailHelper.SendMultipleEmails(list, Resource.subjectP, $"<h1>{Resource.subjectP +" "+ serviceEntity.ServiceName }</h1>" +
               $"{Resource.MessageF+" "+serviceEntity.ServiceName+" "+Resource.MessageS+" "+userEntity.FullName+". "+ Resource.MessageT + "<strong>"+RequestService.Status + "</strong>. "+Resource.MessaFo + " "}<img src=\"https://mechanicalassistancewebk.azurewebsites.net/images/ErrorNotFound.png\" >");

            return Ok(new Response
            {
                IsSuccess = true,
                Message = Resource.RequestMessage
            });

        }


    }
}
