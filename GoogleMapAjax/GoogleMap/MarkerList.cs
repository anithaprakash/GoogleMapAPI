using System;
using System.Collections.Generic;
using System.Text;

namespace NetGain.GoogleMap
{
    public class MarkerList : Dictionary<string, Marker>
    {

        //public new bool Remove(string Key)
        //{
        //    //throw exception
        //    return false;
        //}

        public void Add(Marker m)
        {
            this.Add(m.MarkerId, m);
        }

        public override string ToString()
        {
            StringBuilder sbret = new StringBuilder();
            foreach (Marker m in this.Values)
            {
                sbret.Append(m.ToString() + "||");
            }

            return sbret.ToString();

            //return base.ToString();
        }

        public static MarkerList FromString(string MarkerListStr)
        {
            string[] MarkersArr = MarkerListStr.Split(new string[] { "||" }, StringSplitOptions.None);
            MarkerList mlist = new MarkerList();

            foreach (string markerStr in MarkersArr)
            {
                Marker m = Marker.FromString(markerStr);

                if (m == null) continue;
                if (string.IsNullOrEmpty(m.MarkerId)) continue;

                if (!mlist.ContainsKey(m.MarkerId))
                    mlist.Add(m);
            }

            return mlist;
        }

        public MarkerList Copy()
        {
            MarkerList mlist = new MarkerList();

            foreach (Marker m in this.Values)
            {
                Marker mc = new Marker();
                mc.ImgH = m.ImgH;
                mc.ImgSrc = m.ImgSrc;
                mc.ImgW = m.ImgW;
                mc.InfoWindowContentHtml = m.InfoWindowContentHtml;
                mc.InfoWindowOnClick = m.InfoWindowOnClick;
                mc.Latitude = m.Latitude;
                mc.Longtitude = m.Longtitude;
                //mc.MarkerClicked += m.MarkerClicked; ###############################################################################
                mc.MarkerId = m.MarkerId;
                mc.Tooltip = m.Tooltip;
                mc.Visible = m.Visible;
                mc.Draggable = m.Draggable;

                mlist.Add(mc);
            }

            return mlist;
        }
    }

}
