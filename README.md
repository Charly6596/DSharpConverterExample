# Custom command parameter types converting DSharpPlus entities
This is a basic example of converting DSharpPlus entities to your own entities. Might be useful if you follow the [Miunie architechture](https://github.com/discord-bot-tutorial/Miunie).

##### NOTE: This is not aguide about using your own converters, but you could learn about that with this example.

## Getting started
### Prerequisites
You need a Discord bot using the DSharpPlus library. This project have a simple bot able to connect and execute commands. In the case you download this project, modify the `config.json` file with is in the project fodler:
```json
{
    "token": "YOUR-TOKEN",
    "prefix": "BOT-PREFIX"
}
```
To run the project, you can use the [.NET Core CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x) run command in the project folder:
```
dotnet run
```
### Creating the data model
In this example, we will convert a `DiscordMember` to our own model, `CustomUser`. The first thing to do, is create the data model of the `CustomUser`:
```cs
public class CustomUser
{
    public string Name { get; set; }
    public ulong Id { get; set; }
}
```
This should be enough to create our converter. Remember, you can use your own model, with more information if needed.
### Creating and understanding the converter 
To create a DSharpPlus converter, it's as simple as creating a class that inherits the `IArgumentConverter<T>` class, where `T` is the type you want to convert to. In our example, we will inherit `IArgumentConverter<CustomUser>`.

This interface has a method we must implement, `bool TryConvert(string userInput, CommandContext context, T out result)`, where the `userInput` is the text the user inputs as command parameter, and the `T out result` is the conversion result output.
This method allows us to convert from the string user input, to anything that is convertible from string, for example, take a look at this [`MathOperationConverter`](https://github.com/DSharpPlus/Example-Bots/blob/master/DSPlus.Examples.CSharp.Ex02/MathOperationConverter.cs) wich converts a string input to a enum.

However, we're not converting from `string` to `CustomUser` directly, we will take adventage of the already-implemented string to `DiscordMember` converter, [`DiscordMemberConverter`](https://dsharpplus.emzi0767.com/api/DSharpPlus.CommandsNext.Converters.DiscordMemberConverter.html), so, we will need an instance of a `DiscordMemberConverter` in our `CustomUserConverter`:
```cs
public class CustomUserConverter : IArgumentConverter<CustomUser>
{
    private DiscordMemberConverter _discordMemberConverter;
    public CustomUserConverter()
    {
        _discordMemberConverter = new DiscordMemberConverter();
    }
    
    public bool TryConvert(string userInput, CommandContext context, out CustomUser customUser)
    {            
        throw new NotImplementedException();
    }
}
```
Then, we can convert the user input to a `DiscordMember` in our `TryConvert` method:
```cs
public bool TryConvert(string userInput, CommandContext context, out Customuser customUser)
{
    DiscordMember discordMember;
    var result = _discordMemberConverter.TryConvert(userInput, context, out discordMember);
    return result;
}
```
We're converting the `userInput` to a `DiscordMember` using the `TryConvert` method of the `DiscordMemberConverter`. Since our converter mainly depends on the `DiscordMemberConverter` to be suscessful, we return its return value, wich is a `bool` that indicates if it was able to perform the conversion.
But this should give us an error, we don't assing the value of our `customUser` output value.
We have to convert from a `Discordmember` to our `CustomUser`, to achieve that, I've created the following method:
```cs
private CustomUser DiscordMemberToCustomUser(DiscordMember discordMember)
{
    var customUser = new CustomUser();
    customUser.Name = discordMember.Username;
    customUser.Id = discordMember.Id;
    return customUser;
}
```
And this is how the implementation of `TryConvert` looks like:
```cs
public bool TryConvert(string value, CommandContext context, out CustomUser customUser)
{
    DiscordMember discordMember;
    var result = _discordMemberConverter.TryConvert(value, context, out discordMember);
    customUser = DiscordMemberToCustomUser(discordMember);
    return result;
}
```

### Registering the converter
In order to let our command handler know about the conversion we just implemented, we have to register the converter in our `CommandsNextModule`, using the `CommandsNextUtilities` class:
```cs
var customUserConverter = new CustomUserConverter();
CommandsNextUtilities.RegisterConverter(customUserConverter);
CommandsNextUtilities.RegisterUserFriendlyTypeName<CustomUserConverter>("CustomUser");
```
And that's it! 🎉 we just implemented our DSharpPlus entity to our own entity, with the possibility of using it as command parameter

### Using our entity as command parameter
Once we have our converter setted up, we can start using our `CustomUser` as command parameter:
```cs
[Command("profile")]
public async Task ShowProfile(CommandContext context, CustomUser user)
{
    await context.Channel.SendMessageAsync($"User ID: {user.Id}\nUser Name: {user.Name}");
}
```
<p align="center">
  <img src="https://i.imgur.com/FtPgCJR.png">
</p>

## Authors

* **Charly6596** - [Github profile](https://github.com/Charly6596) - Discord: Charly#7094

## Bibliography and sources

* [Discord-BOT-tutorial](https://discord.gg/cGhEZuk) discord server.
* [C# Discord Bot Common Issues GitHub repository](https://github.com/discord-bot-tutorial/common-issues).
* [DSharpPlus Example-Bots GitHub repository](https://github.com/DSharpPlus/Example-Bots).
* [DSharpPlus documentation](https://dsharpplus.emzi0767.com/index.html).
