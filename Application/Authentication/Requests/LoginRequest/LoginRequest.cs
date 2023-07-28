using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Application.Authentication.Requests.LoginRequest
{
    public record LoginRequest([property: Required, EmailAddress]
    string Email, [property: Required, DataType(DataType.Password)]
    string Password, string ReturnUrl) : IRequest<SignInResult>
    {
        public bool RememberMe { get; set; } = false;
    }
}
