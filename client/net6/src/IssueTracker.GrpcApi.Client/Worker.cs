using Grpc.Core;
using Grpc.Net.Client;
using IssueTracker.GrpcApi.Grpc.Services;

namespace IssueTracker.GrpcApi.Client;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly string _serviceUrl;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _serviceUrl = configuration.GetValue<string>("ServiceUrl");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);

            using GrpcChannel channel = GrpcChannel.ForAddress(_serviceUrl);
            IssueTrackerService.IssueTrackerServiceClient client = new (channel);

            IssueSummariesMessage summaries = client
                .GetIssues(new PagedIssueRequestMessage {PageNumber = 1, PageSize = 10, SortBy = IssueSortBy.Title, Direction = SortOrder.Ascending }, cancellationToken: stoppingToken);
            if (summaries.Status == ResultCode.Success)
            {
                Console.WriteLine("Paged of issues:");
                foreach (IssueSummaryMessage summary in summaries.Summaries)
                {
                    _logger.LogInformation("{Id}: {Title}", summary.Id, summary.Title);
                }
            }

            AsyncServerStreamingCall<IssueSummaryMessage> allIssues = client
                .GetAllIssues(new SortedIssueRequestMessage { SortBy = IssueSortBy.Priority, Direction = SortOrder.Descending }, cancellationToken: stoppingToken);

            /*
             * TODO:
            foreach (IssueSummaryMessage summary in allIssues.ResponseStream)
            {
            }
            */

        }
    }
}
