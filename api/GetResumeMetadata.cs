using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace api;

public class GetResumeMetadata
{
    private readonly IResumeStorageService _resumeStorageService;

    public GetResumeMetadata(IResumeStorageService resumeStorageService)
    {
        _resumeStorageService = resumeStorageService;
    }

    [Function("GetResumeMetadata")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "resume-metadata")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("GetResumeMetadata");
        logger.LogInformation("Resume metadata requested.");

        try
        {
            var revisionText = await _resumeStorageService.GetRevisionTextAsync();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { revisionText = revisionText ?? "Unknown" });
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to fetch resume metadata from blob storage.");
            var error = req.CreateResponse(HttpStatusCode.InternalServerError);
            await error.WriteStringAsync("Unable to fetch resume metadata at this time.");
            return error;
        }
    }
}
