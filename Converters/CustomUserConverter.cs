using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlusConvertersPlayaround.Model;

namespace DSharpPlusConvertersPlayaround.Converters
{
    public class CustomUserConverter : IArgumentConverter<CustomUser>
    {
        //To convert from a DiscordMember to our custom user type,
        //we need the original DiscordMember converter, since it already works
        //we can avoid converting our own user input to CustomUser converter
        private DiscordMemberConverter _discordMemberConverter;
        public CustomUserConverter()
        {
            _discordMemberConverter = new DiscordMemberConverter();
        }

        //Method to convert from DiscordMember to CustomMember
        private CustomUser DiscordMemberToCustomUser(DiscordMember discordMember)
        {
            var customUser = new CustomUser();
            customUser.Name = discordMember.Username;
            customUser.Id = discordMember.Id;
            return customUser;
        }
        public bool TryConvert(string value, CommandContext context, out CustomUser customUser)
        {            
            //First, we convert the user input (value) to a DiscordMember
            DiscordMember discordMember;
            var result = _discordMemberConverter.TryConvert(value, context, out discordMember);     
            //Then, convert the DiscordMember to our CustomUser
            customUser = DiscordMemberToCustomUser(discordMember);
            return result;
        }
    }
}
