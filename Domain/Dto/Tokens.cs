using Domain.Entities;

namespace Domain.Dto;

public record Tokens(string AccessToken, RefreshToken RefreshToken);