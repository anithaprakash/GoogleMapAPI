using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace NetGain.GoogleMap
{
    public enum MapTypes { ROADMAP, SATELLITE, HYBRID, TERRAIN }
   

    public class MapInfo
    {
        #region "MapInfo"
        private double milatitude;
        private double milongtitude;
        private int mizoom;
        private MapTypes mimapType;

        public double MI_Latitude
        {
            get { return milatitude; }
            set { milatitude = value; }
        }
        public double MI_Longtitude
        {
            get { return milongtitude; }
            set { milongtitude = value; }
        }
        public int MI_Zoom
        {
            get { return mizoom; }
            set { mizoom = value; }
        }
        public MapTypes MI_MapType
        {
            get { return mimapType; }
            set { mimapType = value; }
        }

        public override string ToString()
        {
            return

            string.Format("lat:{0}|lng:{1}|zoom:{2}|maptype:{3}",
                milatitude.ToString().Replace(",", "."),
                milongtitude.ToString().Replace(",", "."),
                mizoom.ToString(),
                mimapType.ToString())

            ;

            // return base.ToString();
        }

        public static MapInfo FromString(string MapInfoStr)
        {
            string extra1 = null, extra2 = null, extra3 = null;
            return FromString(MapInfoStr, ref  extra1, ref  extra2, ref  extra3);
        }

        public static MapInfo FromString(string MapInfoStr, ref string extra1, ref string extra2, ref string extra3)
        {
            //if(Regex.IsMatch(MapInfoStr,"lat:.+|lng")
            if (!MapInfoStr.Contains("lat:") || !MapInfoStr.Contains("lng:")
                || !MapInfoStr.Contains("zoom:") || !MapInfoStr.Contains("maptype:"))
                return null;

            //extra1 = null; extra2 = null; extra3 = null;

            CultureInfo ciTR = new CultureInfo("tr-TR");

            MapInfo inf = new MapInfo();
            string[] infArr = MapInfoStr.Split('|');
            string maptypeStr = null;

            foreach (string param in infArr)
            {
                string[] paramArr = param.Split(':');
                if (paramArr.Length == 2)
                {
                    switch (paramArr[0])
                    {
                        case "lat":
                            inf.milatitude = double.Parse(paramArr[1].Replace(".", ","), ciTR.NumberFormat);
                            break;

                        case "lng":
                            inf.milongtitude = double.Parse(paramArr[1].Replace(".", ","), ciTR.NumberFormat);
                            break;

                        case "zoom":
                            inf.mizoom = int.Parse(paramArr[1]);
                            break;

                        case "maptype":
                            maptypeStr = paramArr[1];
                            break;

                        case "extra1":
                            extra1 = paramArr[1];
                            break;

                        case "extra2":
                            extra2 = paramArr[1];
                            break;

                        case "extra3":
                            extra3 = paramArr[1];
                            break;

                        default:
                            break;
                    }
                }
            }

            switch (maptypeStr.ToLower(new CultureInfo("en-US")))
            {
                case "satellite":
                    inf.mimapType = MapTypes.SATELLITE;
                    break;
                case "roadmap":
                    inf.mimapType = MapTypes.ROADMAP;
                    break;
                case "hybrid":
                    inf.mimapType = MapTypes.HYBRID;
                    break;
                case "terrain":
                    inf.mimapType = MapTypes.TERRAIN;
                    break;
                default:
                    inf.mimapType = MapTypes.ROADMAP;
                    break;
            }

            return inf;
        }

        public MapInfo Copy()
        {
            MapInfo inf = new MapInfo();
            inf.milatitude = this.milatitude;
            inf.milongtitude = this.milongtitude;
            inf.mimapType = this.mimapType;
            inf.mizoom = this.mizoom;

            return inf;
        }
        #endregion
    }
}
