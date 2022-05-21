using CryptocurrencyAPI.Data.Interfaces;
using CryptocurrencyAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryptocurrencyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CryptocurrenciesController : ControllerBase
{
    private readonly IRestApi _restApi;

    private readonly ICryptocurrencyRepository _cryptocurrencyRepository;

    public CryptocurrenciesController(IRestApi restApi, ICryptocurrencyRepository cryptocurrencyRepository)
    {
        _restApi = restApi;
        _cryptocurrencyRepository = cryptocurrencyRepository;
    }

    [HttpGet]
    [Route("GetCryptocurrencies")]
    public async Task<IActionResult> GetCryptocurrencies()
    {
        try
        {
            var cryptocurrencies = await _cryptocurrencyRepository.GetAllAsync();

            return Ok(cryptocurrencies);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("GetCryptocurrenciesInformation")]
    public async Task<IActionResult> GetCryptocurrenciesInformation([FromBody] List<CryptocurrencyInfoRequest> cryptocurrencyInfoRequests)
    {
        try
        {
            var cryptocurrenciesInformation = await _restApi.GetPriceInformation(cryptocurrencyInfoRequests);

            return Ok(cryptocurrenciesInformation);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}