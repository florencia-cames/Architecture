namespace Architecture.Application;

public sealed record ListExampleHandler : IHandler<ListExampleRequest, IEnumerable<ExampleModel>>
{
    private readonly IExampleRepository _exampleRepository;
    private readonly IResultService _resultService;

    public ListExampleHandler
    (
        IExampleRepository exampleRepository,
        IResultService resultService
    )
    {
        _exampleRepository = exampleRepository;
        _resultService = resultService;
    }

    public async Task<Result<IEnumerable<ExampleModel>>> HandleAsync(ListExampleRequest request)
    {
        var list = await _exampleRepository.ListModelAsync();

        return _resultService.Success(list);
    }
}
