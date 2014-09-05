using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace NetGain.GoogleMap
{
    public delegate void MarkerClickedEventHandler(string MarkerId);

    public class Marker
    {
        //public event MarkerClickedEventHandler MarkerClicked;

        private string markerid;

        private double latitude;
        private double longtitude;
        private string imgsrc = null;
        private uint? imgh;
        private uint? imgw;
        private string tooltip;

        private bool infoWindowOnClick = false;
        private string infoWindowContentHtml = null;
        private bool visible = true;
        private bool draggable = false;

        public bool Draggable
        {
            get { return draggable; }
            set { draggable = value; }
        }       

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool InfoWindowOnClick
        {
            get { return infoWindowOnClick; }
            set { infoWindowOnClick = value; }
        }

        public string InfoWindowContentHtml
        {
            get { return infoWindowContentHtml; }
            set { infoWindowContentHtml = value; }
        }

        public Marker()
        { }

        public Marker(string MarkerId, double Latitude, double Longtitude)
            : this(MarkerId, Latitude, Longtitude, null) { }

        public Marker(string MarkerId, double Latitude, double Longtitude, string ImageSrc)
            : this(MarkerId, Latitude, Longtitude, ImageSrc, null, null) { }

        public Marker(string MarkerId, double Latitude, double Longtitude, string ImageSrc, uint? ImageHeigth, uint? ImageWidth)
        {
            markerid = MarkerId;
            latitude = Latitude;
            longtitude = Longtitude;
            imgsrc = ImageSrc;
            imgh = ImageHeigth;
            imgw = ImageWidth;
        }

       

        public override string ToString()
        {
            string retstr = "markerid::" + markerid + ",,lat::" + latitude.ToString().Replace(",", ".") +
                ",,lng::" + longtitude.ToString().Replace(",", ".") + ",,visible::" + visible.ToString().ToLower() + 
                ",,draggable::" + draggable.ToString().ToLower();

            if (!string.IsNullOrEmpty(imgsrc)) retstr += ",,imgsrc::" + imgsrc;
            if (imgh != null) retstr += ",,imgh::" + imgh;
            if (imgw != null) retstr += ",,imgw::" + imgw;
            if (!string.IsNullOrEmpty(tooltip)) retstr += ",,tooltip::" + tooltip;
            if (InfoWindowOnClick && !string.IsNullOrEmpty(infoWindowContentHtml)) retstr += ",,infoWindowContentHtml::" + infoWindowContentHtml;

            return retstr;
        }

        public static Marker FromString(string MarkerStr)
        {
            string[] MarkerProps = MarkerStr.Split(new string[] { ",," }, StringSplitOptions.None);
            CultureInfo ciTR = new CultureInfo("tr-TR");
            Marker m = new Marker();

            foreach (string prop in MarkerProps)
            {
                string[] propArr = prop.Split(new string[] { "::" }, StringSplitOptions.None);
                if (propArr.Length != 2) continue;
                switch (propArr[0])
                {
                    case "markerid":
                        m.MarkerId = propArr[1];
                        break;

                    case "lat":
                        m.latitude = double.Parse(propArr[1].Replace(".", ","), ciTR.NumberFormat);
                        break;

                    case "lng":
                        m.longtitude = double.Parse(propArr[1].Replace(".", ","), ciTR.NumberFormat);
                        break;

                    case "visible":
                        m.visible = bool.Parse(propArr[1]);
                        break;

                    case "imgsrc":
                        m.imgsrc = propArr[1];
                        break;

                    case "imgh":
                        m.imgh = uint.Parse(propArr[1]);
                        break;

                    case "imgw":
                        m.imgw = uint.Parse(propArr[1]);
                        break;

                    case "tooltip":
                        m.tooltip = propArr[1];
                        break;

                    case "draggable":
                        m.draggable = bool.Parse(propArr[1]);
                        break;

                    case "infoWindowContentHtml":
                        m.infoWindowContentHtml = propArr[1];
                        m.infoWindowOnClick = true;
                        break;

                    default:
                        break;
                }
            }

            return m;

        }


        public string MarkerId
        {
            get { return markerid; }
            set { markerid = value; }
        }

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public double Longtitude
        {
            get { return longtitude; }
            set { longtitude = value; }
        }

        public string ImgSrc
        {
            get { return imgsrc; }
            set
            {
                if (value != null)
                    if (value.Contains(",,") || value.Contains("::"))
                        throw new ArgumentException("ImageSrc not allowed to contain  ,,  or ::  ");

                imgsrc = value;
            }
        }

        public uint? ImgH
        {
            get { return imgh; }
            set
            {
                imgh = value;
            }
        }

        public uint? ImgW
        {
            get { return imgw; }
            set
            {
                imgw = value;
            }
        }

        public string Tooltip
        {
            get { return tooltip; }
            set
            {
                if (value != null)
                    if (value.Contains(",,") || value.Contains("::"))
                        throw new ArgumentException("Tooltip not allowed to contain  ,,  or ::  ");

                tooltip = value;
            }
        }
    }
}
