using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ERP.Shared;
using ERP.Shared.Messages;

namespace ERP.Application.Requests.AuthenticationRequests;

public class LogInRequest : BaseRequestWithValidation
{
    public LogInRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }

    [Required]
    [MaxLength(100)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; }

    [Required]
    [MinLength(4)]
    [DataType(DataType.Password)]
    public string Password { get; }
    
    public override async Task ValidateAsync() =>
        ValidationResult = await LazyValidator.ValidateAsync<LogInRequestValidator>(this);

}
