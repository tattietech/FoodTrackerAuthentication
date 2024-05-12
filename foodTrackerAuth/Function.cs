using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using foodTrackerAuth.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace foodTrackerAuth;

public class Function
{

    private string authKey = Environment.GetEnvironmentVariable("FOOD_TRACKER_AUTH_KEY");
    private string clientId = Environment.GetEnvironmentVariable("FOOD_TRACKER_CLIENT_ID");

    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request)
    {
        var path = request.RequestContext?.Http?.Path?.Split("/")?.LastOrDefault(string.Empty);
        HttpClient client = new();

        if (path == "auth")
        {
            bool gotCode = request.QueryStringParameters.TryGetValue("code", out var code);

            if (!gotCode && !string.IsNullOrEmpty(code))
            {
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Body = "Incorrect Code Provided"
                };
            }

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://foodtracker.auth.eu-west-2.amazoncognito.com/oauth2/token");
            tokenRequest.Headers.Add("Authorization", authKey);

            tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string> {
            { "client_id", clientId },
            { "grant_type", "authorization_code" },
            { "code", code },
            { "redirect_uri", "https://localhost:7048" }});

            var response = await client.SendAsync(tokenRequest);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            var token = payload.Value<string>("access_token");
            var refreshToken = payload.Value<string>("refresh_token");

            var authResponse = new AuthResponse() { AccessToken = token, RefreshToken = refreshToken };
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(authResponse)
            };
        }
        else if(path == "userInfo")
        {
            var gotKey = request.Headers.TryGetValue("authorization", out var key);

            if (!gotKey)
            {
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Body = "Incorrect Key Provided"
                };
            }

            var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, "https://foodtracker.auth.eu-west-2.amazoncognito.com/oauth2/userInfo");
            userInfoRequest.Headers.Add("Authorization", key);

            var response = await client.SendAsync(userInfoRequest);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = payload.ToString()
            };
        }
        else if (path == "refresh")
        {
            if (string.IsNullOrEmpty(request.Body))
            {
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Body = "Incorrect Token Provided"
                };
            }

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://foodtracker.auth.eu-west-2.amazoncognito.com/oauth2/token");
            tokenRequest.Headers.Add("Authorization", authKey);
            tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string> {
            { "client_id", clientId },
            { "grant_type", "refresh_token" },
            { "refresh_token", request.Body }});

            var response = await client.SendAsync(tokenRequest);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            var accessToken = payload.Value<string>("access_token");

            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = accessToken
            };
        }
        else if(path == "logout")
        {
            if (string.IsNullOrEmpty(request.Body))
            {
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Body = "Incorrect Token Provided"
                };
            }

            var revokeRequest = new HttpRequestMessage(HttpMethod.Post, "https://foodtracker.auth.eu-west-2.amazoncognito.com/oauth2/revoke");
            revokeRequest.Headers.Add("Authorization", authKey);
            revokeRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string> {
            { "client_id", clientId },
            { "token", request.Body }});

            var response = await client.SendAsync(revokeRequest);
            response.EnsureSuccessStatusCode();

            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = (int)HttpStatusCode.NotFound,
            Body = "Made to lambda but not found"
        };
    }
}
