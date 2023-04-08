using GFS.Common.Extensions;
using GFS.Common.Interfaces;

namespace GFS.Portfolio.Common.Models;

public class PortfolioSettings : IValidatedModel
{
    public void ValidateModel()
    {
        this.Validate();
    }
}