using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Web;
using System.Text.RegularExpressions;

namespace REST_Web_service.Controllers
{
    [ApiController]
    public class testController : ControllerBase
    {
        //локации и площадки хранятся в словаре, где ключ - локация, а значение - список площадок
        public static Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();


        [HttpPost, Route("/adplatforms/refresh")]
        public async Task<IActionResult> adPlatformsRefresh([FromForm] IFormFile file = null)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Вы не загрузили файл");
            }

            dict.Clear(); 

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var fileContent = await reader.ReadToEndAsync();
                foreach (var line in fileContent.Split("\r\n"))
                {

                    string[] splitArr = line.Split(":", StringSplitOptions.RemoveEmptyEntries); //разделение строки файла на 2 части - имя площадки и имя рабочих локаций.

                    if (splitArr.Length <= 1)
                    {
                        return BadRequest("Некорректный формат строки: отсутствует название либо локация рекламной площадки");
                    }

                    if (string.IsNullOrEmpty(splitArr[0].Trim()))
                    {
                        return BadRequest("Некорректное название рекламной площадки");
                    }

                    var pattern = @"^(/[a-z]{1,},{0,1}){1,}$"; //паттерн допускает только слэши, строчные английские буквы и запятые
                    if (!Regex.IsMatch(splitArr[1].Trim(), pattern))
                    {
                        return BadRequest("Некорректное название локации");
                    }

                    string adPlatformName = splitArr[0];
                    string[] keys = splitArr[1].Split(',');
                    //наполнение словаря
                    foreach (var fkey in keys)
                    {
                        string key = fkey;
                        while (key.LastIndexOf('/') != -1)
                        {
                            if (dict.ContainsKey(key))
                            {
                                if (!dict[key].Contains(adPlatformName))
                                {
                                    dict[key].Add(adPlatformName);
                                }
                            }
                            else
                            {
                                dict[key] = new List<string> { adPlatformName };
                            }
                            key = key.Substring(0, key.LastIndexOf('/'));
                        }
                    }
                }
            }
            return Ok();
        }


        [HttpGet, Route("/adplatforms/locations")]
        public IActionResult adPlatformsInLocation([FromQuery] string loc)
        {
            try
            {
                var some = new { adPlatforms = dict[loc] };
                return Ok(some);
            }
            catch
            {
                var some = new { adPlatforms = new List<string> { } };
                return Ok(some);
            }
        }
    }
}
