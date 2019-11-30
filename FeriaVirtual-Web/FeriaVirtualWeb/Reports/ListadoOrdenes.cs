using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Models.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FeriaVirtualWeb.Reports
{
    public class ListadoOrdenes
    {
        int totalColumnas = 4;
        Document document;
        Font fontStyle;
        PdfPTable pdfTable = new PdfPTable(4);
        PdfPCell pdfCell;
        MemoryStream memoryStream = new MemoryStream();
        List<ProcesoVentaViewModel> newOrden = new List<ProcesoVentaViewModel>();

        public byte[] PdfReport(List<ProcesoVentaViewModel> orden)
        {
            newOrden = orden;
            document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            document.SetPageSize(PageSize.A4);
            document.SetMargins(20f, 20f, 20f, 20f);
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();
            pdfTable.SetWidths(new float[] { 20f, 50f, 50f, 50f });

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
            pdfCell = new PdfPCell(new Phrase("LISTADO DE ORDENES DE COMPRA", fontStyle));
            pdfCell.Colspan = totalColumnas;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.Border = 0;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfCell.ExtraParagraphSpace = 0;
            pdfCell.PaddingBottom = 10f;
            pdfTable.AddCell(pdfCell);
            pdfTable.CompleteRow();
        }

        private void ReportBody()
        {
            fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            pdfCell = new PdfPCell(new Phrase("N° ORDEN", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfTable.AddCell(pdfCell);


            pdfCell = new PdfPCell(new Phrase("FECHA", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfTable.AddCell(pdfCell);


            pdfCell = new PdfPCell(new Phrase("RUT CLIENTE", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfTable.AddCell(pdfCell);


            pdfCell = new PdfPCell(new Phrase("NOMBRE CLIENTE", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfTable.AddCell(pdfCell);
            pdfTable.CompleteRow();

            foreach (var item in newOrden)
            {
                fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                pdfCell = new PdfPCell(new Phrase(item.ORDEN.ToString(), fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdfTable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(item.FECHA.ToString(), fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdfTable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(item.RUTCLIENTELOCAL.ToString(), fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdfTable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(item.NOMBRECLIENTE.ToString(), fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdfTable.AddCell(pdfCell);
                pdfTable.CompleteRow();
            }
        }
    }
}