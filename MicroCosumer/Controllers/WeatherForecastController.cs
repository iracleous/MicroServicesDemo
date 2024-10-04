using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
 

namespace MicroCosumer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
 
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet ]
        public async Task<IEnumerable<WeatherForecast>?> Get()
        {
            HttpClient _httpClient = new HttpClient();
            var urlProducer = "https://localhost:7249/weatherforecast";

            IEnumerable<WeatherForecast>? returnValue = [];

            try
            {
                // Setting request headers (e.g., user agent) to avoid some API restrictions
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");

                // Send GET request
                HttpResponseMessage response = await _httpClient.GetAsync(urlProducer);

                // Ensure the request was successful, otherwise throws an exception
                response.EnsureSuccessStatusCode();

                // Read response content as string
                string responseData = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Response Data:");
                Console.WriteLine(responseData);
                returnValue = JsonConvert.DeserializeObject< IEnumerable<WeatherForecast>>(responseData);
        
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request exceptions (network errors, invalid responses, etc.)
                Console.WriteLine($"Request error: {ex.Message}");
            }
            finally
            {
                // Optionally dispose of HttpClient when you're done (if not reusing it)
                _httpClient.Dispose();
            }

            return returnValue;
        }
    }
}
