using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

public class MyMiddleware : IMiddleware
{
    public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next, CancellationToken cancellationToken = default)
    {
        // 添付ファイルがある
        var activity = turnContext.Activity;
        if (activity.Type == ActivityTypes.Message
            && activity.Attachments != null
            && activity.Attachments.Any())
            await turnContext.SendActivityAsync("テキストを送ってください");
        else
            await next.Invoke(cancellationToken);
    }
}
