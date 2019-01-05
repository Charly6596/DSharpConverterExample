using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlusConvertersPlayaround.Model;

namespace DSharpPlusConvertersPlayaround.Commands
{
    public class ExampleConversionCommand
    {
        [Command("profile")]
        public async Task ShowProfile(CommandContext context, CustomUser user)
        {
            await context.Channel.SendMessageAsync($"User ID: {user.Id}\nUser Name: {user.Name}");
        }
    }
}
