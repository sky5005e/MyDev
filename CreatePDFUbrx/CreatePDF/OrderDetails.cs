using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Data;

namespace CreatePDF
{
    public class InvoiceInfo
    {


        /// <summary>
        /// Genearete Pdf Invoice
        /// </summary>
        /// <param name="objOrder">OrderDetails object</param>
        /// <returns>bytes </returns>
        public void GeneratePdfInvoice(String path, OrderDetails objOrder)
        {
            new InvoiceTemplates().GenerateUberInvoicePDF(path, objOrder);
        }

        public void GenerateHotelsPdfInvoice(String path, HotelsDetails objhotels)
        {
            new Hotel_Invoice().GenerateInvoicePDF(path, objhotels);
        }

        public void GenerateAirtelPdfInvoice(String path)
        {
            new AirtelBills().GenerateInvoicePDF(path);
        }

        public void GenerateHotelMainsPdfInvoice(String path, HotelsDetails objhotels)
        {
            new HotelMain().GenerateInvoicePDF(path, objhotels);
        }

    }
    public class HotelsDetails
    {

        public String date { get; set; }
        public string name { get; set; }
        public String daytime { get; set; }
        public String totalAmount { get; set; }
        public String TripFare { get; set; }
        public String subtotal { get; set; }
        public String BookingFee { get; set; }
        public String DriverFirstName { get; set; }
        public String DriverLastName { get; set; }
        public String distanceKM { get; set; }

        public String time { get; set; }
        public String timefrom { get; set; }
        public String timeTo { get; set; }
        public String FromAddress { get; set; }
        public String ToAddress { get; set; }

    }
    public class OrderDetails
    {

        public String date { get; set; }
        public String daytime { get; set; }
        public String totalAmount { get; set; }
        public String TripFare { get; set; }
        public String subtotal { get; set; }
        public String BookingFee { get; set; }
        public String DriverFirstName { get; set; }
        public String DriverLastName { get; set; }
        public String distanceKM { get; set; }

        public String time { get; set; }
        public String timefrom { get; set; }
        public String timeTo { get; set; }
        public String FromAddress { get; set; }
        public String ToAddress { get; set; }

    }

    public class OrderDetails1
    {

        public String date { get; set; }
        public String daytime { get; set; }
        public String totalAmount { get; set; }
        public String terifa { get; set; }
        public String tiempo { get; set; }
        public String distancia { get; set; }
        public String subtotal { get; set; }
        public String Cuotadesolicitud { get; set; }
        public String ReAdjusto { get; set; }
        public String DriverFirstName { get; set; }
        public String DriverLastName { get; set; }
        public String distanceKM { get; set; }

        public String time { get; set; }
        public String timefrom { get; set; }
        public String timeTo { get; set; }
        public String FromAddress { get; set; }
        public String ToAddress { get; set; }

    }


}
