HttpClient httpClient = new();
var response = await httpClient.GetAsync("https://localhost:7177/api/values");

if (response.IsSuccessStatusCode)
{
	Console.WriteLine(await response.Content.ReadAsStringAsync());
}