using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel; 

namespace NetGain.GoogleMap
{
    public class Address
    {
        public Address(string _streetaddress, string _address1,
            string _address2, string _address3,
            string _city, string _state,
            string _country, string _zipcode, bool _isprimary,
            string _agency, string _RFPNumber)
        {
            StreetAddress = _streetaddress;
            Address1 = _address1;
            Address2 = _address2;
            Address3 = _address3;
            City = _city;
            State = _state;
            Country = _country;
            Zipcode = _zipcode;
            IsAddressPrimary = _isprimary;
            BuyerFriendlyName = _agency;
            BuyerContractNumber = _RFPNumber;
            BuyerContractName = _agency;
        }

        public int AddressID { get; set;}    
        public string StreetAddress { get; set;}
        public string Address1 { get; set;}
        public string Address2 { get; set;}
        public string Address3 { get; set;}
        public string City { get; set; }
        public int StateID { get; set; }
        public string State { get; set; }
        public int CountryID { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        //Tells whether an address is Primary or Not
        [DefaultValue(false)]
        public bool IsAddressPrimary { get; set; }
        public double DistanceFromPrimaryAddress { get; set; }

        //additional buyer and rfp information needed for the pin displays
        public int BuyerID { get; set;}    
        public int BuyerTypeID { get; set;}    
        public string BuyerFriendlyName { get; set;}    
        public int RFPID { get; set;}    
        public string BuyerContractName { get; set;}    
        public string BuyerContractNumber { get; set;}    
    }
}
