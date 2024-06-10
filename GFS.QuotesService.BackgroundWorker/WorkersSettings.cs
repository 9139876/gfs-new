namespace GFS.QuotesService.BackgroundWorker;

internal class WorkersSettings
{
    public ushort UpdateQuotesSleepInSeconds { get; init; }
    public ushort UpdateAssetsListSleepInSeconds { get; init; }
}