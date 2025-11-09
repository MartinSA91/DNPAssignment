namespace ApiContracts;


    public record UserCreateDto(string UserName, string Password);
    public record UserUpdateDto(int Id, string UserName, string Password);
    public record UserDto(int Id, string UserName);
    public record UserLoginDto(string UserName, string Password);
    public record LoginRequestDto(string UserName, string Password);
    public record LoginResponseDto(string Token, int UserId, string UserName);
