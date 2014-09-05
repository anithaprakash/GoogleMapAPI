#region Copyright (c), Some Rights Reserved
/*##########################################################################
 * 
 * GoogleMap.cs
 * -------------------------------------------------------------------------
 * Description:
 * implements asp.net web control that provides handling google map api on server side..
 * 
 * -------------------------------------------------------------------------
 ###########################################################################*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Configuration;

namespace NetGain.GoogleMap
{
    //public enum MapTypes { ROADMAP, SATELLITE, HYBRID, TERRAIN }
    public struct Position
    {
        public Position(double latitude, double longtitude)
        {
            this.latitude = latitude;
            this.longtitude = longtitude;
        }
        public double latitude;
        public double longtitude;

        public override string ToString()
        {
            return "lat:" + latitude + "-lng:" + longtitude;
            //return base.ToString();
        }
    }

    public delegate void MapCenterChangedEventHandler();
    public delegate void ZoomChangedEventHandler();
    public delegate void MapTypeChangedEventHandler();
    public delegate void MapClickedEventHandler(double Latitude, double Longtitude);

    public class GoogleMap : Control, ICallbackEventHandler
    {
        public event MapCenterChangedEventHandler MapCenterChanged;
        public event ZoomChangedEventHandler ZoomChanged;
        public event MapTypeChangedEventHandler MapTypeChanged;
        public event MapClickedEventHandler MapClicked;

        public string JsFilePath = "GoogleMap.js";

        private MapInfo mapinfo = new MapInfo();        
        private MarkerList markers = new MarkerList();

        #region "MapInfo"
        private int zoom;
        private MapTypes mapType;
        private int radius;
       
        public int Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }

        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public MapTypes MapType
        {
            get { return mapType; }
            set { mapType = value; }
        }
        #endregion

        public MapInfo MapInfo
        { get { return mapinfo; } }      


        public MarkerList Markers
        { get { return markers; } }

        private HtmlInputHidden hdMapInfo = new HtmlInputHidden();
        private HtmlInputHidden hdAttachedEvents = new HtmlInputHidden();

        //lat::29.444,,lng::40.11,,imgsrc::http://ghghghh,,imgw::12,,imgh::12||lat::29.33,,lng::40.41,,imgsrc::http://dfdf,imgw:12,imgh:12
        public HtmlInputHidden hdMarkers = new HtmlInputHidden();

        public HtmlInputHidden hdRouteMapInfo = new HtmlInputHidden();

        private Int32 width = 500;
        private Int32 height = 500;           
        

        public Int32 Width
        {
            get
            {
                if (ViewState["Width"] == null)
                {
                    //return uint.Parse(ConfigurationManager.AppSettings["GoogleMapWidth"]);
                    return width;
                }
                return Int32.Parse(ViewState["Width"].ToString());
            }
            set { ViewState["Width"] = value; }
        }
        public Int32 Height
        {
            get
            {
                if (ViewState["Height"] == null)
                {
                    //return uint.Parse(ConfigurationManager.AppSettings["GoogleMapHeight"]);
                    return height;
                }
                return Int32.Parse(ViewState["Height"].ToString());
            }
            set { ViewState["Height"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {

            hdMapInfo.ID = "hdMapInfo";
            hdMapInfo.Name = hdMapInfo.ID;

            hdAttachedEvents.ID = "hdAttachedEvents";
            hdAttachedEvents.Name = hdAttachedEvents.ID;

            hdMarkers.ID = "hdMarkers";
            hdMarkers.Name = hdMarkers.ID;

            hdRouteMapInfo.ID = "hdRouteMapInfo";
            hdRouteMapInfo.Name = hdRouteMapInfo.ID;

            #region mapinfo
            mapinfo.MI_Zoom = Zoom;
            mapinfo.MI_MapType = MapType;
            //mapinfo.MI_Latitude = Latitude;
            //mapinfo.MI_Longtitude = Longtitude;
            #endregion

            Dictionary<string, int> postData = new Dictionary<string, int>();

            string[] allPostKeys = Page.Request.Form.AllKeys;
            for (int i = 0; i < allPostKeys.Length; i++)
                postData.Add(allPostKeys[i], i);

            if (postData.ContainsKey(hdMapInfo.ID))
                hdMapInfo.Value = Page.Request.Form.Get(postData[hdMapInfo.ID]);

            if (postData.ContainsKey(hdAttachedEvents.ID))
                hdAttachedEvents.Value = Page.Request.Form.Get(postData[hdAttachedEvents.ID]);

            if (postData.ContainsKey(hdMarkers.ID))
                hdMarkers.Value = Page.Request.Form.Get(postData[hdMarkers.ID]);

            MapInfo reqMapInfo = MapInfo.FromString(hdMapInfo.Value);

            if (reqMapInfo != null)
            {
                this.mapinfo.MI_MapType = reqMapInfo.MI_MapType;
                this.mapinfo.MI_Zoom = reqMapInfo.MI_Zoom;
                this.mapinfo.MI_Latitude = reqMapInfo.MI_Latitude;
                this.mapinfo.MI_Longtitude = reqMapInfo.MI_Longtitude;
            }

            MarkerList mlist = MarkerList.FromString(hdMarkers.Value);
            if (mlist != null) this.markers = mlist;

            //load mapinfo ,  markers here...

            base.OnInit(e);
        }


        //works on only postbacks
        protected override void Render(HtmlTextWriter writer)
        {
            string AttachedEvents = "";
            if (MapCenterChanged != null) AttachedEvents += "MapCenterChanged,";
            if (ZoomChanged != null) AttachedEvents += "ZoomChanged,";
            if (MapTypeChanged != null) AttachedEvents += "MapTypeChanged,";
            if (MapClicked != null) AttachedEvents += "MapClicked,";

            hdMapInfo.Value = this.mapinfo.ToString();
            hdAttachedEvents.Value = AttachedEvents;
            hdMarkers.Value = markers.ToString();

            hdMapInfo.RenderControl(writer);
            hdAttachedEvents.RenderControl(writer);
            hdMarkers.RenderControl(writer);
            hdRouteMapInfo.RenderControl(writer);

            writer.Write("<div id='map_canvas' style='width:" + this.Width + "px; height:" + this.Height + "px'></div>");
            writer.Write("<script type='text/javascript'>");
            writer.Write("Initialize();");
            writer.Write(" </script>");
            
            writer.Write("<script type='text/javascript'>");
            writer.Write("function DoCallBack(clientarg, callbackfnc, clientcontext, errcallbackfnc)");
            writer.Write("{");
            string s = Page.ClientScript.GetCallbackEventReference(this, "clientarg", "callbackfnc", "clientcontext", "errcallbackfnc", false);
            writer.Write(s + ";");
            writer.Write("}");
            writer.Write(" </script>");

            base.Render(writer);
        }

        protected override void OnPreRender(EventArgs e)
        {
            //js scriptleri referans et
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("jsScript"))
                Page.ClientScript.RegisterClientScriptInclude("jsScript", JsFilePath);

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("gmappath"))
                Page.ClientScript.RegisterClientScriptInclude("gmappath", "http://maps.google.com/maps/api/js?sensor=false");

            base.OnPreRender(e);
        }

        #region ICallbackEventHandler Members

        public void RaiseCallbackEvent(string eventArgument)
        {
            //var clientArg= 'eventtype:'+eventTypeId+'<hdMapInfo>'+mapInfo+'</hdMapInfo>'+'<hdMarkers>'+markersInfo+'</hdMarkers>';

            if (!Regex.IsMatch(eventArgument, "eventtype:.+<hdMapInfo>.+</hdMapInfo><hdMarkers>.*</hdMarkers>"))
                return;

            CultureInfo ciTR = new CultureInfo("tr-TR");

            string eventtype = eventArgument.Substring(10, eventArgument.IndexOf("<hdMapInfo>") - 10);
            string mapInfoStr = eventArgument.Substring(eventArgument.IndexOf("<hdMapInfo>") + "<hdMapInfo>".Length,
                eventArgument.IndexOf("</hdMapInfo>") - "<hdMapInfo>".Length - eventArgument.IndexOf("<hdMapInfo>"));
            string markersStr = eventArgument.Substring(eventArgument.IndexOf("<hdMarkers>") + "<hdMarkers>".Length,
                eventArgument.IndexOf("</hdMarkers>") - "<hdMarkers>".Length - eventArgument.IndexOf("<hdMarkers>"));

            string extra1 = null, extra2 = null, extra3 = null;
            MapInfo reqMapInfo = MapInfo.FromString(mapInfoStr, ref extra1, ref extra2, ref extra3);

            this.markers = MarkerList.FromString(markersStr);

            this.mapinfo.MI_MapType = reqMapInfo.MI_MapType;
            this.mapinfo.MI_Zoom = reqMapInfo.MI_Zoom;
            this.mapinfo.MI_Latitude = reqMapInfo.MI_Latitude;
            this.mapinfo.MI_Longtitude = reqMapInfo.MI_Longtitude;

            //MapInfo mapInfCopy = this.mapinfo.Copy();
            MapInfo mapInfCopyConst = this.mapinfo.Copy();
            MarkerList markerListCopyConst = this.markers.Copy();

            switch (eventtype.ToLower())
            {
                case "mapcenterchanged":
                    if (MapCenterChanged != null)
                        MapCenterChanged.Invoke();
                    break;
                case "zoomchanged":
                    if (ZoomChanged != null)
                        ZoomChanged.Invoke();
                    break;
                case "maptypechanged":
                    if (MapTypeChanged != null)
                        MapTypeChanged.Invoke();
                    break;
                case "mapclicked":
                    if (MapClicked != null)
                        MapClicked.Invoke(
                            double.Parse(extra1.Replace(".", ","), ciTR.NumberFormat),//lat
                            double.Parse(extra2.Replace(".", ","), ciTR.NumberFormat));//lng
                    break;
                default:
                    break;
            }

            responceString = "";

            foreach (Marker m in this.markers.Values)
            {
                if (markerListCopyConst.ContainsKey(m.MarkerId))
                {
                    if (markerListCopyConst[m.MarkerId].ToString() != m.ToString())
                    {
                        //marker updated, updatemarker operation
                        responceString += "||updatemarker####" + m.ToString();
                    }
                }
                else
                {
                    //new marker, addmarker operation
                    responceString += "||addmarker####" + m.ToString();
                }
            }

            if (!string.IsNullOrEmpty(hdRouteMapInfo.Value)) responceString += "||playroutemap####" + hdRouteMapInfo.Value;

            if (this.mapinfo.MI_Latitude != mapInfCopyConst.MI_Latitude || this.mapinfo.MI_Longtitude != mapInfCopyConst.MI_Longtitude)
                responceString += "||setcenter####" + this.mapinfo.MI_Latitude.ToString().Replace(",", ".")
                    + ",," + this.mapinfo.MI_Longtitude.ToString().Replace(",", ".");
            if (this.mapinfo.MI_Zoom != mapInfCopyConst.MI_Zoom)
                responceString += "||setzoom####" + this.mapinfo.MI_Zoom.ToString();
            if (this.mapinfo.MI_MapType != mapInfCopyConst.MI_MapType)
                responceString += "||setmaptype####" + this.mapinfo.MI_MapType.ToString();
           

            //remove 1st char
            if (responceString.Length > 0) responceString = responceString.Substring(2, responceString.Length - 2);

            //burda clientten gelen stringgi parse et gereken return stringi generate et
            //throw new Exception("The method or operation is not implemented.");
        }

        string responceString = "";
        public string GetCallbackResult()
        {
            return responceString;
        }

        public void ShowMap(List<Address> lstAddress)
        {     
            lstAddress = UpdateAddress(lstAddress);
            if (lstAddress.Count > 0)
            {
                //set main address on the map, the first address in the list is taken as main address
                Address addrPrimary = lstAddress.Find(
                delegate(Address addr)
                {
                    return addr.IsAddressPrimary == true;
                }
                );
                Location getLocation = GoogleAddressAPIClient.GetLocationByAddress(addrPrimary);
                MapInfo.MI_Latitude = getLocation.lat;
                MapInfo.MI_Longtitude = getLocation.lng;
                //Set Markers on the map, based on the address list
                foreach (Address addr in lstAddress)
                {
                    if (addr.DistanceFromPrimaryAddress <= Radius)
                    {
                        Location Lat_and_Lon = GoogleAddressAPIClient.GetLocationByAddress(addr);
                        Marker m = new Marker(Guid.NewGuid().ToString(), Lat_and_Lon.lat, Lat_and_Lon.lng);
                        m.InfoWindowOnClick = true;
                        //InfoWindowContentHtml property to be used to pass HTML content which will show the Window when clicked on Marker
                        m.InfoWindowContentHtml = "<b>" + addr.BuyerFriendlyName + "</b>" +
                            "<br>" + addr.BuyerContractName + "<br>" + addr.BuyerContractNumber;
                        m.Tooltip = addr.StreetAddress;
                        Markers.Add(m);
                    }
                }
            }
        }

        /// <summary>
        /// Take List of Address as input.
        /// Finds the Primary Address, if more than one Primary Address, it will take the first marked Primary Address from the list.
        /// If no address is marked as Primary, it will take first address from the Address List as Primary.
        /// </summary>
        /// <returns></returns>
        public List<Address> UpdateAddress(List<Address> lstAddress)
        {
           // List<Address> lstAddress = GetAddressList();
            // Find a book by its ID.
            Address addrPrimary = lstAddress.Find(
            delegate(Address addr)
            {
                return addr.IsAddressPrimary == true;
            }
            );
            if (addrPrimary == null)
            {
                addrPrimary = lstAddress[0];
                lstAddress[0].IsAddressPrimary = true;
            }
            Location Lat_and_Lon = GoogleAddressAPIClient.GetLocationByAddress(addrPrimary);
            double dblStartLat = Lat_and_Lon.lat;
            double dblStartLon = Lat_and_Lon.lng;

            foreach (Address addr in lstAddress)
            {
                Location thisLat_and_Lon = GoogleAddressAPIClient.GetLocationByAddress(addr);
                addr.DistanceFromPrimaryAddress = CDistanceBetweenLocations.Calc(dblStartLat, dblStartLon, thisLat_and_Lon.lat, thisLat_and_Lon.lng);
            }
            return lstAddress;
        }

        void GoogleMap1_MapClicked(double Latitude, double Longtitude)
        {
            Marker m = new Marker(Guid.NewGuid().ToString(), Latitude, Longtitude);
            m.Draggable = true;
            m.InfoWindowOnClick = true;
            m.InfoWindowContentHtml = "New Marker Content Html, Added at: " + DateTime.Now.ToString();
            Markers.Add(m);
        }
        
        #endregion
    }
}
