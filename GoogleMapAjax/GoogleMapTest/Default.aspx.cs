using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NetGain.GoogleMap;
using System.Collections.Generic;
using System.Net;
using System.Web.Configuration;
//using System.Web.Script.Serialization;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            rptAddress.DataSource = GetAddressList();
            rptAddress.DataBind();
            GoogleMap1.ShowMap(GetAddressList());
            Repeater1.DataSource = GetAddressList1();
            Repeater1.DataBind();
            GoogleMap2.ShowMap(GetAddressList1());
        }
        //GoogleMap1.MapCenterChanged += new GoogleMap.MapCenterChangedEventHandler(GoogleMap1_MapCenterChanged);
        //GoogleMap1.MapClicked += new MapClickedEventHandler(GoogleMap1_MapClicked);
    }

    protected List<Address> GetAddressList()
    {
        List<Address> lstAddress = new List<Address>();
        lstAddress.Add(new Address("2517, Dunksferry Road", "", "","","Bensalem","PA","USA","19020", false,"AgencyName","RFPNumber"));
        lstAddress.Add(new Address("3600 Horizon Blvd", "", "", "", "Trevose", "PA", "USA", "19053", false, "AgencyName", "RFPNumber"));
        lstAddress.Add(new Address("65 Bellwood Dr", "", "", "", "Trevose", "PA", "USA", "19053", false, "AgencyName", "RFPNumber"));
        lstAddress.Add(new Address("345 W Signal Hill Rd", "", "", "", "King of Prussia", "PA", "USA", "19406", false, "AgencyName", "RFPNumber"));
        return lstAddress;
    }

    protected List<Address> GetAddressList1()
    {
        List<Address> lstAddress = new List<Address>();
        lstAddress.Add(new Address("577 Haddon Ave", "", "", "", "Collingswood", "NJ", "USA", "08108", false, "NetGain", "RFPNumber"));
        lstAddress.Add(new Address("424 CollingsAve", "", "", "", "Collingswood", "NJ", "USA", "08108", false, "Collingswood High School", "RFPNumber"));
        lstAddress.Add(new Address("400 Comly Ave", "", "", "", "Oaklyn", "NJ", "USA", "08108", false, "Collingswood Special Education", "RFPNumber"));
        lstAddress.Add(new Address("", "", "", "", "Haddon TWP", "NJ", "USA", "08108", false, "Collingswood Police Department", "RFPNumber"));
        return lstAddress;
    }
}
