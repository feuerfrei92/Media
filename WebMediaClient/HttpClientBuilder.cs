using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebMediaClient.Models;

namespace WebMediaClient
{
	public class HttpClientBuilder<T>
	{
		public async static Task<T> GetAsync(string url, string token)
		{
			using (var client = new HttpClient())
			{
				InitializeClient(client, url, token);

				HttpResponseMessage response = await client.GetAsync(url);
				if (response.IsSuccessStatusCode)
				{
					T item = await response.Content.ReadAsAsync<T>();
					return item;
				}
				else throw new Exception(string.Concat("Request failed with status code: ", response.StatusCode.ToString()));
			}
		}

		public async static Task<List<T>> GetListAsync(string url, string token)
		{
			using (var client = new HttpClient())
			{
				InitializeClient(client, url, token);

				HttpResponseMessage response = await client.GetAsync(url);
				if (response.IsSuccessStatusCode)
				{
					List<T> items = await response.Content.ReadAsAsync<List<T>>();
					return items;
				}
				else throw new Exception(string.Concat("Request failed with status code: ", response.StatusCode.ToString()));
			}
		}

        public async static Task<List<U>> GetListAsync<U>(T item, string url, string token)
        {
            using (var client = new HttpClient())
            {
                InitializeClient(client, url, token);

                HttpResponseMessage response = await client.PostAsJsonAsync<T>(url, item);
                if (response.IsSuccessStatusCode)
                {
                    List<U> items = await response.Content.ReadAsAsync<List<U>>();
                    return items;
                }
                else throw new Exception(string.Concat("Request failed with status code: ", response.StatusCode.ToString()));
            }
        }

		public static T Get(string url, string token)
		{
			using (var client = new HttpClient())
			{
				InitializeClient(client, url, token);

				HttpResponseMessage response = client.GetAsync(url).Result;
				if (response.IsSuccessStatusCode)
				{
					T item = response.Content.ReadAsAsync<T>().Result;
					return item;
				}
				else throw new Exception(string.Concat("Request failed with status code: ", response.StatusCode.ToString()));
			}
		}

		public async static Task<HttpResponseMessage> PostEmptyAsync(T item, string url, string token)
		{
			using (var client = new HttpClient())
			{
				InitializeClient(client, url, token);

				HttpResponseMessage response = await client.PostAsJsonAsync<T>(url, item);
				if (response.IsSuccessStatusCode)
				{
					return response;
				}
				else throw new Exception(string.Concat("Request failed with status code: ", response.StatusCode.ToString()));
			}
		}

		public async static Task<T> PostAsync(T item, string url, string token)
		{
			using (var client = new HttpClient())
			{
				InitializeClient(client, url, token);

				HttpResponseMessage response = await client.PostAsJsonAsync<T>(url, item);
				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadAsStringAsync().Result;
					T returnItem = JsonConvert.DeserializeObject<T>(result);
					return returnItem;
				}
				else throw new Exception(string.Concat("Request failed with status code: ", response.StatusCode.ToString()));
			}
		}

		public async static Task<U> PostAsync<U>(T item, string url, string token)
		{
			using (var client = new HttpClient())
			{
				InitializeClient(client, url, token);

				HttpResponseMessage response = await client.PostAsJsonAsync<T>(url, item);
				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadAsStringAsync().Result;
					U returnItem = JsonConvert.DeserializeObject<U>(result);
					return returnItem;
				}
				else throw new Exception(string.Concat("Request failed with status code: ", response.StatusCode.ToString()));
			}
		}

		public async static Task<T> PutAsync(T item, string url, string token)
		{
			using (var client = new HttpClient())
			{
				InitializeClient(client, url, token);

				HttpResponseMessage response = await client.PutAsJsonAsync<T>(url, item);
				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadAsStringAsync().Result;
					T returnItem = JsonConvert.DeserializeObject<T>(result);
					return returnItem;
				}
				else throw new Exception(string.Concat("Request failed with status code: ", response.StatusCode.ToString()));
			}
		}

		public async static Task<U> PutAsync<U>(T item, string url, string token)
		{
			using (var client = new HttpClient())
			{
				InitializeClient(client, url, token);

				HttpResponseMessage response = await client.PutAsJsonAsync<T>(url, item);
				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadAsStringAsync().Result;
					U returnItem = JsonConvert.DeserializeObject<U>(result);
					return returnItem;
				}
				else throw new Exception(string.Concat("Request failed with status code: ", response.StatusCode.ToString()));
			}
		}

		public async static void DeleteAsync(string url, string token)
		{
			using (var client = new HttpClient())
			{
				InitializeClient(client, url, token);

				HttpResponseMessage response = await client.DeleteAsync(url);
				if (!response.IsSuccessStatusCode)
					throw new Exception(string.Concat("Request failed with status code: ", response.StatusCode.ToString()));
			}
		}

		public async static Task<U> LoginAsync<U>(T item, string url)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

				if(!(item is LoginViewModel))
				{
					throw new Exception("Invalid type of variable");
				}

				var workingItem = item as LoginViewModel;
				string content = string.Format("grant_type=password&username={0}&password={1}", workingItem.Email, workingItem.Password);
				var stringContent = new StringContent(content);

				HttpResponseMessage response = await client.PostAsync(url, stringContent);
				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadAsStringAsync().Result;
					U token = JsonConvert.DeserializeObject<U>(result);
					return token;
				}
				else throw new Exception(string.Concat("Request failed with status code: ", response.StatusCode.ToString()));
			}
		}

		private static void InitializeClient(HttpClient client, string url, string token)
		{
			client.BaseAddress = new Uri(url);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			if (!string.IsNullOrEmpty(token))
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}
	}
}

