using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AYLogistics.CustomXML
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Awmds
    {

        private AwmdsGeneral_segment general_segmentField;

        private AwmdsBol_segment[] bol_segmentField;

        /// <remarks/>
        public AwmdsGeneral_segment General_segment
        {
            get
            {
                return this.general_segmentField;
            }
            set
            {
                this.general_segmentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Bol_segment")]
        public AwmdsBol_segment[] Bol_segment
        {
            get
            {
                return this.bol_segmentField;
            }
            set
            {
                this.bol_segmentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsGeneral_segment
    {

        private AwmdsGeneral_segmentGeneral_segment_id general_segment_idField;

        private AwmdsGeneral_segmentTotals_segment totals_segmentField;

        private AwmdsGeneral_segmentTransport_information transport_informationField;

        private AwmdsGeneral_segmentLoad_unload_place load_unload_placeField;

        private AwmdsGeneral_segmentTonnage tonnageField;

        /// <remarks/>
        public AwmdsGeneral_segmentGeneral_segment_id General_segment_id
        {
            get
            {
                return this.general_segment_idField;
            }
            set
            {
                this.general_segment_idField = value;
            }
        }

        /// <remarks/>
        public AwmdsGeneral_segmentTotals_segment Totals_segment
        {
            get
            {
                return this.totals_segmentField;
            }
            set
            {
                this.totals_segmentField = value;
            }
        }

        /// <remarks/>
        public AwmdsGeneral_segmentTransport_information Transport_information
        {
            get
            {
                return this.transport_informationField;
            }
            set
            {
                this.transport_informationField = value;
            }
        }

        /// <remarks/>
        public AwmdsGeneral_segmentLoad_unload_place Load_unload_place
        {
            get
            {
                return this.load_unload_placeField;
            }
            set
            {
                this.load_unload_placeField = value;
            }
        }

        /// <remarks/>
        public AwmdsGeneral_segmentTonnage Tonnage
        {
            get
            {
                return this.tonnageField;
            }
            set
            {
                this.tonnageField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsGeneral_segmentGeneral_segment_id
    {

        private string customs_office_codeField;

        private string voyage_numberField;

        private System.DateTime? date_of_departureField;

        private System.DateTime date_of_arrivalField;

        private string time_of_arrivalField;

        /// <remarks/>
        public string Customs_office_code
        {
            get
            {
                return this.customs_office_codeField;
            }
            set
            {
                this.customs_office_codeField = value;
            }
        }

        /// <remarks/>
        public string Voyage_number
        {
            get
            {
                return this.voyage_numberField;
            }
            set
            {
                this.voyage_numberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime? Date_of_departure
        {
            get
            {
                return this.date_of_departureField;
            }
            set
            {
                this.date_of_departureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Date_of_arrival
        {
            get
            {
                return this.date_of_arrivalField;
            }
            set
            {
                this.date_of_arrivalField = value;
            }
        }

        /// <remarks/>
        public string Time_of_arrival
        {
            get
            {
                return this.time_of_arrivalField;
            }
            set
            {
                this.time_of_arrivalField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsGeneral_segmentTotals_segment
    {

        private int total_number_of_bolsField;

        private int total_number_of_packagesField;

        private int total_number_of_containersField;

        private decimal total_gross_massField;

        /// <remarks/>
        public int Total_number_of_bols
        {
            get
            {
                return this.total_number_of_bolsField;
            }
            set
            {
                this.total_number_of_bolsField = value;
            }
        }

        /// <remarks/>
        public int Total_number_of_packages
        {
            get
            {
                return this.total_number_of_packagesField;
            }
            set
            {
                this.total_number_of_packagesField = value;
            }
        }

        /// <remarks/>
        public int Total_number_of_containers
        {
            get
            {
                return this.total_number_of_containersField;
            }
            set
            {
                this.total_number_of_containersField = value;
            }
        }

        /// <remarks/>
        public decimal Total_gross_mass
        {
            get
            {
                return this.total_gross_massField;
            }
            set
            {
                this.total_gross_massField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsGeneral_segmentTransport_information
    {

        private AwmdsGeneral_segmentTransport_informationCarrier carrierField;

        private AwmdsGeneral_segmentTransport_informationShipping_Agent shipping_AgentField;

        private int mode_of_transport_codeField;

        private string identity_of_transporterField;

        private string nationality_of_transporter_codeField;

        private string place_of_transporterField;

        private string master_informationField;

        /// <remarks/>
        public AwmdsGeneral_segmentTransport_informationCarrier Carrier
        {
            get
            {
                return this.carrierField;
            }
            set
            {
                this.carrierField = value;
            }
        }

        /// <remarks/>
        public AwmdsGeneral_segmentTransport_informationShipping_Agent Shipping_Agent
        {
            get
            {
                return this.shipping_AgentField;
            }
            set
            {
                this.shipping_AgentField = value;
            }
        }

        /// <remarks/>
        public int Mode_of_transport_code
        {
            get
            {
                return this.mode_of_transport_codeField;
            }
            set
            {
                this.mode_of_transport_codeField = value;
            }
        }

        /// <remarks/>
        public string Identity_of_transporter
        {
            get
            {
                return this.identity_of_transporterField;
            }
            set
            {
                this.identity_of_transporterField = value;
            }
        }

        /// <remarks/>
        public string Nationality_of_transporter_code
        {
            get
            {
                return this.nationality_of_transporter_codeField;
            }
            set
            {
                this.nationality_of_transporter_codeField = value;
            }
        }

        /// <remarks/>
        public string Place_of_transporter
        {
            get
            {
                return this.place_of_transporterField;
            }
            set
            {
                this.place_of_transporterField = value;
            }
        }

        /// <remarks/>
        public string Master_information
        {
            get
            {
                return this.master_informationField;
            }
            set
            {
                this.master_informationField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsGeneral_segmentTransport_informationCarrier
    {

        private string carrier_codeField;

        private string carrier_nameField;

        private string carrier_addressField;

        /// <remarks/>
        public string Carrier_code
        {
            get
            {
                return this.carrier_codeField;
            }
            set
            {
                this.carrier_codeField = value;
            }
        }

        /// <remarks/>
        public string Carrier_name
        {
            get
            {
                return this.carrier_nameField;
            }
            set
            {
                this.carrier_nameField = value;
            }
        }

        /// <remarks/>
        public string Carrier_address
        {
            get
            {
                return this.carrier_addressField;
            }
            set
            {
                this.carrier_addressField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsGeneral_segmentTransport_informationShipping_Agent
    {

        private string shipping_Agent_codeField;

        private string shipping_Agent_nameField;

        /// <remarks/>
        public string Shipping_Agent_code
        {
            get
            {
                return this.shipping_Agent_codeField;
            }
            set
            {
                this.shipping_Agent_codeField = value;
            }
        }

        /// <remarks/>
        public string Shipping_Agent_name
        {
            get
            {
                return this.shipping_Agent_nameField;
            }
            set
            {
                this.shipping_Agent_nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsGeneral_segmentLoad_unload_place
    {

        private string place_of_departure_codeField;

        private string place_of_destination_codeField;

        /// <remarks/>
        public string Place_of_departure_code
        {
            get
            {
                return this.place_of_departure_codeField;
            }
            set
            {
                this.place_of_departure_codeField = value;
            }
        }

        /// <remarks/>
        public string Place_of_destination_code
        {
            get
            {
                return this.place_of_destination_codeField;
            }
            set
            {
                this.place_of_destination_codeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsGeneral_segmentTonnage
    {

        private double tonnage_net_weightField;

        private double tonnage_gross_weightField;

        /// <remarks/>
        public double Tonnage_net_weight
        {
            get
            {
                return this.tonnage_net_weightField;
            }
            set
            {
                this.tonnage_net_weightField = value;
            }
        }

        /// <remarks/>
        public double Tonnage_gross_weight
        {
            get
            {
                return this.tonnage_gross_weightField;
            }
            set
            {
                this.tonnage_gross_weightField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsBol_segment
    {

        private AwmdsBol_segmentBol_id bol_idField;

        private AwmdsBol_segmentLoad_unload_place load_unload_placeField;

        private AwmdsBol_segmentTraders_segment traders_segmentField;

        private AwmdsBol_segmentCtn_segment[] ctn_segmentField;

        private AwmdsBol_segmentGoods_segment goods_segmentField;

        private AwmdsBol_segmentValue_segment value_segmentField;

        /// <remarks/>
        public AwmdsBol_segmentBol_id Bol_id
        {
            get
            {
                return this.bol_idField;
            }
            set
            {
                this.bol_idField = value;
            }
        }

        /// <remarks/>
        public AwmdsBol_segmentLoad_unload_place Load_unload_place
        {
            get
            {
                return this.load_unload_placeField;
            }
            set
            {
                this.load_unload_placeField = value;
            }
        }

        /// <remarks/>
        public AwmdsBol_segmentTraders_segment Traders_segment
        {
            get
            {
                return this.traders_segmentField;
            }
            set
            {
                this.traders_segmentField = value;
            }
        }

        /// <remarks/>
       [System.Xml.Serialization.XmlElementAttribute("ctn_segment")]
        public AwmdsBol_segmentCtn_segment[] ctn_segment
        {
            get
            {
                return this.ctn_segmentField;
            }
            set
            {
                this.ctn_segmentField = value;
            }
        }

        /// <remarks/>
        public AwmdsBol_segmentGoods_segment Goods_segment
        {
            get
            {
                return this.goods_segmentField;
            }
            set
            {
                this.goods_segmentField = value;
            }
        }

        /// <remarks/>
        public AwmdsBol_segmentValue_segment Value_segment
        {
            get
            {
                return this.value_segmentField;
            }
            set
            {
                this.value_segmentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsBol_segmentBol_id
    {

        private string bol_referenceField;

        private byte line_numberField;

        private byte bol_natureField;

        private string bol_type_codeField;

        private string master_bol_ref_numberField;

        /// <remarks/>
        public string Bol_reference
        {
            get
            {
                return this.bol_referenceField;
            }
            set
            {
                this.bol_referenceField = value;
            }
        }

        /// <remarks/>
        public byte Line_number
        {
            get
            {
                return this.line_numberField;
            }
            set
            {
                this.line_numberField = value;
            }
        }

        /// <remarks/>
        public byte Bol_nature
        {
            get
            {
                return this.bol_natureField;
            }
            set
            {
                this.bol_natureField = value;
            }
        }

        /// <remarks/>
        public string Bol_type_code
        {
            get
            {
                return this.bol_type_codeField;
            }
            set
            {
                this.bol_type_codeField = value;
            }
        }

        /// <remarks/>
        public string Master_bol_ref_number
        {
            get
            {
                return this.master_bol_ref_numberField;
            }
            set
            {
                this.master_bol_ref_numberField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsBol_segmentLoad_unload_place
    {

        private string place_of_loading_codeField;

        private string place_of_unloading_codeField;

        private string port_of_origin_codeField;

        private string original_port_of_loading_codeField;

        private string place_of_delivery_codeField;

        private string place_of_ultimate_destination_codeField;

        /// <remarks/>
        public string Place_of_loading_code
        {
            get
            {
                return this.place_of_loading_codeField;
            }
            set
            {
                this.place_of_loading_codeField = value;
            }
        }

        /// <remarks/>
        public string Place_of_unloading_code
        {
            get
            {
                return this.place_of_unloading_codeField;
            }
            set
            {
                this.place_of_unloading_codeField = value;
            }
        }

        /// <remarks/>
        public string Port_of_origin_code
        {
            get
            {
                return this.port_of_origin_codeField;
            }
            set
            {
                this.port_of_origin_codeField = value;
            }
        }

        /// <remarks/>
        public string Original_port_of_loading_code
        {
            get
            {
                return this.original_port_of_loading_codeField;
            }
            set
            {
                this.original_port_of_loading_codeField = value;
            }
        }

        /// <remarks/>
        public string Place_of_delivery_code
        {
            get
            {
                return this.place_of_delivery_codeField;
            }
            set
            {
                this.place_of_delivery_codeField = value;
            }
        }

        /// <remarks/>
        public string Place_of_ultimate_destination_code
        {
            get
            {
                return this.place_of_ultimate_destination_codeField;
            }
            set
            {
                this.place_of_ultimate_destination_codeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsBol_segmentTraders_segment
    {

        private AwmdsBol_segmentTraders_segmentCarrier carrierField;

        private AwmdsBol_segmentTraders_segmentExporter exporterField;

        private AwmdsBol_segmentTraders_segmentNotify notifyField;

        private AwmdsBol_segmentTraders_segmentConsignee consigneeField;

        /// <remarks/>
        public AwmdsBol_segmentTraders_segmentCarrier Carrier
        {
            get
            {
                return this.carrierField;
            }
            set
            {
                this.carrierField = value;
            }
        }

        /// <remarks/>
        public AwmdsBol_segmentTraders_segmentExporter Exporter
        {
            get
            {
                return this.exporterField;
            }
            set
            {
                this.exporterField = value;
            }
        }

        /// <remarks/>
        public AwmdsBol_segmentTraders_segmentNotify Notify
        {
            get
            {
                return this.notifyField;
            }
            set
            {
                this.notifyField = value;
            }
        }

        /// <remarks/>
        public AwmdsBol_segmentTraders_segmentConsignee Consignee
        {
            get
            {
                return this.consigneeField;
            }
            set
            {
                this.consigneeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsBol_segmentTraders_segmentCarrier
    {

        private string carrier_codeField;

        private string carrier_nameField;

        private string carrier_addressField;

        /// <remarks/>
        public string Carrier_code
        {
            get
            {
                return this.carrier_codeField;
            }
            set
            {
                this.carrier_codeField = value;
            }
        }

        /// <remarks/>
        public string Carrier_name
        {
            get
            {
                return this.carrier_nameField;
            }
            set
            {
                this.carrier_nameField = value;
            }
        }

        /// <remarks/>
        public string Carrier_address
        {
            get
            {
                return this.carrier_addressField;
            }
            set
            {
                this.carrier_addressField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsBol_segmentTraders_segmentExporter
    {

        private string exporter_nameField;

        private string exporter_addressField;

        /// <remarks/>
        public string Exporter_name
        {
            get
            {
                return this.exporter_nameField;
            }
            set
            {
                this.exporter_nameField = value;
            }
        }

        /// <remarks/>
        public string Exporter_address
        {
            get
            {
                return this.exporter_addressField;
            }
            set
            {
                this.exporter_addressField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsBol_segmentTraders_segmentNotify
    {

        private string notify_codeField;

        private string notify_nameField;

        private string notify_addressField;

        /// <remarks/>
        public string Notify_code
        {
            get
            {
                return this.notify_codeField;
            }
            set
            {
                this.notify_codeField = value;
            }
        }

        /// <remarks/>
        public string Notify_name
        {
            get
            {
                return this.notify_nameField;
            }
            set
            {
                this.notify_nameField = value;
            }
        }

        /// <remarks/>
        public string Notify_address
        {
            get
            {
                return this.notify_addressField;
            }
            set
            {
                this.notify_addressField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsBol_segmentTraders_segmentConsignee
    {

        private string consignee_nameField;

        private string consignee_addressField;

        /// <remarks/>
        public string Consignee_name
        {
            get
            {
                return this.consignee_nameField;
            }
            set
            {
                this.consignee_nameField = value;
            }
        }

        /// <remarks/>
        public string Consignee_address
        {
            get
            {
                return this.consignee_addressField;
            }
            set
            {
                this.consignee_addressField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsBol_segmentCtn_segment
    {

        private string ctn_referenceField;

        private int number_of_packagesField;

        private string type_of_containerField;

        private string empty_FullField;

        private string marks1Field;

        private string marks2Field;

        private string marks3Field;

        private string sealing_PartyField;

        /// <remarks/>
        public string Ctn_reference
        {
            get
            {
                return this.ctn_referenceField;
            }
            set
            {
                this.ctn_referenceField = value;
            }
        }

        /// <remarks/>
        public int Number_of_packages
        {
            get
            {
                return this.number_of_packagesField;
            }
            set
            {
                this.number_of_packagesField = value;
            }
        }

        /// <remarks/>
        public string Type_of_container
        {
            get
            {
                return this.type_of_containerField;
            }
            set
            {
                this.type_of_containerField = value;
            }
        }

        /// <remarks/>
        public string Empty_Full
        {
            get
            {
                return this.empty_FullField;
            }
            set
            {
                this.empty_FullField = value;
            }
        }

        /// <remarks/>
        public string Marks1
        {
            get
            {
                return this.marks1Field;
            }
            set
            {
                this.marks1Field = value;
            }
        }

        /// <remarks/>
        public string Marks2
        {
            get
            {
                return this.marks2Field;
            }
            set
            {
                this.marks2Field = value;
            }
        }

        /// <remarks/>
        public string Marks3
        {
            get
            {
                return this.marks3Field;
            }
            set
            {
                this.marks3Field = value;
            }
        }

        /// <remarks/>
        public string Sealing_Party
        {
            get
            {
                return this.sealing_PartyField;
            }
            set
            {
                this.sealing_PartyField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsBol_segmentGoods_segment
    {

        private int number_of_packagesField;

        private string package_type_codeField;

        private string package_typeField;

        private decimal gross_massField;

        private string shipping_marksField;

        private string goods_descriptionField;

        private decimal volume_in_cubic_metersField;

        private int num_of_ctn_for_this_bolField;

        private string informationField;

        /// <remarks/>
        public int Number_of_packages
        {
            get
            {
                return this.number_of_packagesField;
            }
            set
            {
                this.number_of_packagesField = value;
            }
        }

        /// <remarks/>
        public string Package_type_code
        {
            get
            {
                return this.package_type_codeField;
            }
            set
            {
                this.package_type_codeField = value;
            }
        }

        /// <remarks/>
        public string Package_type
        {
            get
            {
                return this.package_typeField;
            }
            set
            {
                this.package_typeField = value;
            }
        }

        /// <remarks/>
        public decimal Gross_mass
        {
            get
            {
                return this.gross_massField;
            }
            set
            {
                this.gross_massField = value;
            }
        }

        /// <remarks/>
        public string Shipping_marks
        {
            get
            {
                return this.shipping_marksField;
            }
            set
            {
                this.shipping_marksField = value;
            }
        }

        /// <remarks/>
        public string Goods_description
        {
            get
            {
                return this.goods_descriptionField;
            }
            set
            {
                this.goods_descriptionField = value;
            }
        }

        /// <remarks/>
        public decimal Volume_in_cubic_meters
        {
            get
            {
                return this.volume_in_cubic_metersField;
            }
            set
            {
                this.volume_in_cubic_metersField = value;
            }
        }

        /// <remarks/>
        public int Num_of_ctn_for_this_bol
        {
            get
            {
                return this.num_of_ctn_for_this_bolField;
            }
            set
            {
                this.num_of_ctn_for_this_bolField = value;
            }
        }

        /// <remarks/>
        public string Information
        {
            get
            {
                return this.informationField;
            }
            set
            {
                this.informationField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsBol_segmentValue_segment
    {

        private AwmdsBol_segmentValue_segmentFreight_segment freight_segmentField;

        /// <remarks/>
        public AwmdsBol_segmentValue_segmentFreight_segment Freight_segment
        {
            get
            {
                return this.freight_segmentField;
            }
            set
            {
                this.freight_segmentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AwmdsBol_segmentValue_segmentFreight_segment
    {

        private string pC_indicatorField;

        private byte freight_valueField;

        private string freight_currencyField;

        /// <remarks/>
        public string PC_indicator
        {
            get
            {
                return this.pC_indicatorField;
            }
            set
            {
                this.pC_indicatorField = value;
            }
        }

        /// <remarks/>
        public byte Freight_value
        {
            get
            {
                return this.freight_valueField;
            }
            set
            {
                this.freight_valueField = value;
            }
        }

        /// <remarks/>
        public string Freight_currency
        {
            get
            {
                return this.freight_currencyField;
            }
            set
            {
                this.freight_currencyField = value;
            }
        }
    }


}