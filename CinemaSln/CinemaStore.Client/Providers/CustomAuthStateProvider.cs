using CinemaStore.Client.Models.Dto;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace CinemaStore.Client.Providers
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js;
        private ClaimsPrincipal _currentUser;

        private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

        public CustomAuthStateProvider(IJSRuntime js)
        {
            _js = js;
            _currentUser = Anonymous;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Читаємо з localStorage один раз при старті
            var json = await _js.InvokeAsync<string>("localStorage.getItem", "authUser");
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var storedClaims = JsonSerializer.Deserialize<List<StoredClaim>>(json);
                    if (storedClaims != null)
                    {
                        var identity = new ClaimsIdentity(
                            storedClaims.Select(c => new Claim(c.Type, c.Value)),
                            "apiauth_type"
                        );
                        _currentUser = new ClaimsPrincipal(identity);
                    }
                }
                catch
                {
                    _currentUser = Anonymous;
                }
            }

            return new AuthenticationState(_currentUser);
        }

        public async Task MarkUserAsAuthenticated(string email, string? role)
        {
            // Створюємо claims
            var claims = new List<StoredClaim> { new StoredClaim { Type = ClaimTypes.Name, Value = email } };
            if (!string.IsNullOrEmpty(role))
                claims.Add(new StoredClaim { Type = ClaimTypes.Role, Value = role });

            // Зберігаємо у localStorage (для refresh)
            await _js.InvokeVoidAsync("localStorage.setItem", "authUser", JsonSerializer.Serialize(claims));

            // Створюємо identity для миттєвого оновлення
            var identity = new ClaimsIdentity(claims.Select(c => new Claim(c.Type, c.Value)), "apiauth_type");
            _currentUser = new ClaimsPrincipal(identity);

            // Повідомляємо Blazor про зміну стану
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }

        public async Task Logout()
        {
            // Видаляємо токен і claims з localStorage
            await _js.InvokeVoidAsync("localStorage.removeItem", "authUser");
            await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");

            // Ставимо користувача анонімним
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());

            // Оповіщаємо Blazor про зміну стану
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }
        public async Task MarkUserAsLoggedOut()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "authUser");
            _currentUser = Anonymous;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }
    }
}

