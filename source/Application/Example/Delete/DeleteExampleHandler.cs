namespace Architecture.Application;

public sealed record DeleteExampleHandler : IHandler<DeleteExampleRequest>
{
    private readonly IExampleRepository _exampleRepository;
    private readonly IResultService _resultService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExampleHandler
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

    public async Task<Result> HandleAsync(DeleteExampleRequest request)
    {
        await _exampleRepository.DeleteAsync(request.Id);

        await _unitOfWork.SaveChangesAsync();

        return _resultService.Success();
    }
}
