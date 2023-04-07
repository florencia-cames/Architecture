namespace Architecture.Application;

public sealed record AddExampleHandler : IHandler<AddExampleRequest, long>
{
    private readonly IExampleRepository _exampleRepository;
    private readonly IResultService _resultService;
    private readonly IUnitOfWork _unitOfWork;

    public AddExampleHandler
    (
        IExampleRepository exampleRepository,
        IResultService resultService,
        IUnitOfWork unitOfWork
    )
    {
        _exampleRepository = exampleRepository;
        _resultService = resultService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<long>> HandleAsync(AddExampleRequest request)
    {
        var entity = new Example(request.Name);

        await _exampleRepository.AddAsync(entity);

        await _unitOfWork.SaveChangesAsync();

        return _resultService.Success(entity.Id);
    }
}
