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

var smsEndpoint = app.MapGroup("/sms");
smsEndpoint.MapGet("/", () =>
{
    return "Please use: /get, /list-all, /add, /edit, or /delete endpoint";
});

smsEndpoint.MapGet("/get", async (InMemorySimpleDb db, string id) =>
{
    return await db.GetSmsAsync(id);
});

smsEndpoint.MapGet("/list-all", async (InMemorySimpleDb db) =>
{
    return await db.ListAllSmsAsync();
});

smsEndpoint.MapPost("/add", async (InMemorySimpleDb db, SmsBase? sms) =>
{
    if (sms == null)
        return new PostResponse { IsSuccess = false, Message = "Please provide data" };

    var id = Guid.NewGuid().ToString();
    return await db.AddSmsAsync(id, sms.From, sms.To, sms.Text);
});

smsEndpoint.MapPut("/edit", async (InMemorySimpleDb db, SmsBase? sms) =>
{
    if (sms == null)
        return new PostResponse { IsSuccess = false, Message = "Please provide data" };

    if (string.IsNullOrWhiteSpace(sms.SmsId))
        return new PostResponse { IsSuccess = false, Message = "SmsId is required" };

    return await db.EditSmsAsync(sms.SmsId, sms.From, sms.To, sms.Text);
});

smsEndpoint.MapDelete("/delete", async (InMemorySimpleDb db, string id) =>
{
    return await db.DeleteSmsAsync(id);
});

await app.RunAsync();

[JsonSerializable(typeof(List<Sms>))]
[JsonSerializable(typeof(SmsBase))]
[JsonSerializable(typeof(Sms))]
[JsonSerializable(typeof(PostResponse))]
internal partial class AppJsonSerializerContext : JsonSerializerContext { }
