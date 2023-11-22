using ApiNativeAot.Models;
using ApiNativeAot.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiNativeAot.Endpoints;

public static class SmsEndpoints
{
    public static void MapSmsEndpoints(this IEndpointRouteBuilder builder, InMemorySimpleDb db)
    {
        var smsEndpoint = builder.MapGroup("/sms");

        smsEndpoint.MapGet("/", () =>
        {
            return "Please use: /get, /list-all, /add, /edit, or /delete endpoint";
        });

        smsEndpoint.MapGet("/get", async (string id) =>
        {
            var sms = await db.GetSmsAsync(id);
            return sms == null ? Results.NoContent() : Results.Ok(sms);
        });

        smsEndpoint.MapGet("/list-all", async () =>
        {
            var list = await db.ListAllSmsAsync();
            return Results.Ok(list);
        });

        smsEndpoint.MapPost("/add", async (SmsBase? sms) =>
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

        smsEndpoint.MapPut("/edit", async (SmsBase? sms) =>
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

        smsEndpoint.MapDelete("/delete", async (string id) =>
        {
            return await db.DeleteSmsAsync(id);
        });
    }
}
