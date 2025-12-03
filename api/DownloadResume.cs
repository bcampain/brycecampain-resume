using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace api;

public class DownloadResume
{
    private readonly IResumeStorageService _resumeStorageService;

    public DownloadResume(IResumeStorageService resumeStorageService)
    {
        _resumeStorageService = resumeStorageService;
    }

    [Function("DownloadResume")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "download-resume")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("DownloadResume");
        logger.LogInformation("Resume download requested.");

        byte[] pdfBytes;
        try
        {
            pdfBytes = await _resumeStorageService.DownloadAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to download resume from blob storage.");
            var error = req.CreateResponse(HttpStatusCode.InternalServerError);
            await error.WriteStringAsync("Unable to fetch resume at this time.");
            return error;
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/pdf");
        response.Headers.Add("Content-Disposition", "attachment; filename=\"Bryce-Campain-Resume.pdf\"");
        await response.WriteBytesAsync(pdfBytes);

        return response;
    }
}
