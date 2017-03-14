﻿using AutoMapper;
using System.Web.Http;
using UoW.Interface;
using UserAccountFacade.Interface;
using UserAccountServiceNameSpace.Interface;
using Viewmodels.UserAccount;
using System;
using System.Threading.Tasks;
using Exceptions.Validation;
using MessageBuilder;

namespace NetForumApi.Controllers.UserAccountControllers
{
    [RoutePrefix("api/register")]
    public class RegisterController : ApiController
    {
        private readonly IRegisterFacade _registerFacade;

        public RegisterController(IUnitOfWork uow, IMapper automapper, IRegisterService registerService, IRegisterFacade registerFacade)
        {
            _registerFacade = registerFacade;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _registerFacade.Dispose();

            base.Dispose(disposing);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("")]
        public async Task<IHttpActionResult> Post(UserViewmodel user)
        {
            try
            {
                var identityResult = await _registerFacade.Register(user, user.Password);
                if (identityResult.Succeeded)
                    return Ok("Registration successful!");
                else
                    return BadRequest(ErrorMessageBuilder.BuildErrorMessage("Registration failed: ", identityResult.Errors));
            }
            catch (ServerValidationException serverExc)
            {
                return BadRequest(serverExc.Message);
            }
            catch(Exception ex)
            {
                return BadRequest($"Something unexpected happened: {ex.Message}. Try to reload this page.");
            }
            
        }
    }
}