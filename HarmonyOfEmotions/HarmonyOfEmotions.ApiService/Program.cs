using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using HarmonyOfEmotions.ApiService.Authentication;
using HarmonyOfEmotions.ApiService.Data;
using HarmonyOfEmotions.ApiService.Implementations;
using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain;
using HarmonyOfEmotions.Domain.Exceptions;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SpotifyAPI.Web;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
	var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	//  -> it gets the name of the assembly for the project
	var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
	setupAction.IncludeXmlComments(xmlCommentsFullPath);

	// include the xml comments file for the Domain project
	var xmlCommentsFileShared = $"{Assembly.GetAssembly(typeof(Country))!.GetName().Name}.xml";
	var xmlCommentsFullPathShared = Path.Combine(AppContext.BaseDirectory, xmlCommentsFileShared);
	setupAction.IncludeXmlComments(xmlCommentsFullPathShared);
	setupAction.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey
	});

	setupAction.OperationFilter<SecurityRequirementsOperationFilter>();
	
});
builder.Services.ConfigureSwaggerGen(setup =>
{
	setup.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Harmony of Emotions API",
		Version = "v1"
	});
});

// Add Spotify client
builder.Services.AddSingleton(SpotifyClientConfig.CreateDefault());
builder.Services.AddScoped<SpotifyClientBuilder>();

// Add Firebase client
builder.Services.AddSingleton<IFirebaseClient>(
	new FirebaseClient(new FirebaseConfig
	{
		AuthSecret = builder.Configuration["Firebase:AuthSecret"],
		BasePath = builder.Configuration["Firebase:BasePath"]
	}));
builder.Services.AddScoped<IRepository, FirebaseRepository>();

// Add services
builder.Services.AddScoped<IRecommenderService, RecommenderService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<ISpotifyService, SpotifyService>();
builder.Services.AddScoped<ILastFmService, LastFmService>();
builder.Services.AddScoped<IMusicBrainzService, MusicBrainzService>();
builder.Services.AddScoped<IUserTrackService, UserTrackService>();

// Add http client
builder.Services.AddHttpClient<ILastFmService, LastFmService>(client =>
{
	client.BaseAddress = new Uri("https://ws.audioscrobbler.com/2.0/");
	client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient<IMusicBrainzService, MusicBrainzService>(client =>
{
	client.BaseAddress = new Uri("https://musicbrainz.org/ws/2/");
	client.DefaultRequestHeaders.Add("Accept", "application/json");
	client.DefaultRequestHeaders.Add("User-Agent", "HarmonyOfEmotions");
});

builder.Services.AddHttpClient<IRecommenderService, RecommenderService>(client =>
{
	client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Add database context
builder.Services.AddDbContext<HarmonyOfEmotionsDbContext>(options =>
	options.UseSqlServer(builder.Configuration["HarmonyOfEmotionsConnectionString"]));
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
	.AddEntityFrameworkStores<HarmonyOfEmotionsDbContext>();

builder.Services.AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme).Configure(options => {
	options.BearerTokenExpiration = TimeSpan.FromSeconds(60*30);
});

// add exception handling
builder.Services.AddControllers(options =>
{
	options.Filters.Add<GlobalExceptionFilter>();
});

var app = builder.Build();

app.MapDefaultEndpoints();

string tempFolder = "../tmp";
if (!Directory.Exists(tempFolder))
{
	Directory.CreateDirectory(tempFolder);
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();