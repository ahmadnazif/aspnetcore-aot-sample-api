using ApiNativeAot.Models;

namespace ApiNativeAot.Services;

public class InMemorySimpleDb
{
    private Dictionary<string, Sms> Smses { get; set; } = [];

    public async Task<Sms> GetSmsAsync(string id)
    {
        var sms = !await IsSmsExistAsync(id) ? null : Smses[id];
        return await Task.FromResult(sms);
    }

    public async Task<List<Sms>> ListAllSmsAsync()
    {
        var all = Smses.Values.ToList();
        return await Task.FromResult(all);
    }

    public async Task<PostResponse> AddSmsAsync(string id, string from, string to, string text)
    {
        Smses.Add(id, new()
        {
            SmsId = id,
            From = from,
            To = to,
            Text = text,
            CreatedTimeUtc = DateTime.UtcNow
        });

        return await Task.FromResult(new PostResponse
        {
            IsSuccess = true,
            Message = $"SMS '{id}' added"
        });
    }

    public async Task<PostResponse> EditSmsAsync(string id, string from, string to, string text)
    {
        if (!await IsSmsExistAsync(id))
            await AddSmsAsync(id, from, to, text);
        else
        {
            var oldCt = Smses[id].CreatedTimeUtc;

            Smses[id] = new Sms
            {
                SmsId = id,
                From = from,
                To = to,
                Text = text,
                CreatedTimeUtc = oldCt
            };
        }

        return await Task.FromResult(new PostResponse
        {
            IsSuccess = true,
            Message = $"SMS '{id}' updated"
        });
    }

    public async Task<PostResponse> DeleteSmsAsync(string id)
    {
        PostResponse resp;

        if (!await IsSmsExistAsync(id))
            resp = new() { IsSuccess = false, Message = $"SMS '{id}' not exist" };
        else
        {
            Smses.Remove(id);
            resp = new() { IsSuccess = true, Message = $"SMS '{id}' deleted" };
        }

        return await Task.FromResult(resp);
    }

    private async Task<bool> IsSmsExistAsync(string id) => await Task.FromResult(Smses.TryGetValue(id, out _));

}
