var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.HarmonyOfEmotions_ApiService>("harmony-of-emotions-apiservice");

builder.AddProject<Projects.HarmonyOfEmotions_Client>("harmony-of-emotions-client")
.WithExternalHttpEndpoints()
    .WithReference(apiService);

//var apiService = builder.AddProject<Projects.HarmonyOfEmotions_ApiService>("harmony-of-emotions-apiservice");

builder.Build().Run();
