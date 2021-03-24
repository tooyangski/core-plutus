using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ProjectPlutus.API.ViewModels;
using ProjectPlutus.Domain.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectPlutus.API.Filters
{
    public class UserResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(
            ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;

            if (resultFromAction?.Value == null
                || resultFromAction.StatusCode < 200
                || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();

            var value = resultFromAction.Value;

            if (value is IList && value.GetType().IsGenericType)
                resultFromAction.Value = mapper.Map<IEnumerable<UserViewModel>>(value);
            else if (value is User)
                resultFromAction.Value = mapper.Map<UserViewModel>(value);
            
            await next();
        }
    }
}
