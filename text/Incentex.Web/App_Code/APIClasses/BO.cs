using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Summary description for BO
/// </summary>
public class BO
{
    public BO()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Documents Documents { get; set; }

    public List<Document_LineRow.row> Document_Lines { get; set; }

    public AddressExtension AddressExtension { get; set; }

    public override String ToString()
    {
        Type objBO = (typeof(BO));
        String URL = String.Empty;
        Int32 CNT = 0;
        foreach (PropertyInfo propertyInfo in objBO.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (CNT == 0)
            {
                URL += String.Format("{0}={1}", propertyInfo.Name, propertyInfo.GetValue(this, null));
                CNT++;
            }
            else
                URL += String.Format("&{0}={1}", propertyInfo.Name, propertyInfo.GetValue(this, null));
        }
        return URL;
    }
}