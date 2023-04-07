namespace Architecture.Application;

public sealed record AddUserHandler : IHandler<AddUserRequest, long>
{
    private readonly IAuthRepository _authRepository;
    private readonly IHashService _hashService;
    private readonly IResultService _resultService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public AddUserHandler
    (
        IAuthRepository authRepository,
        IHashService hashService,
        IResultService resultService,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository
    )
    {
        _authRepository = authRepository;
        _hashService = hashService;
        _resultService = resultService;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<Result<long>> HandleAsync(AddUserRequest request)
    {
        if (await _userRepository.EmailExistsAsync(request.Email)) return _resultService.Error<long>("EmailExists");

        if (await _authRepository.LoginExistsAsync(request.Login)) return _resultService.Error<long>("LoginExists");

        var user = new User(request.Name, request.Email);

        var auth = new Auth(request.Login, request.Password, user);

        var password = _hashService.Create(auth.Password, auth.Salt.ToString());

        auth.UpdatePassword(password);

        await _userRepository.AddAsync(user);

        await _authRepository.AddAsync(auth);

        await _unitOfWork.SaveChangesAsync();

        return _resultService.Success(user.Id);
    }
}
