using System;
using System.Collections.Generic;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class controller_ReTransmitToSAP : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            List<GetOrdersToBeRetransmittedToSAPResult> lstOrdersToRetransmit = new SAPRepository().GetOrdersToBeRetransmittedToSAP();

            foreach (GetOrdersToBeRetransmittedToSAPResult objOrderToRetransmit in lstOrdersToRetransmit)
                new SAPOperations().SubmitOrderToSAP(objOrderToRetransmit.OrderID);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}