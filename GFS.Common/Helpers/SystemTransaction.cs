using System.Transactions;

namespace GFS.Common.Helpers;

public class SystemTransaction
{
    public static TransactionScope Default() => new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);
}