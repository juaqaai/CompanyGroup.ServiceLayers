using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubDistribution.Models
{
    /// <summary>
    /// -<product> 
    /// <itemid>41018718</itemid> 
    /// <name>SAMSUNG E1190, TITAN GREY</name> 
    /// <stock>3 db</stock>
    /// <netprice>19</netprice> 
    /// <grossprice>24</grossprice>
    /// -<attributes> 
    ///     -<mainattribute name="Általános adatok"> 
    ///         <subattribute name="Hálózat">GSM 900 / 1800 </subattribute> 
    ///     </mainattribute>
    ///     -<mainattribute name="Méret adatok"> 
    ///         <subattribute name="Méret (mm)">88 x 44 x 18.9 </subattribute> 
    ///         <subattribute name="Súly(g)">71 </subattribute> 
    ///     </mainattribute>
    /// </attributes> 
    /// -<package> 
    ///     <pitem name="Akkumulátor">AB463446BU</pitem> 
    ///     <pitem name="Hálózati töltő">ATADS10EBE</pitem> 
    ///     <pitem name="Telefon doboz ma*szé*mé (cm)">5,3*7,5*12</pitem> 
    ///     <pitem name="db/karton">20</pitem> 
    ///     <pitem name="Karton ma*szé*mé (cm)">14,5*23*39</pitem> 
    ///     <pitem name="Menü nyelvezet">english, magyar, cesky, slovencina</pitem> 
    /// </package>
    /// -<pictures> 
    ///     <picture>http://www.mobilmania.hu/products/41018718.jpg</picture> 
    /// </pictures> 
    /// </product>
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class Product
    {
        [System.Runtime.Serialization.DataMember(Name = "itemid", Order = 1)]
        public string ItemId { get; set;}

        [System.Runtime.Serialization.DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "stock", Order = 3)]
        public string Stock { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "netprice", Order = 4)]
        public string NetPrice { get; set; }


        [System.Runtime.Serialization.DataMember(Name = "grossprice", Order = 5)]
        public string GrossPrice { get; set; }
    }


}