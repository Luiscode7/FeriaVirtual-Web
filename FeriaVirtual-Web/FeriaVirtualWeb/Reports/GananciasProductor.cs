using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FeriaVirtualWeb.Reports
{
    public class GananciasProductor
    {
        int totalColumnas = 2;
        Document document;
        Font fontStyle;
        PdfPTable pdfTable = new PdfPTable(2);
        PdfPCell pdfCell;
        MemoryStream memoryStream = new MemoryStream();
        VENTA newVenta = new VENTA();
        public byte[] PdfReport(VENTA venta)
        {
            newVenta = venta;
            document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            document.SetPageSize(PageSize.A4);
            document.SetMargins(20f, 20f, 20f, 20f);
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();
            pdfTable.SetWidths(new float[] { 20f, 150f });

            ReportHeader();
            ReportBody();
            pdfTable.HeaderRows = 2;
            document.Add(pdfTable);
            document.Close();
            return memoryStream.ToArray();

        }

        private void ReportHeader()
        {
            fontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            pdfCell = new PdfPCell(new Phrase("GANANCIA DE VENTA", fontStyle));
            pdfCell.Colspan = totalColumnas;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.Border = 0;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfCell.ExtraParagraphSpace = 0;
            pdfCell.PaddingBottom = 10f;
            pdfTable.AddCell(pdfCell);
            pdfTable.CompleteRow();

            //fontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            //pdfCell = new PdfPCell(new Phrase("Items Valores", fontStyle));
            //pdfCell.Colspan = totalColumnas;
            //pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfCell.Border = 0;
            //pdfCell.BackgroundColor = BaseColor.WHITE;
            //pdfCell.ExtraParagraphSpace = 0;
            //pdfTable.AddCell(pdfCell);
            //pdfTable.CompleteRow();
        }

        private void ReportBody()
        {
            //fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            //pdfCell = new PdfPCell(new Phrase("Items Ganancia", fontStyle));
            //pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //pdfTable.AddCell(pdfCell);

            //pdfCell = new PdfPCell(new Phrase("Items Valores Ganancia", fontStyle));
            //pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //pdfTable.AddCell(pdfCell);
            //pdfTable.CompleteRow();


            fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            pdfCell = new PdfPCell(new Phrase("N° PROCESO DE VENTA", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(newVenta.PROCESOVENTA_IDPROCESOVENTA.ToString(), fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfTable.AddCell(pdfCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            pdfCell = new PdfPCell(new Phrase("IMPUESTO ADUANA", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(newVenta.IMPUESTOADUANA.ToString(), fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfTable.AddCell(pdfCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            pdfCell = new PdfPCell(new Phrase("COSTO TRASPORTE", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(newVenta.COSTOTRANSPORTE.ToString(), fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfTable.AddCell(pdfCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            pdfCell = new PdfPCell(new Phrase("COMISION EMPRESA", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(newVenta.COMISIONEMPRESA.ToString(), fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfTable.AddCell(pdfCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            pdfCell = new PdfPCell(new Phrase("GANANCIA NETA", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(newVenta.GANANCIAPRODUCTORNETA.ToString(), fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfTable.AddCell(pdfCell);
            pdfTable.CompleteRow();

            fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            pdfCell = new PdfPCell(new Phrase("GANANCIA TOTAL", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfTable.AddCell(pdfCell);


            pdfCell = new PdfPCell(new Phrase(newVenta.GANANCIATOTAL.ToString(), fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfTable.AddCell(pdfCell);
            pdfTable.CompleteRow();
        }
    }
}