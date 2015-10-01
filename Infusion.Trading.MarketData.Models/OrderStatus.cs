namespace Infusion.Trading.MarketData.Models
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Filled,
        Cancelled,
        Replaced,
        Rejected,
        //NewRejected,
        //CancelRejected,
        //ReplaceRejected,
        //Frozen,
        MarketToLimit,
        //Triggered,
        PartiallyFilled,
        CancelledOfImmediateOrCancel,
        //BmsReject,
        Max
    }
}