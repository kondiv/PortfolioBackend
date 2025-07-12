using FluentAssertions;
using Domain.Entities;
using Services.Validators;
using Xunit;

namespace Tests.Validators
{
    public class ProjectValidatorTests
    {
        private const string ValidGithubReference = "https://github.com/kondiv/thoughtful";
        private const string InvalidGithubReference = "https://youtube.com";
        private const string InvalidGithubReferenceEmpty = "";
        private string InvalidGithubReferenceLong = "https://github.com/" + new string('a', 255);

        private const string ValidProjectTitle = "Title";
        private const string InvalidProjectTitleEmpty = "";
        private const string InvalidProjectTitleShort = "t";
        private string InvalidProjectTitleLong = new string('s', 51);

        private const string ValidProjectDescription = "Description";
        private const string InvalidProjectDescriptionEmpty = "";
        private const string InvalidProjectDescriptionShort = "Desc";
        private string InvalidProjectDescriptionLong = new string('a', 1001);

        [Fact]
        public void ProjectValidator_WhenModelIsValid_IsErrorFalse()
        {
            // Arrange
            var validator = new ProjectValidator();

            var validProjectModel = new Project
            {
                ProjectId = Guid.NewGuid(),
                Title = ValidProjectTitle,
                Description = ValidProjectDescription,
                GithubReference = ValidGithubReference
            };

            // Act
            var validationResult = validator.Validate(validProjectModel);

            // Assert
            validationResult.Errors.Should().HaveCount( 0 );
            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ProjectValidator_WhenModelFieldsAreEmpty_IsErrorTrue()
        {
            // Arrange
            var validator = new ProjectValidator();

            var invalidProjectModel = new Project
            {
                ProjectId = Guid.NewGuid(),
                Title = InvalidProjectTitleEmpty,
                Description = InvalidProjectDescriptionEmpty,
                GithubReference = InvalidGithubReferenceEmpty
            };

            // Act
            var validationResult = validator.Validate(invalidProjectModel);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().HaveCount(3);
            foreach (var error in validationResult.Errors)
            {
                error.ErrorMessage.Should().Be("Empty");
            }
        }

        [Fact]
        public void ProjectValidator_WhenModelFieldsAreShort_IsErrorTrue()
        {
            // Arrange
            var validator = new ProjectValidator();

            var invalidProjectModel = new Project
            {
                ProjectId = Guid.NewGuid(),
                Title = InvalidProjectTitleShort,
                Description = InvalidProjectDescriptionShort,
                GithubReference = ValidGithubReference
            };

            // Act
            var validationResult = validator.Validate(invalidProjectModel);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().HaveCount(2);
            foreach (var error in validationResult.Errors)
            {
                error.ErrorMessage.Should().Be("Wrong length");
            }
        }

        [Fact]
        public void ProjectValidator_WnehModelFieldsAreLong_IsErrorTrue()
        {
            var validator = new ProjectValidator();

            var invalidProjectModel = new Project
            {
                ProjectId = Guid.NewGuid(),
                Title = InvalidProjectTitleLong,
                Description = InvalidProjectDescriptionLong,
                GithubReference = InvalidGithubReferenceLong
            };

            // Act
            var validationResult = validator.Validate(invalidProjectModel);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().HaveCount(3);
            foreach (var error in validationResult.Errors)
            {
                error.ErrorMessage.Should().Be("Wrong length");
            }
        }

        [Fact]
        public void ProjectValidator_WhenGithubReferenceIsWrong_IsErrorTrue()
        {
            var validator = new ProjectValidator();

            var invalidProjectModel = new Project
            {
                ProjectId = Guid.NewGuid(),
                Title = ValidProjectTitle,
                Description = ValidProjectDescription,
                GithubReference = InvalidGithubReference
            };

            // Act
            var validationResult = validator.Validate(invalidProjectModel);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().HaveCount(1);
            foreach (var error in validationResult.Errors)
            {
                error.ErrorMessage.Should().Be("Not github reference");
            }
        }
    }
}
