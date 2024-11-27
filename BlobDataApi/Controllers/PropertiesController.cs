using BlobDataApi.Models;
using BlobDataApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PropertiesController : ControllerBase
{
    private readonly BlobStorageService _blobStorageService;

    public PropertiesController(BlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProperties()
    {
        try
        {
            var data = await _blobStorageService.GetBlobDataAsync<List<Property>>();
            return Ok(data);
        }
        catch (FileNotFoundException)
        {
            return NotFound("The requested file was not found in the blob storage.");
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid or expired SAS token.");
        }
        catch (Exception ex)
        {
            // Log the exception for debugging
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred while processing the request.");
        }
    }

}
