using GFS.QuotesService.Cli.Arguments;
using Spectre.Console.Cli;

namespace GFS.QuotesService.Cli.Commands;

internal class GetTasksCommand : Command<GetTasksInfoCmdArguments>
{
    public override int Execute(CommandContext context, GetTasksInfoCmdArguments settings)
    {
        Console.WriteLine($"Execute {GetType().Name}");
        
        Console.WriteLine($"context.Name = {context.Name}");
        Console.WriteLine($"context.Data = {context.Data}");
        
        return 0;
    }
}

internal class GetQuotesInfoCommand : Command<GetQuotesInfoCmdArguments>
{
    public override int Execute(CommandContext context, GetQuotesInfoCmdArguments settings)
    {
        Console.WriteLine($"Execute {GetType().Name}");
        
        return 0;
    }
}