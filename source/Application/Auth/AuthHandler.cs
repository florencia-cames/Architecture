namespace Architecture.Application;

public sealed record AuthHandler : IHandler<AuthRequest, AuthResponse>
{
    private readonly IAuthRepository _authRepository;
    private readonly IHashService _hashService;
    private readonly IJwtService _jwtService;
    private readonly IResultService _resultService;

    public AuthHandler
    (
        IAuthRepository authRepository,
        IHashService hashService,
        IJwtService jwtService,
        IResultService resultService
    )
    {
        _authRepository = authRepository;
        _hashService = hashService;
        _jwtService = jwtService;
        _resultService = resultService;
    }

    public async Task<Result<AuthResponse>> HandleAsync(AuthRequest request)
    {
        var error = _resultService.Error<AuthResponse>("AuthError");

        var auth = await _authRepository.GetByLoginAsync(request.Login);

        if (auth is null) return error;

        var password = _hashService.Create(request.Password, auth.Salt.ToString());

        if (auth.Password != password) return error;

        var claims = new List<Claim>();

        claims.AddSub(auth.Id.ToString());

        claims.AddRoles(auth.Roles.ToArray());

        var token = _jwtService.Encode(claims);

        var response = new AuthResponse(token);

        return _resultService.Success(response);
    }
}
