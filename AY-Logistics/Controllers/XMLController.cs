using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using AYLogistics.Models;

namespace AYLogistics.Controllers
{
    public class XMLController : Controller
    {
        //
        // GET: /XML/

        public ActionResult DragAndDropUpload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (HttpPostedFileBase file in files)
            {
                string filePath = Path.Combine(Server.MapPath("~/App_Data/UploadXMLFiles"), DateTime.Now.ToString("yyyyMMddhhmm") + file.FileName);
                /***write file****/
                System.IO.File.WriteAllBytes(filePath, ReadData(file.InputStream));

                /**DeSerialize To XML**/
                string XmlString = System.IO.File.ReadAllText(filePath);
                CustomXML.Awmds ManifestXML = new CustomXML.Awmds();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(XmlString);
                //doc.Load(file.InputStream);
                XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
                XmlSerializer ser = new XmlSerializer(ManifestXML.GetType());
                object obj = ser.Deserialize(reader);
                // cast obj
                CustomXML.Awmds myObj = (CustomXML.Awmds)obj;

                //Send XML file Data to Manifest Model
                ManifestModel XMLdata = new ManifestModel();
                int code = MySettings.GetCustomOfficeId(myObj.General_segment.General_segment_id.Customs_office_code);
                XMLdata.CustomOfficeId = code;
                XMLdata.VoyageNo = myObj.General_segment.General_segment_id.Voyage_number;
                XMLdata.DateOfDeparture = myObj.General_segment.General_segment_id.Date_of_departure;
                XMLdata.DateOfArrival = myObj.General_segment.General_segment_id.Date_of_arrival;
                if (myObj.General_segment.Transport_information.Mode_of_transport_code == 4)
                {
                    XMLdata.ModeofShipment = 2; //Id of Asia Forwarding
                }
                else if (myObj.General_segment.Transport_information.Mode_of_transport_code == 5)
                {
                    XMLdata.ModeofShipment = 3; //Id of Asia Forwarding
                }
                else
                {
                    XMLdata.ModeofShipment = myObj.General_segment.Transport_information.Mode_of_transport_code;
                }
                XMLdata.NetTonnage = myObj.General_segment.Tonnage.Tonnage_net_weight;
                XMLdata.GrossTonnage = myObj.General_segment.Tonnage.Tonnage_gross_weight;
                XMLdata.MasterBLno = XMLdata.GetXMLMasterBL(myObj.Bol_segment); // check All HouseBL and get MasterBL from it
                //Validation
                bool option1 = true;
                bool option2 = true;
                if (XMLdata.MasterBLno != "")
                {
                    Dictionary<object, object> MBLexist = ManifestModel.GetMasterBL(XMLdata.MasterBLno);
                    if (MBLexist.Count > 0)
                    {
                        option1 = false;
                        return Json("<font color='red'>Failed! Master BL No: " + XMLdata.MasterBLno + " is already entered</font>");
                    }
                }
                string HBLexist = XMLdata.CheckExistingHBL(myObj.Bol_segment);
                if (HBLexist != "")
                {
                    option2 = false;
                    return Json("<font color='red'>Failed! House BL No: " + HBLexist + " is already entered</font>");
                }
                //Save Manifest
                if (option1 == true && option2 == true)
                {
                    if (XMLdata.SaveXMLasManifest())
                    {
                        int LastShipmentId = ManifestModel.GetLastShipmentId();
                        //Send BL data of XML file to Save Function
                        XMLdata.SaveXMLasHouseBLItems(myObj.Bol_segment, LastShipmentId);
                        XMLdata.UpdateSequence();
                    }
                }
            }

            int ShipmentId = ManifestModel.GetLastShipmentId();
            Dictionary<object, object> SavedManifestinfo = ManifestModel.getLastManifestInfo(ShipmentId);
            return Json("<a href=/Manifest/EditManifest?ManifestId=" + SavedManifestinfo["LastSid"].ToString() + ">" + SavedManifestinfo["LastSnumber"].ToString() + "</a>");
        }
 
        private byte[] ReadData(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }

