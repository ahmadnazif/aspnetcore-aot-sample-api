using ApiNativeAot.Models;
using ApiNativeAot.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiNativeAot.Endpoints;

public static class SmsEndpoints
{
    public static void MapSmsEndpoints(this IEndpointRouteBuilder builder, InMemorySimpleDb db)
    {
        var endpoint = builder.MapGroup("/sms");

        endpoint.MapGet("/", () =>
        {
            return "Please use: /count-all, /get, /list-all, /add, /edit, or /delete endpoint";
        });

        endpoint.MapGet("/count-all", async () =>
        {
            var count = await db.CountAllAsync();
            return Results.Ok(count);
        });

        endpoint.MapGet("/get", async (string id) =>
        {
            var sms = await db.GetSmsAsync(id);
            return sms == null ? Results.NoContent() : Results.Ok(sms);
        });

        endpoint.MapGet("/list-all", async () =>
        {
            var list = await db.ListAllSmsAsync();
            return Results.Ok(list);
        });

        endpoint.MapPost("/add", async (SmsBase? sms) =>
        {
            try
            {
                if (sms == null)
                    return new PostResponse { IsSuccess = false, Message = "Please provide data" };

                var id = Guid.NewGuid().ToString();
                return await db.AddSmsAsync(id, sms.From, sms.To, sms.Text);
            }
            catch (Exception ex)
            {
                return new PostResponse { IsSuccess = false, Message = $"Exception: {ex.Message}" };
            }
        });

        endpoint.MapPut("/edit", async (SmsBase? sms) =>
        {
            try
            {
                if (sms == null)
                    return new PostResponse { IsSuccess = false, Message = "Please provide data" };

                if (string.IsNullOrWhiteSpace(sms.SmsId))
                    return new PostResponse { IsSuccess = false, Message = "SmsId is required" };

                return await db.EditSmsAsync(sms.SmsId, sms.From, sms.To, sms.Text);
            }
            catch (Exception ex)
            {
                return new PostResponse { IsSuccess = false, Message = $"Exception: {ex.Message}" };
            }
        });

        endpoint.MapDelete("/delete", async (string id) =>
        {
            return await db.DeleteSmsAsync(id);
        });
    }
}
