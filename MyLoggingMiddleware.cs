using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

public class MyLoggingMiddleware : IMiddleware
{
    public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next, CancellationToken cancellationToken = default)
    {
        Debug.WriteLine($"{turnContext.Activity.From}:{turnContext.Activity.Type}");
        Debug.WriteLineIf(
            !string.IsNullOrEmpty(turnContext.Activity.Text),
            turnContext.Activity.Text);
        await next.Invoke(cancellationToken);
    }
}
