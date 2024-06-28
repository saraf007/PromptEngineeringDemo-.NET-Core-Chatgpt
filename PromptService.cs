using PromptEngineeringDemo.DTOs;
using System.Text;
using System.Text.Json;

namespace PromptEngineeringDemo
{
    public class PromptService : IPromptService
    {
        public readonly IConfiguration _configuration;

        public PromptService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> TriggerOpenAI(string prompt)
        {
            var apiKey = _configuration.GetValue<string>("OpenAISettings:APIKey");
            var baseUrl = _configuration.GetValue<string>("OpenAISettings:BaseUrl");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var request = new OpenAIRequestDto
            {
                Model = "gpt-3.5-turbo",
                Messages = new List<OpenAIMessageRequestDto>
                {
                    new OpenAIMessageRequestDto
                    {
                        Role = "user",
                        Content = prompt
                    }
                },
                MaxTokens = 100
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(baseUrl, content);
            var responseJson = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(responseJson);
                throw new System.Exception(errorResponse.Error.Message);
            }

            var data = JsonSerializer.Deserialize<OpenAIResponseDto>(responseJson);
            var responseText = data.choices[0].message.content;

            return responseText;
        }
    }

    public interface IPromptService
    {
        Task<string> TriggerOpenAI(string prompt);
    }
}
