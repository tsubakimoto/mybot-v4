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
        dialogs.Add(new ProfileDialog(accessors));
    }

    public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
    {
        if (turnContext.Activity.Type == ActivityTypes.Message)
        {
            var dialogContext = await dialogs.CreateContextAsync(turnContext, cancellationToken);
            var results = await dialogContext.ContinueDialogAsync(cancellationToken);

            if (results.Status == DialogTurnStatus.Empty)
            {
                await dialogContext.BeginDialogAsync(nameof(ProfileDialog), null, cancellationToken);
            }
            else if (results.Status == DialogTurnStatus.Complete)
            {
                var userProfile = await accessors.UserProfile.GetAsync(turnContext);
                await turnContext.SendActivityAsync(MessageFactory.Text($"ようこそ '{userProfile.Name}' さん！"));
            }

            await accessors.UserState.SaveChangesAsync(turnContext, false, cancellationToken);
            await accessors.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
        }
    }
}