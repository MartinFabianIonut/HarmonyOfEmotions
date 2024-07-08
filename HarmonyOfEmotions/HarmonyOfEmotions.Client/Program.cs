using HarmonyOfEmotions.Client.Components;
using HarmonyOfEmotions.Client.Services.ApiServices;
using HarmonyOfEmotions.Client.Services.Authentication;
using HarmonyOfEmotions.Client.Services.ErrorHandling;
using HarmonyOfEmotions.Domain.Authentication;
using HarmonyOfEmotions.ServiceDefaults.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;


var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddOutputCache();
builder.Services.AddAuthorizationCore();

// add services for session management
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// add services for authentication
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddTransient<BearerTokenHandler>();

builder.Services.AddCascadingAuthenticationState();

var harmonyAuthentication = AuthState.HarmonyAuthentication.ToString();
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = harmonyAuthentication;
	options.DefaultChallengeScheme = harmonyAuthentication;
})
	.AddCookie(harmonyAuthentication, options =>
	{
		options.Cookie.Name = "HarmonyOfEmotions.Authentication";
		options.Cookie.HttpOnly = true;  // Cookie HTTP-only
		options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Secure cookie
		options.Cookie.SameSite = SameSiteMode.Strict;  // SameSite policy
		options.LoginPath = "/login";
		options.LogoutPath = "/logout";
		options.AccessDeniedPath = "/access-denied";
	});

builder.Services.AddAuthorization();

// add services for API client
builder.Services.AddHttpClient<ApiClient>(client =>
{
	// This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
	// Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
	client.BaseAddress = new("https+http://harmony-of-emotions-apiservice");
	// content type is json
	client.DefaultRequestHeaders.Accept.Add(new("application/json"));
}).AddHttpMessageHandler<BearerTokenHandler>();

// add services for error handling
builder.Services.AddSingleton<IErrorHandlingService, ErrorHandlingService>();

// add services for API calls
builder.Services.AddScoped<UserTrackPreferencesService>();
builder.Services.AddScoped<SpotifyTrackService>();
builder.Services.AddScoped<MusicRecommenderSystemService>();
builder.Services.AddScoped<ArtistInfoService>();
builder.Services.AddScoped<ZenQuotesService>();
builder.Services.AddScoped<GoogleSearchAutocompleteService>();

builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
