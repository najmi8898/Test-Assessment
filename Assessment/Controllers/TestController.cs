using Assessment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nancy.Json;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestDbContext _dbContext;

        public TestController(TestDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Login API
        [HttpPost("Login")]
        public async Task<IActionResult> PostLogin(LoginRequest login)
        {
            // serialize to JSON
            string jsonData = JsonConvert.SerializeObject(login);

            var result = "";

            using (var client = new HttpClient())
            {
                var url = "http://test-demo.aemenersol.com/api/Account/Login";

                // wrap into Response body content
                var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, contentData);

                if (response.IsSuccessStatusCode)
                {
                    var stringData = await response.Content.ReadAsStringAsync();
                    result = stringData;
                }
            }
            
            Console.Write(jsonData);

            return Ok(result);
        }

        // 
        [HttpGet("GetPlatformWellActual/{token}")]
        public async Task<IActionResult> GetPlatformWellActual(string token)
        {
            var client = new HttpClient();
            var url = "http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual";
            //client.DefaultRequestHeaders.Authorization
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(url);

            var result = "";
            var stringData = "";

            if (response.IsSuccessStatusCode)
            {
                stringData = await response.Content.ReadAsStringAsync();
                //result = stringData;  
            }

            // deserialize -> JsonConvert
            var platformDto = JsonConvert.DeserializeObject<List<PlatformDto>>(stringData);
            //var wellDto = JsonConvert.DeserializeObject<List<WellDto>>(result);

            foreach (var data in platformDto)
            {
                var existingPlatform = await _dbContext.Platform.Where(p => p.Id == data.Id).FirstOrDefaultAsync();

                if (existingPlatform != null)
                {
                    //var newPlatform = new PlatformDto();
                    //existingPlatform.Id = data.Id;
                    existingPlatform.UniqueName = data.UniqueName;
                    existingPlatform.Latitude = data.Latitude;
                    existingPlatform.Longitude = data.Longitude;
                    existingPlatform.CreatedAt = data.CreatedAt;
                    existingPlatform.UpdatedAt = data.UpdatedAt;

                    _dbContext.Platform.Update(existingPlatform);
                    result = "Platform Updated Successfully";

                    foreach (var data2 in data.Well)
                    {
                        var existingWell = await _dbContext.Well.Where(p => p.Id == data2.Id).FirstOrDefaultAsync();
                        if (existingWell != null)
                        {
                            existingWell.Platform = existingPlatform;
                            existingWell.PlatformId = data2.PlatformId;
                            existingWell.UniqueName = data2.UniqueName;
                            existingWell.Latitude = data2.Latitude;
                            existingWell.Longitude = data2.Longitude;
                            existingWell.CreatedAt = data2.CreatedAt;
                            existingWell.UpdatedAt = data2.UpdatedAt;

                            _dbContext.Well.Update(existingWell);
                            result = "Well Updated Successfully";
                        }
                        else
                        {
                            var newWell = new WellDto();
                            newWell.Id = data2.Id;
                            newWell.Platform = existingPlatform;
                            newWell.PlatformId = data2.PlatformId;
                            newWell.UniqueName = data2.UniqueName;
                            newWell.Latitude = data2.Latitude;
                            newWell.Longitude = data2.Longitude;
                            newWell.CreatedAt = data2.CreatedAt;
                            newWell.UpdatedAt = data2.UpdatedAt;

                            existingPlatform.Well.Add(newWell);
                            result = "Well Created Successfully";
                        }
                    }
                }
                else
                {
                    var newPlatform = new PlatformDto();
                    newPlatform.Id = data.Id;
                    newPlatform.UniqueName = data.UniqueName;
                    newPlatform.Latitude = data.Latitude;
                    newPlatform.Longitude = data.Longitude;
                    newPlatform.CreatedAt = data.CreatedAt;
                    newPlatform.UpdatedAt = data.UpdatedAt;
                    //newPlatform.Wells = data.Wells;

                    // well loop
                    foreach (var data2 in data.Well)
                    {
                        var existingWell = await _dbContext.Well.Where(p => p.Id == data2.Id).FirstOrDefaultAsync();
                        if (existingWell != null)
                        {
                            existingWell.Platform = newPlatform;
                            existingWell.PlatformId = data2.PlatformId;
                            existingWell.UniqueName = data2.UniqueName;
                            existingWell.Latitude = data2.Latitude;
                            existingWell.Longitude = data2.Longitude;
                            existingWell.CreatedAt = data2.CreatedAt;
                            existingWell.UpdatedAt = data2.UpdatedAt;

                            _dbContext.Well.Update(existingWell);
                            result = "Well Updated Successfully";
                        }
                        else
                        {
                            var newWell = new WellDto();
                            newWell.Id = data2.Id;
                            newWell.Platform = newPlatform;
                            newWell.PlatformId = data2.PlatformId;
                            newWell.UniqueName = data2.UniqueName;
                            newWell.Latitude = data2.Latitude;
                            newWell.Longitude = data2.Longitude;
                            newWell.CreatedAt = data2.CreatedAt;
                            newWell.UpdatedAt = data2.UpdatedAt;
                            
                            newPlatform.Well.Add(newWell);
                            result = "Well Created Successfully";
                        } 
                    }
                    // add
                    _dbContext.Platform.Add(newPlatform);
                    result = "Platform Created Successfully";
                }
                _dbContext.SaveChanges();
            }
            return Ok(result);
        }
    }
}
