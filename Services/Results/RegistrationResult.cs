using Domain.Dto;
using Domain.Entities;

namespace Services.Results;

public sealed class RegistrationResult
{
   private List<Error> _errors = new List<Error>();
   
   public UserDto? User { get; private init; }
      
   public IReadOnlyCollection<Error> Errors => _errors.AsReadOnly();
   
   public bool Succeeded { get; private init; }

   public static RegistrationResult Created(UserDto user)
   {
      return new RegistrationResult(){User = user, _errors = [], Succeeded = true};
   }

   public static RegistrationResult Failed(List<Error> errors)
   {
      return new RegistrationResult(){User = null, _errors = errors, Succeeded = false};
   }

   public RegistrationResult WithTokens(string accessToken, RefreshToken refreshToken)
   {
      return new RegistrationResult()
      {
         User = this.User,
         Succeeded = this.Succeeded,
         _errors = new List<Error>(this._errors)
      };
   }
} 