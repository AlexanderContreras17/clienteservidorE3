using FluentValidation;
using project_api.Models.Dtos;
namespace project_api.Validators
	
{
	public class LoginValidatorDto:AbstractValidator<LoginDto>
	{
		public LoginValidatorDto() { 
		RuleFor(x=>x.UserName).EmailAddress().WithMessage("Ingrese un correo electrónico valido")
				.NotEmpty().NotNull().WithMessage("Ingrese su correo electrónico");
			RuleFor(x => x.Password)
				.NotEmpty().NotNull().WithMessage("Ingrese su contraseña");

		}
	}
}
