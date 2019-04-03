using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F = iTextSharp.text.Font;

namespace FinaveoV2_PDF
{
   public class FinaveoV2PdfEventHelper : PdfPageEventHelper
    {
        PdfContentByte _cb;
        PdfTemplate _footerTemplate;
        BaseFont _bf = null;
        private readonly CarmignacPdfProposal _proposition;
        private readonly string _topLogoPath;
        private readonly string _footerLogoPath;
        private readonly string _fontFilePath;
        private readonly bool _isBTO;
        private readonly bool _isProposalRefused;

        BaseColor vertCarmignac = new BaseColor(78, 130, 103);

        public FinaveoV2PdfEventHelper(CarmignacPdfProposal p, System.Drawing.Color couleurCarmignac, string topLogoPath = null, string footerLogoPath = null, string fontFilePath = null, bool isBTO = false, bool isProposalRefused = false)
        {
            _proposition = p;
            _topLogoPath = topLogoPath;
            _footerLogoPath = footerLogoPath;
            _fontFilePath = fontFilePath;
            _isBTO = isBTO;
            _isProposalRefused = isProposalRefused;
            vertCarmignac = new BaseColor(couleurCarmignac);
        }
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                _bf = GetVerdana(9F, F.NORMAL).BaseFont;
                _cb = writer.DirectContent;
                _footerTemplate = _cb.CreateTemplate(50, 50);
            }
            catch (Exception)
            {
            }
        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);


            var rect = new Rectangle(0, 842, 595, 737)
            {
                BackgroundColor = new BaseColor(240 , 240, 240)
            };
            _cb.Rectangle(rect);

            //float x = 0;
            //float y = 737;
            //_cb.SetRGBColorFill(vertCarmignac.R, vertCarmignac.G, vertCarmignac.B);
            //_cb.SetRGBColorStroke(vertCarmignac.R, vertCarmignac.G, vertCarmignac.B);
            //_cb.MoveTo(x, y + 13);
            //_cb.LineTo(x + 300, y);
            //_cb.LineTo(x, y - 13);
            //_cb.ClosePathFillStroke();

            ColumnText ct = new ColumnText(_cb);
            if (writer.PageNumber % 2 == 1)
                ct.SetSimpleColumn(400, 740, 590, 830);
            else
                ct.SetSimpleColumn(42, 740, 232, 830);
            if (_isBTO)
            {
                ct.AddElement(new Phrase(15, " ", GetVerdana(16F, F.NORMAL/*, BaseColor.WHITE*/)));
                ct.AddElement(new Phrase(20, "Bulletin de transmission d’ordre", GetVerdana(16F, F.NORMAL/*, BaseColor.WHITE*/)));
            }
            else
            {
                ct.AddElement(new Phrase(_proposition.Gestion == TypeGestion.GL ? 25 : 18, _proposition.Gestion == TypeGestion.GL ? "Arbitrage à exécuter" : (this._isProposalRefused) ? "Proposition refusée" : "Arbitrage à exécuter et rapport d'adéquation", GetVerdana(14F, F.NORMAL/*, BaseColor.WHITE*/)));
                ct.AddElement(new Phrase(13, "Numéro : " + _proposition.NumeroProposition, GetVerdana(9F, F.NORMAL/*, BaseColor.WHITE*/)));
                ct.AddElement(new Phrase(13, "Date : " + _proposition.DateProposition.ToString("dd/MM/yyyy HH:mm"), GetVerdana(9F, F.NORMAL/*, BaseColor.WHITE*/)));
                if (this._proposition.Gestion != TypeGestion.GL)
                    ct.AddElement(new Phrase(13, "Validité : 72h (" + _proposition.DateProposition.AddHours(72).ToString("dd/MM/yyyy HH:mm") + ")", GetVerdana(9F, F.NORMAL/*, BaseColor.WHITE*/)));
            }
            ct.Go();

            if (!string.IsNullOrEmpty(_topLogoPath) && File.Exists(_topLogoPath))
            {
                var topLogo = System.Drawing.Image.FromFile(_topLogoPath);
                var logo = Image.GetInstance(topLogo, System.Drawing.Imaging.ImageFormat.Png);
                if (writer.PageNumber % 2 == 1)
                    logo.SetAbsolutePosition(42, 750);
                else
                    logo.SetAbsolutePosition(360, 750);
                logo.ScaleAbsoluteWidth((int)(657 * 0.16));
                logo.ScaleAbsoluteHeight((int)(479 * 0.16));
                _cb.AddImage(logo);
                _cb.Stroke();
            }

            if (!string.IsNullOrEmpty(_footerLogoPath) && File.Exists(_footerLogoPath))
            {
                var footerLogo = System.Drawing.Image.FromFile(_footerLogoPath);
                var logo = Image.GetInstance(footerLogo, System.Drawing.Imaging.ImageFormat.Png);
                logo.SetAbsolutePosition(50, 25);
                logo.ScaleAbsoluteHeight((int)(151 * 0.12));
                logo.ScaleAbsoluteWidth((int)(683 * 0.12));
                _cb.AddImage(logo);
                _cb.Stroke();
            }

            ColumnText disclaimer = new ColumnText(_cb);
            disclaimer.SetSimpleColumn(170, 10, 490, 45);
            //if (_proposition.Gestion == TypeGestion.GC)
                disclaimer.AddElement(new Phrase(8, "Entreprise d’Investissement agréée par la Banque de France sous le numéro 19073G Société Anonyme au capital de 289.400€", GetVerdana(5F, F.NORMAL)));
                disclaimer.AddElement(new Phrase(8, "RCS Paris 512 179 680 ", GetVerdana(5F, F.NORMAL)));
                disclaimer.AddElement(new Phrase(8, "89 Bd Malesherbes – 75008 Paris", GetVerdana(5F, F.NORMAL)));
            //else
            //    disclaimer.AddElement(new Phrase(8, "Carmignac Gestion - 24 place Vendôme - 75001 Paris, France - Téléphone : +33 (0)1 73 00 10 50 - Email : gestion.libre@carmignac.com - Société de gestion de portefeuille(agrément n° GP 97 - 08 du 13 / 03 / 1997) - S.A.au capital de 15 000 000 € -RCS Paris B 349 501 676 - TVA intracommunautaire: FR 303 495 01 676", GetVerdana(5F, F.NORMAL)));

            disclaimer.Go();
            _cb.Stroke();

            //Add paging to footer
            {
                var text = writer.PageNumber + "/";
                _cb.BeginText();
                _cb.SetFontAndSize(_bf, 9F);
                _cb.SetTextMatrix(document.PageSize.GetRight(80), document.PageSize.GetBottom(30));
                _cb.ShowText(text);
                _cb.EndText();
                float len = _bf.GetWidthPoint(text, 9F);
                _cb.AddTemplate(_footerTemplate, document.PageSize.GetRight(80) + len, document.PageSize.GetBottom(30));
            }

            //Move the pointer and draw line to separate footer section from rest of page
            _cb.MoveTo(40, document.PageSize.GetBottom(50));
            _cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            _cb.Stroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            _footerTemplate.BeginText();
            _footerTemplate.SetFontAndSize(_bf, 9F);
            _footerTemplate.SetTextMatrix(0, 0);
            _footerTemplate.ShowText((writer.PageNumber).ToString());
            _footerTemplate.EndText();
        }
        F GetVerdana(float taille, int style = F.NORMAL, BaseColor color = null)
        {
            color = color ?? new BaseColor(111, 111, 111);
            var fontName = "Verdana";
            if (!FontFactory.IsRegistered(fontName) && File.Exists(_fontFilePath))
            {
                FontFactory.Register(_fontFilePath);
            }
            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, taille, style, color);
        }



    }
}
