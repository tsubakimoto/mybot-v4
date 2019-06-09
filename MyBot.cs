using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

public class MyBot : IBot
{
    private readonly MyStateAccessors accessors;
    private readonly DialogSet dialogs;

    public MyBot(MyStateAccessors accessors)
    {
        this.accessors = accessors;
        this.dialogs = new DialogSet(accessors.ConversationDialogState);
        this.dialogs.Add(new TextPrompt("name"));
    }

    public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
    {
        if (turnContext.Activity.Type == ActivityTypes.Message)
        {
            var dialogContext = await dialogs.CreateContextAsync(turnContext, cancellationToken);
            var results = await dialogContext.ContinueDialogAsync(cancellationToken);

            if (results.Status == DialogTurnStatus.Empty)
            {
                await dialogContext.PromptAsync(
                    "name",
                    new PromptOptions { Prompt = MessageFactory.Text("名前を入力してください") },
                    cancellationToken
                );
            }
            else if (results.Status == DialogTurnStatus.Complete)
            {
                if (results.Result != null)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"ようこそ '{results.Result}' さん！"));
                }
            }

            await accessors.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
        }
    }
}