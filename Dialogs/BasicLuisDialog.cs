using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public string CurrentIntent = "";
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Greeting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
            string[] greetings = new string[] { $"How may I help you?" , $"Hello, how may I be of service?" , $"Hi, how can I help?" };
            Random rnd = new Random();
            await context.PostAsync(greetings.GetValue(rnd.Next(0,greetings.Length-1)).ToString());
        }

        [LuisIntent("Service")]
        public async Task ServiceIntent(IDialogContext context, LuisResult result)
        {
            //await this.ShowLuisResult(context, result);
            CurrentIntent = "Service";
            await context.PostAsync($"Which service do you need? Documents? Reports? Transmittals?");
            context.ConversationData.SetValue("Intent", "Service");
        }

        [LuisIntent("Service.Document")]
        public async Task DocumentServiceIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);            
        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
            context.ConversationData.SetValue("Intent", "");
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }
    }
}