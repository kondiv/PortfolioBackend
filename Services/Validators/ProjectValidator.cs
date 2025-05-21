using FluentValidation;
using Models.Entities;

namespace Services.Validators
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        const string githubAddress = "https://github.com/";

        public ProjectValidator()
        {
            RuleFor(p => p.Title)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Empty")
                .NotEmpty().WithMessage("Empty")
                .MinimumLength(2).WithMessage("Wrong length")
                .MaximumLength(50).WithMessage("Wrong length");

            RuleFor(p => p.Description)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Empty")
                .NotEmpty().WithMessage("Empty")
                .MinimumLength(10).WithMessage("Wrong length")
                .MaximumLength(1000).WithMessage("Wrong length");

            RuleFor(p => p.GithubReference)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Empty")
                .NotEmpty().WithMessage("Empty")
                .MaximumLength(255).WithMessage("Wrong length")
                .Must(ContainGithubAddress).WithMessage("Not github reference");
        }

        private bool ContainGithubAddress(string address)
        {
            return address.StartsWith(githubAddress, StringComparison.Ordinal);
        }
    }
}
