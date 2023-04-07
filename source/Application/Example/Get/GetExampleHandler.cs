namespace Architecture.Application;

public sealed record GetExampleHandler : IHandler<GetExampleRequest, ExampleModel>
{
    private readonly IExampleRepository _exampleRepository;
    private readonly IResultService _resultService;

    public GetExampleHandler
    (
        IExampleRepository exampleRepository,
        IResultService resultService
    )
    {
        _exampleRepository = exampleRepository;
        _resultService = resultService;
    }

    public async Task<Result<ExampleModel>> HandleAsync(GetExampleRequest request)
    {
        var model = await _exampleRepository.GetModelAsync(request.Id);

        return _resultService.Success(model);
    }
}
