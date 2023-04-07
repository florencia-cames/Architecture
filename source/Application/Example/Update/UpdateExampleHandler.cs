namespace Architecture.Application;

public sealed record UpdateExampleHandler : IHandler<UpdateExampleRequest>
{
    private readonly IExampleRepository _exampleRepository;
    private readonly IResultService _resultService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateExampleHandler
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

    public async Task<Result> HandleAsync(UpdateExampleRequest request)
    {
        var entity = new Example(request.Id, request.Name);

        await _exampleRepository.UpdateAsync(entity);

        await _unitOfWork.SaveChangesAsync();

        return _resultService.Success();
    }
}
