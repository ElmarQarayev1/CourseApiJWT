using System;
using Course.Ui.Resources;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Course.Ui.Controllers
{
  
	public class GroupController:Controller
	{
        public async Task<IActionResult> Index(int page = 1)
        {
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:7064/api/Groups?page=" + page + "&size=2"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var bodyStr = await response.Content.ReadAsStringAsync();

                        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                        PaginatedResponseResource<GroupListItemGetResource> data = JsonSerializer.Deserialize<PaginatedResponseResource<GroupListItemGetResource>>(bodyStr, options);
                        return View(data);
                    }
                    else
                    {
                        return RedirectToAction("error", "home");
                    }
                }
            }
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GroupCreateResource model)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var jsonContent = JsonSerializer.Serialize(model);
                    var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync("https://localhost:7064/api/Groups", contentString))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "error occured");
                        }
                    }
                }
            }
            return View(model);
        }
    }
  
}