        public ActionResult SerializeManifestToXML( int ManifestId)
        {
            CustomXML.Awmds ManifestXML = new CustomXML.Awmds();
            CustomXML.AwmdsGeneral_segment GeneralSeg = new CustomXML.AwmdsGeneral_segment();

        //General_segment
            Dictionary<object, object> ManifestData = ManifestModel.GetManifestData(ManifestId); // GET Menifest Data
            List<Dictionary<object, object>> BLDataList = HouseBLModel.GetHouseBLItemsData(Convert.ToInt32(ManifestData["ShipmentId"])); // GET BLs
            //Validation
            if (ManifestData["DateDeparture"].Equals("")) 
            {
                ViewBag.type = "alert-error";
                return View("_error").Error("Departure Date is Empty!, XML not Generated");
            }
            if (ManifestData["DateArrival"].Equals(""))
            {
                ViewBag.type = "alert-error";
                return View("_error").Error("Arrival Date is Empty!, XML not Generated");
            }
            if (ManifestData["TotalContainer"].Equals("0"))
            {
                ViewBag.type = "alert-error";
                return View("_error").Error("BOL Container Info Not Added!, XML not Generated");
            }
            #region General_segment_id
            CustomXML.AwmdsGeneral_segmentGeneral_segment_id GeneralSegId = new CustomXML.AwmdsGeneral_segmentGeneral_segment_id();
            GeneralSegId.Customs_office_code = ManifestData["CustomCode"].ToString();
            GeneralSegId.Voyage_number = ManifestData["VoyageNo"].ToString();
            GeneralSegId.Date_of_departure = Convert.ToDateTime(ManifestData["DateDeparture"]);
            GeneralSegId.Date_of_arrival = Convert.ToDateTime(ManifestData["DateArrival"]);
            GeneralSegId.Time_of_arrival = ManifestData["TimeArrival"].ToString();
            GeneralSeg.General_segment_id = GeneralSegId; // Add to General Segment
            #endregion
            #region Totals_segment
            CustomXML.AwmdsGeneral_segmentTotals_segment TotalSeg = new CustomXML.AwmdsGeneral_segmentTotals_segment();
            TotalSeg.Total_number_of_bols = Convert.ToInt32(ManifestData["NumberOfBL"]);
            TotalSeg.Total_number_of_packages = ManifestData["TotalPackage"] == DBNull.Value ? 0 : Convert.ToInt32(ManifestData["TotalPackage"]);
            TotalSeg.Total_number_of_containers = ManifestData["TotalContainer"] == DBNull.Value ? 0 : Convert.ToInt32(ManifestData["TotalContainer"]);
            TotalSeg.Total_gross_mass = ManifestData["TotalWeight"] == DBNull.Value ? 0 : Convert.ToDecimal(ManifestData["TotalWeight"]);
            GeneralSeg.Totals_segment = TotalSeg; // Add to General Segment
            #endregion
            #region Transport_information
            CustomXML.AwmdsGeneral_segmentTransport_information TransportInfo = new CustomXML.AwmdsGeneral_segmentTransport_information();
            TransportInfo.Mode_of_transport_code = Convert.ToInt32(ManifestData["ModeOfTransportCode"]);
            TransportInfo.Identity_of_transporter = "";
            TransportInfo.Nationality_of_transporter_code = ManifestData["CarrierNation"].ToString();
            TransportInfo.Place_of_transporter = "";
            TransportInfo.Master_information = "";
            GeneralSeg.Transport_information = TransportInfo; // Add to General Segment
            CustomXML.AwmdsGeneral_segmentTransport_informationCarrier Carrier = new CustomXML.AwmdsGeneral_segmentTransport_informationCarrier();
            Carrier.Carrier_code = "";
            Carrier.Carrier_name = ManifestData["CarrierName"].ToString();
            Carrier.Carrier_address = "";
            GeneralSeg.Transport_information.Carrier = Carrier; // Add to General Segment
            CustomXML.AwmdsGeneral_segmentTransport_informationShipping_Agent Shipping_Agent = new CustomXML.AwmdsGeneral_segmentTransport_informationShipping_Agent();
            Shipping_Agent.Shipping_Agent_code = "";
            Shipping_Agent.Shipping_Agent_name = ManifestData["ShippingAgentName"].ToString();
            GeneralSeg.Transport_information.Shipping_Agent = Shipping_Agent; // Add to General Segment
            #endregion
            #region Load_unload_place
            CustomXML.AwmdsGeneral_segmentLoad_unload_place LoadUnload = new CustomXML.AwmdsGeneral_segmentLoad_unload_place();
            LoadUnload.Place_of_departure_code = ManifestData["PortDeparture"].ToString();
            LoadUnload.Place_of_destination_code = ManifestData["PortDestination"].ToString();
            GeneralSeg.Load_unload_place = LoadUnload; // Add to General Segment
            #endregion
            #region Tonnage
            CustomXML.AwmdsGeneral_segmentTonnage Tonnage = new CustomXML.AwmdsGeneral_segmentTonnage();
            Tonnage.Tonnage_net_weight = ManifestData["NETtonnage"] == DBNull.Value ? 0 : Convert.ToDouble(ManifestData["NETtonnage"]);
            Tonnage.Tonnage_gross_weight = ManifestData["GROSStonnage"] == DBNull.Value ? 0 : Convert.ToDouble(ManifestData["GROSStonnage"]);
            GeneralSeg.Tonnage = Tonnage; // Add to General Segment
            #endregion
            ManifestXML.General_segment = GeneralSeg;

      //Bol_segment
            int NOofBol = ManifestXML.General_segment.Totals_segment.Total_number_of_bols;
            byte LineCount = 0;
            CustomXML.AwmdsBol_segment[] BolSegList = new CustomXML.AwmdsBol_segment[NOofBol];
            

            for (int i = 0; i < NOofBol; i++)
            {
                Dictionary<object, object> BLData = BLDataList[i];
                CustomXML.AwmdsBol_segment BolSeg = new CustomXML.AwmdsBol_segment();
                LineCount++;
                CustomXML.AwmdsBol_segmentBol_id BolId = new CustomXML.AwmdsBol_segmentBol_id();
                BolId.Bol_reference = BLData["HouseBL"].ToString();
                BolId.Line_number = LineCount;
                BolId.Bol_nature = Convert.ToByte(BLData["BLNature"]);
                BolId.Bol_type_code = BLData["BLType"].ToString();
                BolId.Master_bol_ref_number = BLData["MasterBL"].ToString();
                BolSeg.Bol_id = BolId; // Add to Bol Segment

                CustomXML.AwmdsBol_segmentLoad_unload_place BolLoadUnload = new CustomXML.AwmdsBol_segmentLoad_unload_place();
                BolLoadUnload.Place_of_loading_code = BLData["PortOfLoading"].ToString();
                BolLoadUnload.Place_of_unloading_code = BLData["PortOfUnloading"].ToString();
                BolLoadUnload.Port_of_origin_code = BLData["PortOfOrigin"].ToString();
                BolLoadUnload.Original_port_of_loading_code = BLData["OriginalLoadingPort"].ToString();
                BolLoadUnload.Place_of_delivery_code = BLData["PortOfDelivery"].ToString();
                BolLoadUnload.Place_of_ultimate_destination_code = BLData["UltimateDestination"].ToString();
                BolSeg.Load_unload_place = BolLoadUnload;

                CustomXML.AwmdsBol_segmentTraders_segment TradeSeg = new CustomXML.AwmdsBol_segmentTraders_segment();
                CustomXML.AwmdsBol_segmentTraders_segmentCarrier TScarrier = new CustomXML.AwmdsBol_segmentTraders_segmentCarrier();
                TScarrier.Carrier_code = "";
                TScarrier.Carrier_name = BLData["Carrier"].ToString();
                TScarrier.Carrier_address = "";
                CustomXML.AwmdsBol_segmentTraders_segmentExporter TSexporter = new CustomXML.AwmdsBol_segmentTraders_segmentExporter();
                TSexporter.Exporter_name = BLData["ShipperName"].ToString();
                TSexporter.Exporter_address = BLData["ShipperAddress"].ToString();
                CustomXML.AwmdsBol_segmentTraders_segmentNotify TSnotify = new CustomXML.AwmdsBol_segmentTraders_segmentNotify();
                TSnotify.Notify_code = "";
                TSnotify.Notify_name = BLData["NotifyPartyName"].ToString();
                TSnotify.Notify_address = BLData["NotifyPartyAddress"].ToString();
                CustomXML.AwmdsBol_segmentTraders_segmentConsignee TSconsignee = new CustomXML.AwmdsBol_segmentTraders_segmentConsignee();
                TSconsignee.Consignee_name = BLData["ConsigneeName"].ToString();
                TSconsignee.Consignee_address = BLData["ConsigneeAddress"].ToString();
                TradeSeg.Carrier = TScarrier;       // Add to Trade Segment
                TradeSeg.Exporter = TSexporter;     // Add to Trade Segment
                TradeSeg.Notify = TSnotify;         // Add to Trade Segment
                TradeSeg.Consignee = TSconsignee;   // Add to Trade Segment
                BolSeg.Traders_segment = TradeSeg;  // Add to Bol Segment
//All Containers
                List<Dictionary<object, object>> BLContainerList = HouseBLModel.GetContainerData(Convert.ToInt32(BLData["Id"])); // Get BL Containers
                int NoofContainer = BLContainerList.Count;
                CustomXML.AwmdsBol_segmentCtn_segment[] ContainerSegList = new CustomXML.AwmdsBol_segmentCtn_segment[NoofContainer];

                int x = 0;
                while (x < NoofContainer)
                {
                    Dictionary<object, object> ContainerData = BLContainerList[x];
                    CustomXML.AwmdsBol_segmentCtn_segment ContainerSeg = new CustomXML.AwmdsBol_segmentCtn_segment();
                    ContainerSeg.Ctn_reference = ContainerData["ContainerNo"].ToString();
                    ContainerSeg.Number_of_packages = ContainerData["CNoOfPackage"] == DBNull.Value ? 0 : Convert.ToInt32(ContainerData["CNoOfPackage"]);
                    ContainerSeg.Type_of_container = ContainerData["ContainerType"].ToString();
                    ContainerSeg.Empty_Full = ContainerData["IndicatorName"].ToString();
                    ContainerSeg.Marks1 = ContainerData["SealNo"].ToString();
                    ContainerSeg.Marks2 = "";
                    ContainerSeg.Marks3 = "";
                    ContainerSeg.Sealing_Party = "";
                    ContainerSegList[x] = ContainerSeg; //Add to List
                    x++;
                }
                BolSeg.ctn_segment = ContainerSegList; // Add to Bol Segment
                
//Total of All COntainers
                Dictionary<object, object> PakageData1 = ContainerModel.GetContainerData1(Convert.ToInt32(BLData["Id"])); // this fuction can extend if Goods Segment is multiple
                Dictionary<object, object> PakageData2 = ContainerModel.GetContainerData2(Convert.ToInt32(BLData["Id"])); // this fuction can extend if Goods Segment is multiple
                CustomXML.AwmdsBol_segmentGoods_segment GoodsSeg = new CustomXML.AwmdsBol_segmentGoods_segment();
                GoodsSeg.Number_of_packages = TypeofPackage.CountBLPackages(Convert.ToInt32(BLData["Id"]));
                GoodsSeg.Package_type_code = PakageData1["PKTypeCode"].ToString();
                GoodsSeg.Package_type = PakageData1["PKType"].ToString();
                GoodsSeg.Gross_mass = PakageData2["Weight"] == DBNull.Value ? 0 : Convert.ToDecimal(PakageData2["Weight"]);
                GoodsSeg.Shipping_marks = PakageData2["ShippingMark"].ToString();
                GoodsSeg.Goods_description = PakageData2["Description"].ToString();
                GoodsSeg.Volume_in_cubic_meters = PakageData2["Measurement"] == DBNull.Value ? 0 : Convert.ToDecimal(PakageData2["Measurement"]);
                GoodsSeg.Num_of_ctn_for_this_bol = NoofContainer;
                GoodsSeg.Information = "";
                BolSeg.Goods_segment = GoodsSeg;

                CustomXML.AwmdsBol_segmentValue_segment ValueSeg = new CustomXML.AwmdsBol_segmentValue_segment();
                CustomXML.AwmdsBol_segmentValue_segmentFreight_segment FreightSeg = new CustomXML.AwmdsBol_segmentValue_segmentFreight_segment();
                FreightSeg.PC_indicator = PakageData2["FICOde"].ToString();
                FreightSeg.Freight_value = 00;
                FreightSeg.Freight_currency = "";
                ValueSeg.Freight_segment = FreightSeg; // Add to Value Segment
                BolSeg.Value_segment = ValueSeg; // Add to Bol Segment

                BolSegList[i] = BolSeg; // Add to Bol Segment to List
            }
            ManifestXML.Bol_segment = BolSegList;

            XmlSerializer ser = new XmlSerializer(ManifestXML.GetType());
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            // System.IO.StringWriter writer = new System.IO.StringWriter(sb);
            StringWriterWithEncoding writer = new StringWriterWithEncoding(sb, Encoding.UTF8);

            /** Here Classes are converted to XML String.*/
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(); // removing name space
            ns.Add("", ""); // removing name space
            
            ser.Serialize(writer, ManifestXML,ns);
            /* This can be viewed in SB or writer.*/

            /* Above XML in SB can be loaded in XmlDocument object*/
            // XmlDocument doc = new XmlDocument();
            // doc.LoadXml(sb.ToString());

            /* Return and display XML file*/
            // return this.Content(sb.ToString(), "text/xml");

            //Replacing Name Space and Encoding
           /* sb.Replace("xmlns:xsd", "");
            sb.Replace("http://www.w3.org/2001/XMLSchema", "");
            sb.Replace("xmlns:xsi", "");
            sb.Replace("http://www.w3.org/2001/XMLSchema-instance", "");*/
            sb.Replace(" encoding=\"utf-8\"", "");

            string fileName = ManifestData["ManifestNumber"].ToString() + ".xml"; // To DO
            ManifestModel MF = new ManifestModel();
           // MF.SaveShipmentStatus(Convert.ToInt32(ManifestData["ShipmentId"]), 2);
            MF.UpdateShipmentStatusPay(Convert.ToInt32(ManifestData["ShipmentId"]), 2);

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "application/xml", fileName);
        }

        /***StringWriter Extended with Encoding for XML decleration***/
        public class StringWriterWithEncoding : StringWriter
        {
            public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
                : base(sb)
            {
                this.m_Encoding = encoding;
            }
            private readonly Encoding m_Encoding;
            public override Encoding Encoding
            {
                get
                {
                    return this.m_Encoding;
                }
            }
        }

    }
}
