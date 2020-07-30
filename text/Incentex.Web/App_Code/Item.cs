using System;
using System.Collections.Generic;
using System.Text;

public class Item
{
	private string m_CargoReleaseNumber;
	private string m_PieceQuantity;
	private string m_PartNumber;
	private string m_Pieces;
    private string m_SalesOrderNumber;
	private List<Item> m_ContainedItems = new List<Item>();

	public string CargoReleaseNumber
	{
		get { return m_CargoReleaseNumber; }
		set { m_CargoReleaseNumber = value; }
	}

	public string PieceQuantity
	{
		get { return m_PieceQuantity; }
		set { m_PieceQuantity = value; }
	}

	public string PartNumber
	{
		get { return m_PartNumber; }
		set { m_PartNumber = value; }
	}

	public string Pieces
	{
		get { return m_Pieces; }
		set { m_Pieces = value; }
	}

    public string SalesOrderNumber
    {
        get { return m_SalesOrderNumber; }
        set { m_SalesOrderNumber = value; }
    }
	public List<Item> ContainedItems
	{
		get { return m_ContainedItems; }
	}
}

