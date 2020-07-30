using System;

/// <summary>
/// Summary description for FreeShipping
/// </summary>
[Serializable]
public class FreeShipping
{
    public FreeShipping()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public String ShippingProgramFor { get; set; }

    public Boolean IsFreeShippingActive { get; set; }

    public DateTime ShippingProgramStartDate { get; set; }

    public DateTime ShippingProgramEndDate { get; set; }

    public Boolean IsSaleShipping { get; set; }

    public Decimal MinimumShippingAmount { get; set; }

    public Decimal ShippingPercentOfSale { get; set; }

    public Decimal TotalSaleAbove { get; set; }
}
