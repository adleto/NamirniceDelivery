using Microsoft.AspNetCore.Http;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Services
{
    public class UserResolverService:IUserResolverService
    {
        private readonly IHttpContextAccessor _context;
        public UserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetUsernameOfCurrentUser()
        {
            return _context.HttpContext.User?.Identity?.Name;
        }
    }
}
