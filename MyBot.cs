using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

public class MyBot : IBot
{
    public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
    {
        if (turnContext.Activity.Type == ActivityTypes.Message
            && !string.IsNullOrWhiteSpace(turnContext.Activity.Text))
            await turnContext.SendActivityAsync(turnContext.Activity.Text);
    }
}