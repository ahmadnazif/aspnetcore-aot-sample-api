using ApiNativeAot.Models;
using ApiNativeAot.Services;
using Microsoft.AspNetCore.Mvc;
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

var sampleTodos = new Todo[] {
    new(1, "Walk the dog"),
    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
};

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

var smsEndpoint = app.MapGroup("/sms");
smsEndpoint.MapGet("/", () => new List<Sms>());

//smsEndpoint.MapGet("/get", async (InMemorySimpleDb db, [FromQuery] string id) =>
//{
//    return await db.GetSmsAsync(id);
//});

//smsEndpoint.MapGet("/list-all", async (InMemorySimpleDb db) =>
//{
//    return await db.ListAllSmsAsync();
//});

//smsEndpoint.MapPost("/add", async (InMemorySimpleDb db) =>
//{

//});

await app.RunAsync();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
//[JsonSerializable(typeof(Sms[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
