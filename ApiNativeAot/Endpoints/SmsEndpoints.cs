using ApiNativeAot.Models;
using ApiNativeAot.Services;

namespace ApiNativeAot.Endpoints;

public static class SmsEndpoints
{
    public static void AddSmsEndpoints(this IEndpointRouteBuilder builder, InMemorySimpleDb db)
    {
        var smsEndpoint = builder.MapGroup("/sms");

        smsEndpoint.MapGet("/", () =>
        {
            return "Please use: /get, /list-all, /add, /edit, or /delete endpoint";
        });

        smsEndpoint.MapGet("/get", async (string id) =>
        {
            return await db.GetSmsAsync(id);
        });

        smsEndpoint.MapGet("/list-all", async () =>
        {
            return await db.ListAllSmsAsync();
        });

        smsEndpoint.MapPost("/add", async (SmsBase? sms) =>
        {
            if (sms == null)
                return new PostResponse { IsSuccess = false, Message = "Please provide data" };

            var id = Guid.NewGuid().ToString();
            return await db.AddSmsAsync(id, sms.From, sms.To, sms.Text);
        });

        smsEndpoint.MapPut("/edit", async (SmsBase? sms) =>
        {
            if (sms == null)
                return new PostResponse { IsSuccess = false, Message = "Please provide data" };

            if (string.IsNullOrWhiteSpace(sms.SmsId))
                return new PostResponse { IsSuccess = false, Message = "SmsId is required" };

            return await db.EditSmsAsync(sms.SmsId, sms.From, sms.To, sms.Text);
        });

        smsEndpoint.MapDelete("/delete", async (string id) =>
        {
            return await db.DeleteSmsAsync(id);
        });
    }
}
