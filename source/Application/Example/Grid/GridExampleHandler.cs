namespace Architecture.Application;

public sealed record GridExampleHandler : IHandler<GridExampleRequest, Grid<ExampleModel>>
{
    private readonly IExampleRepository _exampleRepository;
    private readonly IResultService _resultService;

    public GridExampleHandler
    (
        IExampleRepository exampleRepository,
        IResultService resultService
    )
    {
        _exampleRepository = exampleRepository;
        _resultService = resultService;
    }

    public async Task<Result<Grid<ExampleModel>>> HandleAsync(GridExampleRequest request)
    {
        var grid = await _exampleRepository.GridAsync(request);

        return _resultService.Success(grid);
    }
}
