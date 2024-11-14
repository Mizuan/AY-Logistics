using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Aspose.Words.Tables;
using System.Text.RegularExpressions;
using System.Collections;
using Aspose.Words.Drawing;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using System.Drawing;
using DotNet.Highcharts.Helpers;
using Aspose.Words;
using AYLogistics.Models;

namespace MyReports
{
    public class Reports
    {

        public static MemoryStream GenerateReport(Dictionary<object, object> dictionary, string docfilename)
        {
            Document doc = new Document(docfilename);
            DocumentBuilder db = new DocumentBuilder(doc);

            foreach (string key in dictionary.Keys)
            {
                 if (db.MoveToBookmark(key))
                    {
                        string s = dictionary[key].ToString();
                        db.InsertHtml(s);
                    }
            }
            RemoveAspose(doc);
            MemoryStream ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Pdf);
            ms.Position = 0;
            return ms;
        }

        public static MemoryStream Generate_Report(Dictionary<object, object> dictionary, string docfilename, bool isDraft=false)
        {
            Document doc = new Document(docfilename);
            DocumentBuilder db = new DocumentBuilder(doc);

            int mode=0;
            foreach (string key in dictionary.Keys)
            {
                if (dictionary[key] !=null)
                {
                    string s = dictionary[key].ToString();
                    switch (key)
                    {
                        case "FormNo":
                            mode = 2;
                            break;
                        default:
                            mode = 0;
                            break;
                    }
                   // doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s, mode), false); this is for DIrections
                    doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                }
            }

            doc.Range.Replace(new Regex("@?<.+?>"), new mvReplaceEvaluator(""), false);
            RemoveAspose(doc);
            MemoryStream ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Pdf);
            ms.Position = 0;
            return ms;
        }

        public static MemoryStream GenerateMultipleReport(List<Dictionary<object, object>> dictionarylist, string docfilename)
        {
            Document doc = new Document(docfilename);
            doc.RemoveAllChildren();

            foreach (Dictionary<object, object> dictionary in dictionarylist)
            {
                Document srcDoc = new Document(docfilename);
                RemoveAspose(srcDoc);

                DocumentBuilder db = new DocumentBuilder(srcDoc);

                foreach (string key in dictionary.Keys)
                {
                    if (dictionary[key] != null)
                    {
                        string s = dictionary[key].ToString();
                        srcDoc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                    }
                }
                doc.AppendDocument(srcDoc, ImportFormatMode.KeepSourceFormatting);
                doc.Range.Replace(new Regex("@?<.+?>"), new mvReplaceEvaluator(""), false); /*~ Remove nullable fields from doc*/
            }
            RemoveAspose(doc);
            MemoryStream ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Pdf);
            ms.Position = 0;
            return ms;
        }

        public MemoryStream Generate_Manifest(List<Dictionary<object, object>> BLList, Dictionary<object, object> ManifestDT, string docfilename)
        {
            Document doc = new Document(docfilename);
            DocumentBuilder db = new DocumentBuilder(doc);
            db.MoveToBookmark("table");

            TableContent table = new TableContent();
            table.tHeader = new Dictionary<int, TableCell>();
            table.RowCount = new List<int>();

            int i = 0;
            table.tHeader.Add(i++, new TableCell() { Value = "Master BL/AWB", direction = "", font = "Faruma", Width = 11 });
            table.tHeader.Add(i++, new TableCell() { Value = "House BL/AWB", direction = "", font = "Faruma", Width = 11 });
            table.tHeader.Add(i++, new TableCell() { Value = "Shipper,Consignee,Notify Party", direction = "", font = "Faruma", Width = 23 });
            table.tHeader.Add(i++, new TableCell() { Value = "Container & seal No.", direction = "", font = "Faruma", Width = 15 });
            table.tHeader.Add(i++, new TableCell() { Value = "No.of Pkgs & Type", direction = "", font = "Faruma", Width = 11 });
            table.tHeader.Add(i++, new TableCell() { Value = "Description of Goods", direction = "", font = "Faruma", Width = 17 });
            table.tHeader.Add(i++, new TableCell() { Value = "Weight (KGS)", direction = "", font = "Faruma", Width = 8 });
            table.tHeader.Add(i++, new TableCell() { Value = "CBM", direction = "", font = "Faruma", Width = 4 });

            table.tBody = new List<Dictionary<int, TableCell>>();
            TableCell tcell = new TableCell();

            int RowCount = 0;
            i = 0;
            Dictionary<int, TableCell> table_row;
            Dictionary<int, TableCell> table_lastrow;

            foreach (Dictionary<object, object> dictionary in BLList)
            {
                table_row = new Dictionary<int, TableCell>();
                table_lastrow = new Dictionary<int, TableCell>();
                foreach (string key in dictionary.Keys)
                {
                    string s = dictionary[key].ToString();
                    if (key.Equals("MaterBLandPorts") || key.Equals("HouseBLandPorts") || key.Equals("Parties") || key.Equals("ContainerInfo") || key.Equals("TypeOfPackageList") || key.Equals("Description") || key.Equals("Weight") || key.Equals("Measurement"))
                    {
                        table_row.Add(i++, new TableCell()
                        {
                            Value = dictionary[key].ToString(),
                            direction = "",
                            font = "Calibri",
                            //  Width = 20
                        });
                    }
                    //Collect data to Last row
                    if (key.Equals("UltimateDestinationName") || key.Equals("PortOfDeliveryName") || key.Equals("BLnature") || key.Equals("Mark") || key.Equals("FreightIndicatorName") || key.Equals("FreightIndicatorNameTotal") || key.Equals("Weight") || key.Equals("Measurement"))
                    {
                        string val = dictionary[key].ToString();
                        if (key.Equals("FreightIndicatorName"))
                        {
                            val="";
                        }
                        table_lastrow.Add(i++, new TableCell()
                        {
                            Value = val,
                            direction = "",
                            font = "Calibri",
                            //  Width = 20
                        });
                    }


                }

                RowCount++;
                table.RowCount.Add(RowCount);
                table.tBody.Add(table_row);
                /**Add row belwo Table*/
                RowCount++;
                table.RowCount.Add(RowCount);
                table.tBody.Add(table_lastrow);
            }

            db.ParagraphFormat.Alignment = ParagraphAlignment.Right;
           // db = table.DrawTable(db, false, true);
            db = table.DrawTable(db,false,true,false);

            foreach (string key in ManifestDT.Keys)
            {
                string s = ManifestDT[key].ToString();
                Regex alifbaa = new Regex(@"[ހ-ޤ]+");
                if (alifbaa.IsMatch(s))
                {
                    doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                }
                else
                {
                    doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                }
            }

            Break_Accross_Page(doc);
            doc.Range.Replace(new Regex("@?<.+?>"), new mvReplaceEvaluator(""), false);
            RemoveAspose(doc);
            MemoryStream ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Pdf);
            ms.Position = 0;
            return ms;
        }

        public MemoryStream Generate_Manifest_Dispatch(List<Dictionary<object, object>> BLList, Dictionary<object, object> ManifestDT, string NoOfHBL, string docfilename)
        {
            string previousMBL = "";
            int MBLCount = 0;
            Document doc = new Document(docfilename);
            DocumentBuilder db = new DocumentBuilder(doc);
            db.MoveToBookmark("table");

            TableContent table = new TableContent();
            table.tHeader = new Dictionary<int, TableCell>();
            table.RowCount = new List<int>();

            int i = 0;
            table.tHeader.Add(i++, new TableCell() { Value = "Master BL Number", direction = "", font = "Faruma", Width = 11 });
            table.tHeader.Add(i++, new TableCell() { Value = "House BL Number", direction = "", font = "Faruma", Width = 11 });
            table.tHeader.Add(i++, new TableCell() { Value = "Consignee", direction = "", font = "Faruma", Width = 25 });
            table.tHeader.Add(i++, new TableCell() { Value = "Container Number.", direction = "", font = "Faruma", Width = 21 });
            table.tHeader.Add(i++, new TableCell() { Value = "Name", direction = "", font = "Faruma", Width = 12 });
            table.tHeader.Add(i++, new TableCell() { Value = "Sign", direction = "", font = "Faruma", Width = 8 });
            table.tHeader.Add(i++, new TableCell() { Value = "Date", direction = "", font = "Faruma", Width = 8 });

            table.tBody = new List<Dictionary<int, TableCell>>();
            TableCell tcell = new TableCell();

            int RowCount = 0;
            i = 0;
            Dictionary<int, TableCell> table_row;
            Dictionary<int, TableCell> table_lastrow;

            foreach (Dictionary<object, object> dictionary in BLList)
            {
                table_row = new Dictionary<int, TableCell>();
                foreach (string key in dictionary.Keys)
                {
                    string s = dictionary[key].ToString();
                    if (key.Equals("MBL") || key.Equals("HBL") || key.Equals("CustomerName") || key.Equals("ContainerNoList") || key.Equals("WitnessName") || key.Equals("WitnessSign") || key.Equals("WitnessDate"))
                    {
                        string val = dictionary[key].ToString();
                        string currentMBL = val;
                        if (key.Equals("MBL"))
                        {
                            if (previousMBL == currentMBL)
                            {
                                val = "";
                            }
                            else if(currentMBL !="-")
                            {
                                MBLCount = MBLCount + 1;
                            }
                            previousMBL = currentMBL;
                        }
                        table_row.Add(i++, new TableCell()
                        {
                            Value = val,
                            direction = "",
                            font = "Calibri",
                            //  Width = 20
                        });
                    }
                }

                RowCount++;
                table.RowCount.Add(RowCount);
                table.tBody.Add(table_row);
            }

            //Collect data to Last row
            table_lastrow = new Dictionary<int, TableCell>();
                table_lastrow.Add(i++, new TableCell()
                {
                    Value = "<p style='background-color: #ccc9c8; margin:0;padding:0;'><strong>TOTAL MBL: </strong>" + MBLCount + "</p>",
                    direction = "",
                    font = "Calibri",
                    //  Width = 20
                });
                table_lastrow.Add(i++, new TableCell()
                {
                    Value = "<p style='background-color: #ccc9c8; margin:0;padding:0;'><strong>TOTAL HBL: </strong>" + NoOfHBL + "</p>",
                    direction = "",
                    font = "Calibri",
                    //  Width = 20
                });

            /**Add row belwo Table*/
            RowCount++;
            table.RowCount.Add(RowCount);
            table.tBody.Add(table_lastrow);

            db.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            // db = table.DrawTable(db, false, true);
            db = table.DrawTable(db, false, true, false);

            foreach (string key in ManifestDT.Keys)
            {
                string s = ManifestDT[key].ToString();
                Regex alifbaa = new Regex(@"[ހ-ޤ]+");
                if (alifbaa.IsMatch(s))
                {
                    doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                }
                else
                {
                    doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                }
            }

            Break_Accross_Page(doc);
            doc.Range.Replace(new Regex("@?<.+?>"), new mvReplaceEvaluator(""), false);
            RemoveAspose(doc);
            MemoryStream ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Pdf);
            ms.Position = 0;
            return ms;
        }

        public MemoryStream Generate_Quotation(List<Dictionary<object, object>> CATList, Dictionary<object, object> ManifestDT, string docfilename)
        {
            Document doc = new Document(docfilename);
            DocumentBuilder db = new DocumentBuilder(doc);
            db.MoveToBookmark("table");

            TableContent table = new TableContent();
            table.tHeader = new Dictionary<int, TableCell>();
            table.RowCount = new List<int>();

            int i = 0;
            table.tHeader.Add(i++, new TableCell() { Value = "DESCRIPTION", direction = "", font = "Faruma", Width = 50 });
            table.tHeader.Add(i++, new TableCell() { Value = "UNIT", direction = "", font = "Faruma", Width = 14 });
            table.tHeader.Add(i++, new TableCell() { Value = "QTY", direction = "", font = "Faruma", Width = 10 });
            table.tHeader.Add(i++, new TableCell() { Value = "U/PRICE", direction = "", font = "Faruma", Width = 12 });
            table.tHeader.Add(i++, new TableCell() { Value = "Total", direction = "", font = "Faruma", Width = 14 });

            table.tBody = new List<Dictionary<int, TableCell>>();
            TableCell tcell = new TableCell();

            int RowCount = 0;
            i = 0;
            Dictionary<int, TableCell> table_row;

            foreach (Dictionary<object, object> dictionary in CATList)
            {
                table_row = new Dictionary<int, TableCell>();
                foreach (string key in dictionary.Keys)
                {
                    string s = dictionary[key].ToString();
                    if (key.Equals("SalesCATName") || key.Equals("SalesItemName") || key.Equals("UnitName") || key.Equals("Quantity") || key.Equals("UnitPrice") || key.Equals("ItemTotal"))
                    {
                        table_row.Add(i++, new TableCell()
                        {
                            Value = dictionary[key].ToString(),
                            direction = "",
                            font = "Calibri",
                            //  Width = 20
                        });
                    }
                }

                RowCount++;
                table.RowCount.Add(RowCount);
                table.tBody.Add(table_row);
            }


            db.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            // db = table.DrawTable(db, false, true);
            db = table.DrawTable(db, false, true, false, true);

            foreach (string key in ManifestDT.Keys)
            {
                string s = ManifestDT[key].ToString();
                Regex alifbaa = new Regex(@"[ހ-ޤ]+");
                if (alifbaa.IsMatch(s))
                {
                    doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                }
                else
                {
                    doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                }
            }

            Break_Accross_Page(doc);
            doc.Range.Replace(new Regex("@?<.+?>"), new mvReplaceEvaluator(""), false);
            RemoveAspose(doc);
            MemoryStream ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Pdf);
            ms.Position = 0;
            return ms;
        }

        public MemoryStream Generate_SEABL(List<Dictionary<object, object>> ContainerList, Dictionary<object, object> BLinfo, string docfilename)
        {
            Document doc = new Document(docfilename);
            DocumentBuilder db = new DocumentBuilder(doc);
            db.MoveToBookmark("table");

            TableContent table = new TableContent();
            table.tHeader = new Dictionary<int, TableCell>();
            table.RowCount = new List<int>();

            int i = 0;
            /*table.tHeader.Add(i++, new TableCell() { Value = "CONTAINER & SEAL No.", direction = "", font = "Faruma", Width = 50 });
            table.tHeader.Add(i++, new TableCell() { Value = "NUMBER AND TYPE OF PACKAGE", direction = "", font = "Faruma", Width = 30 });
            table.tHeader.Add(i++, new TableCell() { Value = "TYPE OF SERVICE", direction = "", font = "Faruma", Width = 20 });*/
          /*  table.tHeader.Add(i++, new TableCell() { Value = "DESCRIPTION OF GOODS", direction = "", font = "Faruma", Width = 30 });
            table.tHeader.Add(i++, new TableCell() { Value = "WEIGHT(KG)", direction = "", font = "Faruma", Width = 8 });
            table.tHeader.Add(i++, new TableCell() { Value = "MEASURE(M3)", direction = "", font = "Faruma", Width = 8 });*/

            table.tHeader.Add(i++, new TableCell() { Value = "CONTAINER No.", direction = "", font = "Calibri", Width = 30 });
            table.tHeader.Add(i++, new TableCell() { Value = "SEAL No.", direction = "", font = "Calibri", Width = 30 });
            table.tHeader.Add(i++, new TableCell() { Value = "INDICATOR", direction = "", font = "Calibri", Width = 30 });
            table.tHeader.Add(i++, new TableCell() { Value = "CONTAINER SIZE", direction = "", font = "Calibri", Width = 50 });
            table.tHeader.Add(i++, new TableCell() { Value = "CONTAINER TYPE", direction = "", font = "Calibri", Width = 50 });
            table.tHeader.Add(i++, new TableCell() { Value = "NO.OF PACKAGE", direction = "", font = "Calibri", Width = 50 });
            table.tHeader.Add(i++, new TableCell() { Value = "TYPE OF PACKAGE", direction = "", font = "Calibri", Width = 50 });

            table.tBody = new List<Dictionary<int, TableCell>>();
            TableCell tcell = new TableCell();

            int RowCount = 0;
            i = 0;
            Dictionary<int, TableCell> table_row;

            foreach (Dictionary<object, object> dictionary in ContainerList)
            {
                table_row = new Dictionary<int, TableCell>();
                foreach (string key in dictionary.Keys)
                {
                    string s = dictionary[key].ToString();
                        table_row.Add(i++, new TableCell()
                        {
                            Value = dictionary[key].ToString(),
                            direction = "",
                            font = "Calibri",
                            //  Width = 20
                        });
                }

                RowCount++;
                table.RowCount.Add(RowCount);
                table.tBody.Add(table_row);
            }


            db.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            // db = table.DrawTable(db, false, true);
            db = table.DrawTable(db, false, false, true,false,false);

            foreach (string key in BLinfo.Keys)
            {
                string s = BLinfo[key].ToString();
                Regex alifbaa = new Regex(@"[ހ-ޤ]+");
                if (alifbaa.IsMatch(s))
                {
                    doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                }
                else
                {
                    doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                }
            }

            Break_Accross_Page(doc);
            doc.Range.Replace(new Regex("@?<.+?>"), new mvReplaceEvaluator(""), false);
            RemoveAspose(doc);
            MemoryStream ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Pdf);
            ms.Position = 0;
            return ms;
        }

        public MemoryStream Generate_GDN(List<Dictionary<object, object>> ContainerList, Dictionary<object, object> BLinfo, string docfilename)
        {
            Document doc = new Document(docfilename);
            DocumentBuilder db = new DocumentBuilder(doc);
            db.MoveToBookmark("table");

            TableContent table = new TableContent();
            table.tHeader = new Dictionary<int, TableCell>();
            table.RowCount = new List<int>();
            
            int i = 0;
            table.tHeader.Add(i++, new TableCell() { Value = "CONTAINER No.", direction = "", font = "Calibri", Width = 30 });
            table.tHeader.Add(i++, new TableCell() { Value = "SEAL No.", direction = "", font = "Calibri", Width = 30 });
            table.tHeader.Add(i++, new TableCell() { Value = "INDICATOR", direction = "", font = "Calibri", Width = 30 });
            table.tHeader.Add(i++, new TableCell() { Value = "CONTAINER SIZE", direction = "", font = "Calibri", Width = 50 });
            table.tHeader.Add(i++, new TableCell() { Value = "CONTAINER TYPE", direction = "", font = "Calibri", Width = 50 });
            table.tHeader.Add(i++, new TableCell() { Value = "NO.OF PACKAGE", direction = "", font = "Calibri", Width = 50 });
            table.tHeader.Add(i++, new TableCell() { Value = "TYPE OF PACKAGE", direction = "", font = "Calibri", Width = 50 });

            table.tBody = new List<Dictionary<int, TableCell>>();
            TableCell tcell = new TableCell();

            int RowCount = 0;
            i = 0;
            Dictionary<int, TableCell> table_row;

            foreach (Dictionary<object, object> dictionary in ContainerList)
            {
                table_row = new Dictionary<int, TableCell>();
                foreach (string key in dictionary.Keys)
                {
                    string s = dictionary[key].ToString();
                    table_row.Add(i++, new TableCell()
                    {
                        Value = dictionary[key].ToString(),
                        direction = "",
                        font = "Calibri",
                        //  Width = 20
                    });
                }

                RowCount++;
                table.RowCount.Add(RowCount);
                table.tBody.Add(table_row);
            }


            db.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            // db = table.DrawTable(db, false, true);
            db = table.DrawTable(db, false, true, false, false, false,true);

            foreach (string key in BLinfo.Keys)
            {
                string s = BLinfo[key].ToString();
                Regex alifbaa = new Regex(@"[ހ-ޤ]+");
                if (alifbaa.IsMatch(s))
                {
                    doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                }
                else
                {
                    doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                }
            }

            Break_Accross_Page(doc);
            doc.Range.Replace(new Regex("@?<.+?>"), new mvReplaceEvaluator(""), false);
            RemoveAspose(doc);
            MemoryStream ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Pdf);
            ms.Position = 0;
            return ms;
        }

        public MemoryStream Generate_DailyClearanceSheet(List<Dictionary<object, object>> DCUSList, Dictionary<object, object> DCBinfo, string docfilename)
        {
            Document doc = new Document(docfilename);
            DocumentBuilder db = new DocumentBuilder(doc);
            db.MoveToBookmark("table");

            TableContent table = new TableContent();
            table.tHeader = new Dictionary<int, TableCell>();
            table.RowCount = new List<int>();

            int i = 0;
            table.tHeader.Add(i++, new TableCell() { Value = "JOB", direction = "", font = "Calibri", Width = 5 });
            table.tHeader.Add(i++, new TableCell() { Value = "CUSTOMER", direction = "", font = "Calibri", Width = 30 });
            table.tHeader.Add(i++, new TableCell() { Value = "BL NO.", direction = "", font = "Calibri", Width = 5 });
            table.tHeader.Add(i++, new TableCell() { Value = "CONAINER#/SEAL/SIZE/TYPE/PKG", direction = "", font = "Calibri", Width = 5 });
            table.tHeader.Add(i++, new TableCell() { Value = "WEIGHT/UOM.", direction = "", font = "Calibri", Width = 5 });
            table.tHeader.Add(i++, new TableCell() { Value = "DECLARATION NO", direction = "", font = "Calibri", Width = 5 });
            table.tHeader.Add(i++, new TableCell() { Value = "SHIFTING REQUEST", direction = "", font = "Calibri", Width = 5 });
            table.tHeader.Add(i++, new TableCell() { Value = "CLEARANCE MODE", direction = "", font = "Calibri", Width = 5 });
            table.tHeader.Add(i++, new TableCell() { Value = "CLEARANCE PORT", direction = "", font = "Calibri", Width = 5 });
            table.tHeader.Add(i++, new TableCell() { Value = "CLEARANCE SHIFT", direction = "", font = "Calibri", Width = 5 });
            table.tHeader.Add(i++, new TableCell() { Value = "CLEARANCE PARTY", direction = "", font = "Calibri", Width = 5 });
            table.tHeader.Add(i++, new TableCell() { Value = "Staff MCS", direction = "", font = "Calibri", Width = 5 });
            table.tHeader.Add(i++, new TableCell() { Value = "Staff MPL", direction = "", font = "Calibri", Width = 5 });
            table.tHeader.Add(i++, new TableCell() { Value = "Staff ASF", direction = "", font = "Calibri", Width = 5 });
            table.tHeader.Add(i++, new TableCell() { Value = "DELIVERY PLACE", direction = "", font = "Calibri", Width = 5 });

            table.tBody = new List<Dictionary<int, TableCell>>();
            TableCell tcell = new TableCell();

            int RowCount = 0;
            i = 0;
            Dictionary<int, TableCell> table_row;

            foreach (Dictionary<object, object> dictionary in DCUSList)
            {
                table_row = new Dictionary<int, TableCell>();
                foreach (string key in dictionary.Keys)
                {
                    string s = dictionary[key].ToString();
                    table_row.Add(i++, new TableCell()
                    {
                        Value = dictionary[key].ToString(),
                        direction = "",
                        font = "Calibri",
                        //  Width = 20
                    });
                }

                RowCount++;
                table.RowCount.Add(RowCount);
                table.tBody.Add(table_row);
            }


            db.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            // db = table.DrawTable(db, false, true);
             db = table.DrawTable(db, false, false, true, false, true,true);

            foreach (string key in DCBinfo.Keys)
            {
                string s = DCBinfo[key].ToString();
                Regex alifbaa = new Regex(@"[ހ-ޤ]+");
                if (alifbaa.IsMatch(s))
                {
                    doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                }
                else
                {
                    doc.Range.Replace(new Regex("@<" + key + ">"), new mvReplaceEvaluator(s), false);
                }
            }

            Break_Accross_Page(doc);
            doc.Range.Replace(new Regex("@?<.+?>"), new mvReplaceEvaluator(""), false);
            RemoveAspose(doc);
            MemoryStream ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Pdf);
            ms.Position = 0;
            return ms;
        }

        public static MemoryStream GenerateReportWithTable(Dictionary<object, object> dictionary, string docfilename)
        {
            Document doc = new Document(docfilename);
            DocumentBuilder db = new DocumentBuilder(doc);

            foreach (string key in dictionary.Keys)
            {

                if ((db.MoveToBookmark(key) && key != "Plaintif") && (db.MoveToBookmark(key) && key != "Defendant"))
                {
                    string s = dictionary[key].ToString();
                    Regex tagRegex = new Regex(@"<[^>]+>");
                    if (tagRegex.IsMatch(s))
                    {
                        //db.InsertHtml(dictionary[key].ToString());
                        db.InsertHtml("<span dir=\"rtl\" style=\"font-family: Faruma;\">" + s + "</span>");
                    }
                    else
                    {
                        db.Write(s);
                    }
                }

                if (db.MoveToBookmark(key) && key == "Plaintif")
                {
                    string val = Convert.ToString(dictionary["Plaintif"]);
                    string[] Planitiffs = val.Split('%');
                    int dataCount = 0;
                    int nOfRow = Planitiffs.Length - 1;
                    int colunmCount = 0;
                    int rowCount = 0;
                    
                    db.StartTable();

                    RowFormat rowFormat = db.RowFormat;
                    rowFormat.Height = 20;
                    rowFormat.HeightRule = HeightRule.Exactly;

                    //right to left alignment
                    db.Font.Bidi = true;
                    //bold font
                    db.Font.BoldBi = true;

                        db.InsertCell();
                            db.CellFormat.PreferredWidth = PreferredWidth.FromPercent(54.7);
                            db.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.LightGray;
                            db.Font.BoldBi = true;
                            db.Write(" ދާއިމީ އެޑްރެސް");
                        db.InsertCell();
                            db.CellFormat.PreferredWidth = PreferredWidth.FromPercent(30);
                            db.Write(" ނަން");
                        db.InsertCell();
                            db.CellFormat.PreferredWidth = PreferredWidth.FromPercent(8);
                            db.Write(" #");
                        db.EndRow();

                    foreach (string var in Planitiffs)
                    {
                     //bold font remove
                     db.Font.BoldBi = false;

                        db.InsertCell();
                            db.CellFormat.PreferredWidth = PreferredWidth.FromPoints(10);
                            db.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.White;
                        db.Write(var);
                        string s = dictionary[key].ToString();
                        db.InsertHtml(s);
                        dataCount++;
                        if (dataCount == 3) { dataCount = 0; db.EndRow(); colunmCount++; }
                        rowCount++;
                        if (rowCount == nOfRow) { break; }
                    }
                    db.EndTable();
                }

                if (db.MoveToBookmark(key) && key == "Defendant")
                {
                    string val = Convert.ToString(dictionary["Defendant"]);
                    string[] Planitiffs = val.Split('%');
                    int dataCount = 0;
                    int nOfRow = Planitiffs.Length - 1;
                    int colunmCount = 0;
                    int rowCount = 0;

                    db.StartTable();

                    RowFormat rowFormat = db.RowFormat;
                    rowFormat.Height = 20;
                    rowFormat.HeightRule = HeightRule.Exactly;

                    //right to left alignment
                    db.Font.Bidi = true;
                    //bold font
                    db.Font.BoldBi = true;

                    db.InsertCell();
                    db.CellFormat.PreferredWidth = PreferredWidth.FromPercent(54.7);
                    db.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.LightGray;
                    db.Font.BoldBi = true;
                    db.Write(" ދާއިމީ އެޑްރެސް");
                    db.InsertCell();
                    db.CellFormat.PreferredWidth = PreferredWidth.FromPercent(30);
                    db.Write(" ނަން");
                    db.InsertCell();
                    db.CellFormat.PreferredWidth = PreferredWidth.FromPercent(8);
                    db.Write(" #");
                    db.EndRow();

                    foreach (string var in Planitiffs)
                    {
                        //bold font remove
                        db.Font.BoldBi = false;

                        db.InsertCell();
                        db.CellFormat.PreferredWidth = PreferredWidth.FromPoints(10);
                        db.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.White;
                        db.Write(var);
                        dataCount++;
                        if (dataCount == 3) { dataCount = 0; db.EndRow(); colunmCount++; }
                        rowCount++;
                        if (rowCount == nOfRow) { break; }
                    }
                    db.EndTable();
                }

            }
            RemoveAspose(doc);
            MemoryStream ms = new MemoryStream();
            doc.Save(ms, SaveFormat.Pdf);
            ms.Position = 0;
            return ms;
        }

        static void RemoveAspose(Document doc)
        {
            foreach (Run run in doc.GetChildNodes(NodeType.Run, true))
            {
                if (run.Text.Contains("Aspose"))
                {
                    run.Font.Size = 0.0;
                    break;
                }
            }
        }

        //static void RemoveAspose(Document doc)
        //{
        //    foreach (Section section in doc.GetChildNodes(NodeType.Section, true))
        //    {
        //        if (section.Range.Text.Contains("Aspose"))
        //        {
        //            // run.Font.Size = 0.0;
        //            section.Range.Text.Trim();
        //            break;
        //        }
        //    }
        //}

        public static Highcharts gererateChart(string chartTypeIn, string statTxtIn, string endDateIn, string startDateIn, List<string> XAxisList, List<object> YAxisList, string YAxisMyTitle, string XAxisMyTitle)
        {
            //set chart types
            ChartTypes ChartType = ChartTypes.Column;
            //PlotOptions PlotType = new PlotOptions();
           // Object type = PlotType.Column = new PlotOptionsColumn();
            if (chartTypeIn == "Line")
            {
                ChartType = ChartTypes.Line;
               // type = PlotType.Line = new PlotOptionsLine();
               // PlotType = new PlotOptionsLine();

            }
            else if (chartTypeIn == "Area")
            {
                ChartType = ChartTypes.Area;
            }
            else if (chartTypeIn == "Bar")
            {
                ChartType = ChartTypes.Bar;
            }

            //chart
            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { 
                    Width = 980, 
                    Height = 650, 
                    Margin = new[] { 70, 152, 130, 80 },
                    BorderWidth= 2,
                    PlotShadow = true,
                    PlotBorderWidth = 1
                })
                .SetTitle(new Title { Text = statTxtIn, Style = "font: 'normal 20px Faruma'" })
                .SetSubtitle(new Subtitle
                 {
                     Text = " (" + startDateIn + " އިން " + endDateIn + " އަށް) ",
                     X = -20,
                     Y = 40,
                     Style = "font: 'normal 16px Faruma'" 
                 })
                .SetXAxis(new XAxis
                {
                    Title = new XAxisTitle { Text = XAxisMyTitle, Style = "font: 'normal 16px Faruma'" },
                    Categories = XAxisList.ToArray<string>(),
                    Labels = new XAxisLabels
                    {
                        Rotation = -45,
                        Align = HorizontalAligns.Right,
                        Style = "font: 'normal 16px Faruma'"
                    }
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title = new YAxisTitle { Text = YAxisMyTitle, Style = "font: 'normal 16px Faruma'" }
                })
                .SetLegend(new Legend
                {
                    Enabled = true,
                    ItemStyle = "font: 'normal 16px Faruma'",
                    Align = HorizontalAligns.Right,
                    Layout = Layouts.Vertical,
                    VerticalAlign = VerticalAligns.Middle,
                  //  X = -10,
                  //  Y = 100,
                    BorderWidth = 1
                })
                .SetTooltip(new Tooltip { Formatter = "TooltipFormatter", Style = "font: 'normal 16px Faruma'" })
                .SetPlotOptions(new PlotOptions
                {

                    Column =new PlotOptionsColumn
                    {
                        DataLabels = new PlotOptionsColumnDataLabels
                        {
                            Enabled = true,
                            Rotation = 0,
                            Color = ColorTranslator.FromHtml("#000000"),
                            Align = HorizontalAligns.Center,
                            //X = -3,
                            //Y = 10,
                            Formatter = "function() { return this.y; }",
                            Style = "font: 'bold 24px Times New Roman'"
                        }
                    }
                })

                .SetSeries(new Series
                {
                    Name = YAxisMyTitle,
                    Data = new Data(YAxisList.ToArray()),
                    Type = ChartType
                })
                // Disable export button
                .SetExporting(new Exporting
                {
                    Buttons = new ExportingButtons
                    {
                        ExportButton = new ExportingButtonsExportButton
                        {
                            Enabled = false
                        },
                        PrintButton = new ExportingButtonsPrintButton
                        {
                            Enabled = true,
                            /*Align = HorizontalAligns.Right,
                            VerticalAlign = VerticalAligns.Top,
                            BorderColor = Color.Teal,
                            BorderRadius = 3,
                            BorderWidth = 1,
                            Height = 35,
                            HoverBorderColor = Color.Red,
                            HoverSymbolFill = Color.DarkGray,
                            HoverSymbolStroke = Color.Black,
                            SymbolStroke = Color.Teal,
                            SymbolSize = 20,
                            SymbolX = 18,
                            SymbolY = 18,
                            Width = 35,
                            Y = 10,
                            X = -15*/
                        }
                    },

                });

            return chart;
        }

        public static Highcharts gererateChartWithSeries(string chartTypeIn, string statTxtIn, string endDateIn, string startDateIn, List<string> XAxisList, List<object> TotalNoOfPeople, List<object> TotalNoOfFemal, List<object> TotalNoOfMale, string YAxisMyTitle)
        {
            return null;
        }

        private void InsertWatermarkText(Document doc, string watermarkText)
        {
            // Create a watermark shape. This will be a WordArt shape. 
            // You are free to try other shape types as watermarks.
            Shape watermark = new Shape(doc, ShapeType.TextPlainText);

            // Set up the text of the watermark.
            watermark.TextPath.Text = watermarkText;
            watermark.TextPath.FontFamily = "A_Waheed";
            //right to left alignment
            watermark.Font.Bidi = true;
            watermark.Width = 500;
            watermark.Height = 100;
            // Text will be directed from the bottom-left to the top-right corner.
            watermark.Rotation = -40;
            // Remove the following two lines if you need a solid black text.
            watermark.Fill.Color = Color.LightGray; // Try LightGray to get more Word-style watermark
            watermark.StrokeColor = Color.LightGray; // Try LightGray to get more Word-style watermark

            // Place the watermark in the page center.
            watermark.RelativeHorizontalPosition = RelativeHorizontalPosition.Page;
            watermark.RelativeVerticalPosition = RelativeVerticalPosition.Page;
            watermark.WrapType = WrapType.None;
            watermark.VerticalAlignment = VerticalAlignment.Center;
            watermark.HorizontalAlignment = HorizontalAlignment.Center;

            // Create a new paragraph and append the watermark to this paragraph.
            Paragraph watermarkPara = new Paragraph(doc);
            watermarkPara.AppendChild(watermark);

            // Insert the watermark into all headers of each document section.
            foreach (Section sect in doc.Sections)
            {
                // There could be up to three different headers in each section, since we want
                // the watermark to appear on all pages, insert into all headers.
                InsertWatermarkIntoHeader(watermarkPara, sect, HeaderFooterType.HeaderPrimary);
                InsertWatermarkIntoHeader(watermarkPara, sect, HeaderFooterType.HeaderFirst);
                InsertWatermarkIntoHeader(watermarkPara, sect, HeaderFooterType.HeaderEven);
            }
        }
        private void InsertWatermarkIntoHeader(Paragraph watermarkPara, Section sect, HeaderFooterType headerType)
        {
            HeaderFooter header = sect.HeadersFooters[headerType];

            if (header == null)
            {
                // There is no header of the specified type in the current section, create it.
                header = new HeaderFooter(sect.Document, headerType);
                sect.HeadersFooters.Add(header);
            }

            // Insert a clone of the watermark into the header.
            header.AppendChild(watermarkPara.Clone(true));
        }

        public class TableContent
        {

            #region DataMembers
            public Dictionary<int, TableCell> tHeader { get; set; }
            public List<Dictionary<int, TableCell>> tBody { get; set; }
            public List<int> RowCount { get; set; }
            #endregion

            public TableContent()
            {

            }

            public TableContent(bool bigTable)
            {

            }

            public DocumentBuilder DrawTable(DocumentBuilder db, bool PrintHeadersOnEachPage = false, bool smallFont = false, bool verysmallFont = false, bool noBorder = false,bool headerShade = true, bool tableAutofit = false)
            {

               // db.StartTable();
                Aspose.Words.Tables.Table table = db.StartTable(); // apply format before end table- show below

                db.CellFormat.PreferredWidth = PreferredWidth.FromPercent(54.7);
                db.CellFormat.VerticalAlignment = CellVerticalAlignment.Bottom;
                if (smallFont == true)
                {
                    db = this.DrawHeader(db, true, false, noBorder, headerShade);
                }
                else if (verysmallFont == true)
                {
                    db = this.DrawHeader(db, false, true, noBorder, headerShade);
                }
                else
                {
                    db = this.DrawHeader(db);
                }
                db.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.White;
                if (noBorder == true)
                {
                    // db.CellFormat.Borders.Left.LineWidth = 0.0;
                    // db.CellFormat.Borders.Right.LineWidth = 0.0;
                    // db.CellFormat.Borders.Top.LineWidth = 0.0;
                    //db.CellFormat.Borders.Bottom.LineWidth = 2.0;
                    db.CellFormat.Borders.LineWidth = 0;
                }
                foreach (int row in RowCount)
                {
                    foreach (KeyValuePair<int, TableCell> item in tBody[(row - 1)])
                    {
                        db.InsertCell();
                        db.CellFormat.PreferredWidth = PreferredWidth.FromPercent(item.Value.Width);
                        // db.CellFormat.Width = item.Value.Width;
                        /*  if (smallFont == false)
                          {
                              db.InsertHtml("<span dir=\"" + item.Value.direction + "\" style=\"font-family: " + item.Value.font + "; font-size: 11pt" + ";\">" + item.Value.Value + "</span>");
                          }
                          else
                          {
                              db.InsertHtml("<span dir=\"" + item.Value.direction + "\" style=\"font-family: " + item.Value.font + "; font-size: 9pt" + ";\">" + item.Value.Value + "</span>");
                          }*/
                        if (smallFont == true)
                        {
                            db.InsertHtml("<span dir=\"" + item.Value.direction + "\" style=\"font-family: " + item.Value.font + "; font-size: 8pt" + ";\">" + item.Value.Value + "</span>");
                        }
                        else if (verysmallFont == true)
                        {
                            db.CellFormat.FitText = true;// Fit Cell text
                            db.InsertHtml("<span dir=\"" + item.Value.direction + "\" style=\"font-family: " + item.Value.font + "; font-size: 6pt" + ";\">" + item.Value.Value + "</span>");
                        }
                        else
                        {
                            db.InsertHtml("<span dir=\"" + item.Value.direction + "\" style=\"font-family: " + item.Value.font + "; font-size: 11pt" + ";\">" + item.Value.Value + "</span>");
                        }
                        //db.InsertHtml("<span dir=\"" + item.Value.direction + "\" style=\"font-family: " + item.Value.font + ";\">" + item.Value.Value + "</span>");
                    }
                    db.EndRow();
                }
                if (tableAutofit == true)
                {
                    table.AutoFit(AutoFitBehavior.AutoFitToWindow); // Auto fit Table to window
                }
                table.Alignment = TableAlignment.Center;
                db.EndTable();
                return db;
            }

            public DocumentBuilder DrawHeader(DocumentBuilder db, bool smallFont = false, bool verysmallFont = false, bool noBorder = false, bool headerShade = true)
            {
                //if db hasn't started a table throw exception
                RowFormat rowFormat = db.RowFormat;
                rowFormat.Height = 15;
                rowFormat.HeightRule = HeightRule.Auto;

                //right to left alignment
                db.Font.Bidi = true;
                //bold font
                db.Font.BoldBi = true;
                //db.InsertCell();
                db.CellFormat.PreferredWidth = PreferredWidth.FromPercent(54.7);
                db.CellFormat.VerticalAlignment = CellVerticalAlignment.Bottom;
               // db.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.LightGray;
                db.Font.Bold = true;
                if (noBorder == true)
                {
                    // db.CellFormat.Borders.Left.LineWidth = 0.0;
                    // db.CellFormat.Borders.Right.LineWidth = 0.0;
                    // db.CellFormat.Borders.Top.LineWidth = 0.0;
                    //db.CellFormat.Borders.Bottom.LineWidth = 2.0;
                    db.CellFormat.Borders.LineWidth = 0;
                }

                if (headerShade == true)
                {
                    db.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.LightGray;
                }

                foreach (KeyValuePair<int, TableCell> item in tHeader)
                {
                    db.InsertCell();
                    db.CellFormat.PreferredWidth = PreferredWidth.FromPercent(item.Value.Width);
                    // db.CellFormat.Width = item.Value.Width;
                    db.CellFormat.LeftPadding = 4.0;
                    db.CellFormat.RightPadding = 4.0;
                    db.CellFormat.TopPadding = 4.0;
                    db.CellFormat.VerticalAlignment = CellVerticalAlignment.Top;
                    if (smallFont == true)
                    {
                        db.InsertHtml("<span dir=\"" + item.Value.direction + "\" style=\"font-family: " + item.Value.font + "; font-size: 9pt" + ";\">" + item.Value.Value + "</span>");
                    }
                    else if (verysmallFont == true)
                    {
                        db.InsertHtml("<span dir=\"" + item.Value.direction + "\" style=\"font-family: " + item.Value.font + "; font-size: 8pt" + ";\">" + item.Value.Value + "</span>");
                    }
                    else
                    {
                        db.InsertHtml("<span dir=\"" + item.Value.direction + "\" style=\"font-family: " + item.Value.font + "; font-weight: bold" + ";\">" + item.Value.Value + "</span>");
                    }
                    db.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                }

                db.EndRow();
                return db;
            }
        }

        public class TableCell
        {
            public double Width { get; set; }
            public string Value { get; set; }
            public string direction { get; set; }
            public string font { get; set; }
        }

        static void Break_Accross_Page(Document doc)
        {
            foreach (Node n in doc.GetChildNodes(NodeType.Table, true))
            {
                Table t = (Table)n;
                foreach (Row row in t)
                {
                    row.RowFormat.AllowBreakAcrossPages = false;
                }
                /*foreach (Cell c in t.GetChildNodes(NodeType.Cell, true))
                {
                    c.EnsureMinimum();
                    foreach (Paragraph para in c.Paragraphs)
                    {
                        if (!(c.ParentRow.IsLastRow && para.IsEndOfCell))
                        {
                            para.ParagraphFormat.KeepWithNext = true;
                        }
                    }
                }*/ /*move whole Table to next page*/
            }
        }
    }
    public class HandleNodeChanging_FontChanger : INodeChangingCallback
    {

        // Implement the NodeInserted handler to set default font settings for every Run node inserted into the Document

        void INodeChangingCallback.NodeInserted(NodeChangingArgs args)
        {

            // Change the font of inserted text contained in the Run nodes.
            if (args.Node.NodeType == NodeType.Run)
            {

                Aspose.Words.Font font = ((Run)args.Node).Font;
                Regex regex = new Regex("[\u0780-\u07BF]+");
                if (regex.IsMatch(args.Node.GetText()))
                {
                    if (!font.Name.StartsWith("MV ", StringComparison.InvariantCultureIgnoreCase))
                    {
                        font.Name = "Faruma";
                    }
                    font.Bidi = true;
                }
                else
                {
                    font.Bidi = false;
                }
                font.Size = 24;
                //font.Name = "Faruma";
            }
        }



        void INodeChangingCallback.NodeInserting(NodeChangingArgs args)
        {

            // Do Nothing

        }



        void INodeChangingCallback.NodeRemoved(NodeChangingArgs args)
        {

            // Do Nothing

        }



        void INodeChangingCallback.NodeRemoving(NodeChangingArgs args)
        {

            // Do Nothing

        }

    }


    public class mvReplaceEvaluator : IReplacingCallback
    {
        /// <summary>
        /// This is called during a replace operation each time a match is found.
        /// This method appends a number to the match string and returns it as a replacement string.
        /// </summary>
        public mvReplaceEvaluator(string NewValue, int Mode = 0)
        {
            this.NewValue = NewValue;
            this.Mode = Mode;
        }

        ReplaceAction IReplacingCallback.Replacing(ReplacingArgs e)
        {
            DocumentBuilder db = new DocumentBuilder((Document)e.MatchNode.Document);
            Regex tagMatch = new Regex(@"<[^>]+>");
            Regex langMatch = new Regex("[ހ-ޤ]+");
            Regex alphaNumeric = new Regex(@"[A-Za-z0-9\(\)]+");

            db.MoveTo(e.MatchNode);
            if (Mode == 1)
            {
                db.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                Run run = (Run)db.CurrentNode;

                List<KeyValuePair<int, string>> mList = new List<KeyValuePair<int, string>>();

                Match m = alphaNumeric.Match(NewValue);
                while (m.Success)
                {
                    mList.Add(
                            new KeyValuePair<int, string>(m.Index, m.Value.ToString())
                       );
                    m = m.NextMatch();
                }

                bool direction;
                int currIndex = 0;

                foreach (KeyValuePair<int, string> pair in mList)
                {
                    string insertedPortion = NewValue.Substring(currIndex, (pair.Key) - (currIndex));

                    if (!string.IsNullOrEmpty(insertedPortion))
                    {
                        direction = true;
                        db.Font.Bidi = direction;
                        db.Write(insertedPortion);
                    }

                    db.Font.Bidi = false;
                    db.Write(pair.Value);
                    currIndex = pair.Key + pair.Value.Length;
                }

                if (currIndex < NewValue.Length)
                {
                    db.Font.Bidi = true;
                    db.Write(NewValue.Substring(currIndex, NewValue.Length - currIndex));
                }
                e.Replacement = "";
                return ReplaceAction.Replace;
            }
            else if (Mode == 2)
            {
                //use only for FormNo, or any other text which has an overall ltr direction and is separated by a '/'
                Run run = (Run)db.CurrentNode;
                SortedDictionary<int, string> Matches = new SortedDictionary<int, string>();
                Match m = langMatch.Match(NewValue);
                while (m.Success)
                {
                    Matches.Add(m.Index, m.Value.ToString());
                    m = m.NextMatch();
                }

                bool direction;
                int currIndex = NewValue.Length;
                Matches.Reverse();

                foreach (KeyValuePair<int, string> pair in Matches)
                {
                    string insertedPortion = NewValue.Substring((pair.Key + pair.Value.Length), (currIndex) - (pair.Key + pair.Value.Length));

                    if (!string.IsNullOrEmpty(insertedPortion))
                    {
                        direction = false;
                        db.Font.Bidi = direction;
                        db.Write(insertedPortion);
                    }
                    direction = true;
                    db.Font.Bidi = direction;
                    db.Write(pair.Value);
                    db.Write("/");
                    currIndex = pair.Key;
                }

                if (currIndex > 0)
                {
                    direction = false;
                    db.Font.Bidi = direction;
                    db.Write(NewValue.Substring(0, currIndex - 1));
                }
                e.Replacement = "";
                return ReplaceAction.Replace;
            }
            else
            {
                //default Mode: 0//
                if (tagMatch.IsMatch(NewValue))
                {

                    //is html:: Do not use unless other methods don't work ~ InsertHtml ignores all formatting from the Document ~ Font-size, font-family, 
                    //direction has to be set inside the htmltags

                    // align the para to right TODO:: find a way to retrieve the direction of current paragraph and align it to that
                    db.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                   // db.InsertHtml(NewValue);
                    db.InsertHtml("<span style=\"font-size: 9pt;\">" + NewValue + "</span>");
                    e.Replacement = "";
                    return ReplaceAction.Replace;
                }
                else
                {
                    if (langMatch.IsMatch(NewValue))
                    {
                        Run run = (Run)db.CurrentNode;
                        run.Font.Bidi = true;
                        e.Replacement = NewValue;
                        return ReplaceAction.Replace;
                    }
                    else
                    {
                        Run run = (Run)db.CurrentNode;
                        run.Font.Bidi = false;
                    }
                }
                e.Replacement = NewValue;
                return ReplaceAction.Replace;
            }
        }
        private string NewValue;
        /*
         * Mode 0: default,
         * Mode 1: Mixed mode, contains both thaana and eng letters.  ex. Reason in court order
         * Mode 2: Mixed mode eng, contains both thaana and end but the overall text direction is ltr, ex. FormNo
         */
        private int Mode;
    }
}