using ApiNativeAot.Endpoints;
using ApiNativeAot.Models;
using ApiNativeAot.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
var config = builder.Configuration;

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddSingleton<InMemorySimpleDb>();

builder.WebHost.ConfigureKestrel(x =>
{
    var httpPort = int.Parse(config["Port"]);
    x.ListenAnyIP(httpPort);
});

var app = builder.Build();

app.MapGet("/", () => "Please use /sms endpoints");

var db = app.Services.GetRequiredService<InMemorySimpleDb>();
app.MapSmsEndpoints(db);

await app.RunAsync();

[JsonSerializable(typeof(List<Sms>))]
[JsonSerializable(typeof(SmsBase))]
[JsonSerializable(typeof(Sms))]
[JsonSerializable(typeof(PostResponse))]
internal partial class AppJsonSerializerContext : JsonSerializerContext { }
