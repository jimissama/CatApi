using System.Resources;
using System.Text;
using CatApi.Models.Response;
using CatApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatApi.Controllers;

/// <summary>
/// CatApi Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CatsController : ControllerBase
{
    private readonly ICatService _catService;
    private readonly ICatControllerValidationService _catControllerValidation;
    private readonly ILogger<CatsController> _logger;
    private readonly ResourceManager _resourceManager;

    /// <summary>
    /// Controller Constructor
    /// </summary>
    /// <param name="serviceProvider">Service Provider</param>
    public CatsController(IServiceProvider serviceProvider, ILogger<CatsController> logger, ResourceManager resourceManager)
    {
        _catService = serviceProvider.GetRequiredService<ICatService>();
        _catControllerValidation = serviceProvider.GetRequiredService<ICatControllerValidationService>();
        _logger = logger;
        _resourceManager = resourceManager;
    }

    /// <summary>
    /// Fetch 25 cat images and store only the new ones.
    /// </summary>
    /// <returns>Info message about the count of the new stored images</returns>
    /// <exception cref="HttpRequestException">Internal Server Error with message of the cause.</exception>
    /// <example>Men's basketball shoes</example>

    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("fetch")]
    public async Task<IActionResult> Fetch()
    {
        try
        {
            var newCatsCount = await _catService.FetchCats();

            StringBuilder strBuilder = new StringBuilder();

            strBuilder.AppendFormat(_resourceManager.GetString("StoredCatsCount") ?? string.Empty, newCatsCount);

            _logger.LogInformation(strBuilder.ToString());

            return Ok(strBuilder.ToString());

        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, e.Message);

            return StatusCode((int)e.StatusCode, e.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }



    /// <summary>
    /// Retrieves the cat image data with the specified Id.
    /// </summary>
    /// <param name="id">Cat Image record Id</param>
    /// <returns>Cat image record</returns>
    /// <exception cref="HttpRequestException"></exception>

    [ProducesResponseType(typeof(CatResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            _catControllerValidation.GetCatByIdInvalidId(id);

            var result = await _catService.GetCatById(id);

            _catControllerValidation.GetCatByIdNull(result);

            return Ok(result);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode((int)e.StatusCode, e.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }





    }

    /// <summary>
    /// Retrieves the cat image records with pagination.
    /// </summary>
    /// <param name="tag">Cat temperament</param>
    /// <param name="page">The number of the page</param>
    /// <param name="pageSize">The count of the cat image records a page hosts</param>
    /// <returns>Cat image record</returns>
    /// <exception cref="HttpRequestException"></exception>

    [ProducesResponseType(typeof(IEnumerable<CatResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<IActionResult> Get([FromQuery] string? tag, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            _catControllerValidation.GetCatsInvalidPageAndPageSize(page, pageSize);

            return Ok(await _catService.GetCats(tag, page, pageSize));
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode((int)e.StatusCode, e.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }
}
