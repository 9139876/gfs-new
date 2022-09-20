using AutoMapper;
using GFS.Common.Extensions;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BL.Models;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface ITinkoffAdapter : IQuotesProviderAdapter
{
}

public class TinkoffAdapter : ITinkoffAdapter
{
    private readonly InvestApiClient _apiClient;
    private readonly Mapper _mapper;

    public TinkoffAdapter(
        InvestApiClient apiClient,
        Mapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public async Task<List<InitialModel>> GetInitialData()
    {
        var result = new List<InitialModel>();

        var sharesResponse = await _apiClient.Instruments.SharesAsync();
        sharesResponse.RequiredNotNull();
        var shares = _mapper.Map<List<InitialModel>>(sharesResponse.Instruments.Where(s => s != null));
        shares.ForEach(share =>
        {
            share.AssetType = AssetTypeEnum.Shares;
            share.MarketType = GetMarketType(share.Exchange);
        });
        result.AddRange(shares);

        var currenciesResponse = await _apiClient.Instruments.CurrenciesAsync(new InstrumentsRequest());
        currenciesResponse.RequiredNotNull();
        var currencies = _mapper.Map<List<InitialModel>>(currenciesResponse.Instruments.Where(s => s != null));
        currencies.ForEach(currency =>
        {
            currency.AssetType = AssetTypeEnum.Currencies;
            currency.MarketType = GetMarketType(currency.Exchange);
        });
        result.AddRange(currencies);

        var etfsResponse = await _apiClient.Instruments.EtfsAsync();
        etfsResponse.RequiredNotNull();
        var etfs = _mapper.Map<List<InitialModel>>(etfsResponse.Instruments.Where(s => s != null));
        etfs.ForEach(etf =>
        {
            etf.AssetType = AssetTypeEnum.Etfs;
            etf.MarketType = GetMarketType(etf.Exchange);
        });
        result.AddRange(etfs);

        return result;
    }

    #region static

    private static MarketTypeEnum GetMarketType(string exchange)
        => exchange switch
        {
            _ when exchange.StartsWith("MOEX") => MarketTypeEnum.MOEX,
            _ when exchange.StartsWith("SPB") => MarketTypeEnum.SPB,
            _ when exchange.StartsWith("LSE") => MarketTypeEnum.LSE,
            _ => MarketTypeEnum.Unknown
        };

    #endregion
}