using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace api;

public class DownloadResume
{
    private static readonly HttpClient HttpClient = new();
    private const string ResumeUrl = "https://bryceresumestore.blob.core.windows.net/public/Bryce-Campain-Resume.pdf";

    [Function("DownloadResume")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "download-resume")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("DownloadResume");
        logger.LogInformation("Resume download requested.");

        using var upstreamResponse = await HttpClient.GetAsync(ResumeUrl);
        if (!upstreamResponse.IsSuccessStatusCode)
        {
            var error = req.CreateResponse(HttpStatusCode.InternalServerError);
            await error.WriteStringAsync("Unable to fetch resume at this time.");
            return error;
        }

        var pdfBytes = await upstreamResponse.Content.ReadAsByteArrayAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/pdf");
        response.Headers.Add("Content-Disposition", "attachment; filename=\"Bryce-Campain-Resume.pdf\"");
        await response.WriteBytesAsync(pdfBytes);

        return response;
    }
}
