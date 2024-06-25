using HarmonyOfEmotions.Client.Components;
using HarmonyOfEmotions.Client.Services.ApiServices;
using HarmonyOfEmotions.Client.Services.Auth;
using HarmonyOfEmotions.Client.Services.ErrorHandling;
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
// add iauthservice
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddHttpContextAccessor();
//builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<BearerTokenHandler>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.Cookie.Name = "HarmonyOfEmotions.Client";
		options.LoginPath = "/login";
		options.LogoutPath = "/logout";
		options.AccessDeniedPath = "/access-denied";
	});

builder.Services.AddAuthorization();

builder.Services.AddHttpClient<ApiClient>(client =>
{
	// This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
	// Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
	client.BaseAddress = new("https+http://harmony-of-emotions-apiservice");
	// content type is json
	client.DefaultRequestHeaders.Accept.Add(new("application/json"));
}).AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddSingleton<IErrorHandlingService, ErrorHandlingService>();
builder.Services.AddScoped<UserTrackPreferencesService>();
builder.Services.AddScoped<SpotifyTrackService>();
builder.Services.AddScoped<MusicRecommenderSystemService>();
builder.Services.AddScoped<ArtistInfoService>();

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
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.UseOutputCache();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
