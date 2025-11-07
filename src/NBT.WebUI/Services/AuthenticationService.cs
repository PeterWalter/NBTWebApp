using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;

namespace NBT.WebUI.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private UserInfo? _currentUser;
    private string? _token;

    public AuthenticationService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password, bool rememberMe)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new
            {
                email,
                password,
                rememberMe
            });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
                
                if (result?.Success == true && result.Token != null)
                {
                    _token = result.Token;
                    _currentUser = result.User;
                    
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", result.Token);
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", result.RefreshToken);
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userInfo", JsonSerializer.Serialize(result.User));
                    
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);
                }
                
                return result ?? new AuthenticationResult { Success = false, Message = "Invalid response from server" };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new AuthenticationResult 
                { 
                    Success = false, 
                    Message = $"Login failed: {response.StatusCode}" 
                };
            }
        }
        catch (Exception ex)
        {
            return new AuthenticationResult 
            { 
                Success = false, 
                Message = $"Login error: {ex.Message}" 
            };
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            if (_token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                await _httpClient.PostAsync("api/auth/logout", null);
            }
        }
        catch
        {
            // Ignore errors during logout
        }
        finally
        {
            _token = null;
            _currentUser = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
            
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userInfo");
        }
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        if (_token != null)
            return true;

        try
        {
            _token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "authToken");
            
            if (!string.IsNullOrEmpty(_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                
                var userJson = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "userInfo");
                if (!string.IsNullOrEmpty(userJson))
                {
                    _currentUser = JsonSerializer.Deserialize<UserInfo>(userJson);
                }
                
                return true;
            }
        }
        catch
        {
            // Token retrieval failed
        }

        return false;
    }

    public async Task<UserInfo?> GetCurrentUserAsync()
    {
        if (_currentUser != null)
            return _currentUser;

        try
        {
            var userJson = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "userInfo");
            if (!string.IsNullOrEmpty(userJson))
            {
                _currentUser = JsonSerializer.Deserialize<UserInfo>(userJson);
            }
        }
        catch
        {
            // User retrieval failed
        }

        return _currentUser;
    }

    public async Task<string?> GetTokenAsync()
    {
        if (_token != null)
            return _token;

        try
        {
            _token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "authToken");
        }
        catch
        {
            // Token retrieval failed
        }

        return _token;
    }
}
