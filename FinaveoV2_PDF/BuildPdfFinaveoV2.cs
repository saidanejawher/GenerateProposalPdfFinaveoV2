using FinaveoV2_PDF.model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace FinaveoV2_PDF
{
    public static class BuildPdfFinaveoV2
    {
        private static string CarmignacProposalsRootPath => ConfigurationManager.AppSettings.Get("carmiganc.proposals.rootpath");

        private static string CarmignacLogoTransparentPath => ConfigurationManager.AppSettings.Get("carmiganc.logotransparent.path");

        private static string CarmignacLogoPath => ConfigurationManager.AppSettings.Get("carmiganc.logo.path");

        private static string CarmignacPdfFontPath => ConfigurationManager.AppSettings.Get("carmiganc.font.path");

        private static string CarmignacTemplateMif2Path => ConfigurationManager.AppSettings.Get("carmiganc.proposals.template.mif2.path");
        private static string CarmignacTemplateConditionsTransmissionPath => ConfigurationManager.AppSettings.Get("carmiganc.proposals.template.conditions.transmissions.path");
        private static string CarmignacCoffrePdfProposalRootPath => ConfigurationManager.AppSettings.Get("carmiganc.proposals.coffrerootpath");


        private const double CarmignacCgpSignXCoordinate = 108;
        private const double CarmignacCgpSignYCoordinate = 41;

        private const double CarmignacTitulaireSignXCoordinate = 20;
        private const double CarmignacTitulaireSignYCoordinate = 40;

        private const double CarmignacCoTitulaireSignXCoordinate = 60;
        private const double CarmignacCoTitulaireSignYCoordinate = 40;

        //private static Logger _logger = new Logger();


        private static readonly string[] CouleursRisques = new[] { "#329646", "#FFA726", "#b01c35" };

        //private static ProposalsRepository _repository = new ProposalsRepository();


        private static readonly object[][] CodesGlobalisateurs = { //demande d'adrien
            new object[] { CodeGlobalisateur.ACT, "Titre vif - Actions",  Color.FromArgb(64,140,240)},
            new object[] { CodeGlobalisateur.CASH,"Liquidités", Color.FromArgb(252,180,65)},
            new object[] { CodeGlobalisateur.CERTIF,"Certificat d'investissement",  Color.FromArgb(224,64,10)},
            new object[] { CodeGlobalisateur.DROITS,"Droits",  Color.FromArgb(5,100,146)},
            new object[] { CodeGlobalisateur.FDS_EUR,"Fonds EURO", Color.FromArgb(191,191,191)},
            new object[] { CodeGlobalisateur.OBL,"Titre vif - Obligations",  Color.FromArgb(26,59,105)},
            new object[] { CodeGlobalisateur.OPC_ACT,"Gestion Actions - Globale", Color.FromArgb(255,227,130)},
            new object[] { CodeGlobalisateur.OPC_ALT,"Gestion Alternative",  Color.FromArgb(18,156,221)},
            new object[] { CodeGlobalisateur.OPC_DIV,"Gestion Diversifiée", Color.FromArgb(202,107,75)},
            new object[] { CodeGlobalisateur.OPC_EUR,"Gestion Actions - Européenne",Color.FromArgb(0,92,219)},
            new object[] { CodeGlobalisateur.OPC_FOR,"Gestion Profilée", Color.FromArgb(243,209,133)},
            new object[] { CodeGlobalisateur.OPC_FORA,"Gestion Profilée Actions",  Color.FromArgb(80,99,129)},
            new object[] { CodeGlobalisateur.OPC_FORD,"Gestion Profilée Diversifiée", Color.FromArgb(241,185,168)},
            new object[] { CodeGlobalisateur.OPC_FORT,"Gestion Taux", Color.FromArgb(224,131,10)},
            new object[] { CodeGlobalisateur.OPC_MON,"Gestion Monétaire", Color.FromArgb(120,147,190)},
            new object[] { CodeGlobalisateur.OPC_OBL,"Gestion Taux", Color.FromArgb(255,174,201)},
            new object[] { CodeGlobalisateur.OPC_SPE,"Gestion Actions - Spécialisée", Color.FromArgb(181,230,29)},
            new object[] { CodeGlobalisateur.OPCVM,"OPCVM", Color.FromArgb(112,146,190)},
            new object[] { CodeGlobalisateur.OTHERS,"Monétaire et Obligations",Color.FromArgb(200,191,231)}
        };
        private static readonly Color VertCarmignac = Color.FromArgb(241, 65, 65);
        private static readonly string VertCarmignacHtml = "#03344A";
        private static readonly string VertFoncéCarmignacHtml = "#0f2d0f";

        public static bool Build_Pdf_Finaveo_V2(string Path, CarmignacPdfProposal proposal)
        {
            var content = BuildCarmignacPdfProposal(proposal);
            File.WriteAllBytes(Path, content);
            if (File.Exists(Path))
                return true;
            else
                return false;

        }
        public static byte[] BuildCarmignacPdfProposal(this CarmignacPdfProposal proposal)
        {
            var allBytes = new List<byte[]>();
            try
            {
                //
                using (MemoryStream ms = new MemoryStream())
                using (var doc = new Document(PageSize.A4, 50f, 50f, 135f, 85f))
                using (var writer = PdfWriter.GetInstance(doc, ms))
                {
                    int signaturePage;
                    writer.PageEvent = new FinaveoV2PdfEventHelper(proposal, VertCarmignac, CarmignacLogoTransparentPath, CarmignacLogoPath, CarmignacPdfFontPath);
                    doc.Open();
                    BuildPdf(proposal, writer, doc);

                    //add mif2 frais page
                    //if (
                    //    (proposal.Gestion != TypeGestion.GL && proposal.TypeOrdre != ProposalOrdersType.AcheteVendu && proposal.TypeOrdre != ProposalOrdersType.VenteRachat)
                    //    ||
                    //    (proposal.Gestion != TypeGestion.GL && proposal.TypeOrdre == ProposalOrdersType.VenteRachat && proposal.IsRachatPartiel)
                    //    )
                    //{
                    //    var templateLexique = GetPdfFromPath(CarmignacTemplateMif2Path);
                    //    if (templateLexique != null)
                    //    {
                    //        var reader = new PdfReader(templateLexique);
                    //        var cb = writer.DirectContent;
                    //        for (var i = 1; i <= reader.NumberOfPages; i++)
                    //        {
                    //            doc.NewPage();
                    //            var page = writer.GetImportedPage(reader, i);
                    //            cb.AddTemplate(page, 0, 0);
                    //        }
                    //    }
                    //}



                    //BuildBTO(proposal, writer, doc, out signaturePage);

                    //var templateConditions = GetPdfFromPath(CarmignacTemplateConditionsTransmissionPath);
                    //if (templateConditions != null)
                    //{
                    //    var reader = new PdfReader(templateConditions);
                    //    var cb = writer.DirectContent;
                    //    for (var i = 1; i <= reader.NumberOfPages; i++)
                    //    {
                    //        doc.NewPage();
                    //        var page = writer.GetImportedPage(reader, i);
                    //        cb.AddTemplate(page, 0, 0);
                    //    }
                    //}

                    signaturePage = writer.CurrentPageNumber - 1;
                    doc.Close();




                    //all supports dici
                    //var supports = _repository.GetSupportsByIdProposal(proposal.IdProposal);
                    //var dici = GetFundsDici(supports.Select(s => s.fiche_dici).ToList());

                    //List<byte[]> dici = null;
                    allBytes.Add(ms.ToArray());



                    //if (dici != null && dici.Any())
                    //    allBytes.AddRange(dici);


                    return PdfMerge(allBytes, signaturePage, proposal.Gestion == TypeGestion.GL);


                }
            }
            catch (Exception e)
            {
                //_logger.LogException(new LogMessage()
                //{
                //    Date = DateTime.Now,
                //    Message = e.SerializeException(),

                //    WebMethod = MethodBase.GetCurrentMethod().Name
                //});
                return null;
            }
        }

        private static byte[] PdfMerge(IEnumerable<byte[]> bytelist, int signaturePage, bool isGlMode = false)
        {

            try
            {
                byte[] all;

                using (var ms = new MemoryStream())
                {
                    var doc = new Document();
                    var writer = PdfWriter.GetInstance(doc, ms);
                    // adding metada (signature page number, signature coordinates,...)
                    writer.Info.Put(new PdfName("SignaturePage"), new PdfString(signaturePage.ToString()));
                    writer.Info.Put(new PdfName("SignatureCgpXCoordinate"), new PdfString(CarmignacCgpSignXCoordinate.ToString(CultureInfo.InvariantCulture)));
                    writer.Info.Put(new PdfName("SignatureCgpYCoordinate"), new PdfString(CarmignacCgpSignYCoordinate.ToString(CultureInfo.InvariantCulture)));
                    writer.Info.Put(new PdfName("SignatureTitulaireXCoordinate"), new PdfString(CarmignacTitulaireSignXCoordinate.ToString(CultureInfo.InvariantCulture)));
                    writer.Info.Put(new PdfName("SignatureTitulaireYCoordinate"), new PdfString(CarmignacTitulaireSignYCoordinate.ToString(CultureInfo.InvariantCulture)));
                    writer.Info.Put(new PdfName("SignatureCoTitulaireXCoordinate"), new PdfString(CarmignacCoTitulaireSignXCoordinate.ToString(CultureInfo.InvariantCulture)));
                    writer.Info.Put(new PdfName("SignatureCoTitulaireYCoordinate"), new PdfString(CarmignacCoTitulaireSignYCoordinate.ToString(CultureInfo.InvariantCulture)));
                    writer.Info.Put(new PdfName("CreatedBy"), new PdfString("Upsideo"));
                    writer.Info.Put(new PdfName("CreatedFor"), new PdfString("Carmignac"));
                    writer.Info.Put(new PdfName("DocumentCreationDate"), new PdfString(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                    writer.Info.Put(new PdfName("DocumentType"), new PdfString("Proposition d'investissement"));
                    // set page size
                    doc.SetPageSize(PageSize.A4);
                    doc.Open();
                    var cb = writer.DirectContent;

                    foreach (var p in bytelist)
                    {
                        try
                        {
                            var reader = new PdfReader(p);
                            PdfReader.unethicalreading = true;
                            var pages = reader.NumberOfPages;
                            for (var i = 1; i <= pages; i++)
                            {
                                // doc.SetPageSize(PageSize.A4.Rotate()); // Merge pdf mode paysage : 
                                doc.SetPageSize(PageSize.A4);
                                doc.NewPage();
                                var page = writer.GetImportedPage(reader, i);
                                cb.AddTemplate(page, 0, 0);
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }

                    doc.Close();
                    all = ms.GetBuffer();
                    ms.Flush();
                    ms.Dispose();
                }

                return all;

            }
            catch (Exception e)
            {
                //_logger.LogException(new LogMessage()
                //{
                //    Date = DateTime.Now,
                //    Message = e.SerializeException(),

                //    WebMethod = MethodBase.GetCurrentMethod().Name
                //});
                return null;
            }

        }


        private static void BuildPdf(CarmignacPdfProposal proposition, PdfWriter writer, Document document)
        {
            try
            {


                var typeGestion = ((DescriptionAttribute)typeof(TypeGestion).GetMember(proposition.Gestion.ToString())[0].GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description;
                var profilActuel = ((DescriptionAttribute)typeof(Profil).GetMember(proposition.ProfilActuel.ToString())[0].GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description;
                var profilCible = ((DescriptionAttribute)typeof(Profil).GetMember(proposition.ProfilPropose.ToString())[0].GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description;
                var profilKyc = ((DescriptionAttribute)typeof(Profil).GetMember(proposition.ProfilKYC.ToString())[0].GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description;

                //var clientIndivs = _repository.GetClientsByAccountsNumbers(new List<string>() { proposition.NumeroCompte });
                //var clients = (clientIndivs == null || !clientIndivs.Any()) ? proposition.Client : string.Join("<br/>", clientIndivs.Select(c => c.Item2));
                var clients = "testClient";
                var client = "<div class='c3'><h3>Client</h3><hr />" + clients + "<br /><br /><label style='font-size:10px'>Profil d’investisseur : " + profilKyc + "</label></div>" +
                           "<div class='space'></div>" +
                           "<div class='c3'><h3>Compte</h3><hr />Type de placement : " + proposition.TypeEnveloppe + "<br /><br /><label style='font-size:10px'>Mode de gestion : " + typeGestion + "<br />N° " + proposition.NumeroCompte + "<br /></label></div>" +
                           "<div class='space'></div>" +
                           "<div class='c3'><h3>Gérant privé</h3><hr />" + proposition.Cgp + "<br /><br /><label style='font-size:10px'>Téléphone : " + proposition.TelCGP + "</label><br /><label style='font-size:10px'>Code : " + proposition.CodeCgp + "</label></div>";

                var disclaimerCours = "<div style='height:5px'></div><div style='font-size:8px'>*Selon les cours d’exécution, les quantités et/ou montants indiqués sont susceptibles de varier, et de facto les frais</div>";
                var tableauActuel = HtmlTableau("Portefeuille actuel", new[] { "Libellé valeur", "ISIN", "Quantité", "Cours en devise", "Valorisation", "Date de valorisation", "Prix d’Achat Moyen", "+/- values latentes (€)", "+/- values latentes (%)", "Poids" }, proposition.PortefeuilleActuel, proposition);
                var tableauPropose = HtmlTableau("Portefeuille cible", new[] { "Libellé valeur", "ISIN", "Quantité*", "Cours en devise", "Valorisation*", "Poids" }, proposition.PortefeuillePropose, proposition);
                var tableauOrdres = HtmlTableau("Ordres à exécuter", new[] { "Sens", "Libellé valeur", "ISIN", "Quantité*", "Cours en devise", "Montant*" }, proposition.Ordres, proposition);
                //<div style='page-break-before:always'>&nbsp;</div>
                tableauOrdres = "<table style='width:660px;'><tr><td style='border: 3px solid #ddd;'>" + tableauOrdres + "</td></tr></table>";
                tableauOrdres += "<table style='width:660px;'><tr><td>Frais* : " + proposition.FraisGlobauxPourcent.ToString("N") + "&nbsp;%&nbsp;/&nbsp;" + proposition.FraisGlobaux.ToString("N") + "&nbsp;" + proposition.SymboleDeviseFraisGlobaux + "</td><td>Achats : " + proposition.SommeAchats.ToString("N") + "&nbsp;" + proposition.SymboleDeviseSommeAchats + "</td><td>Ventes : " + proposition.SommeVentes.ToString("N") + "&nbsp;" + proposition.SymboleDeviseSommeVentes + "</td><td>Ventes-Achats : " + proposition.SommeAchatsVentes.ToString("N") + "&nbsp;" + proposition.SymboleDeviseSommeAchatsVentes + "</td></tr></table>";
                tableauOrdres += disclaimerCours;
                tableauPropose += disclaimerCours;
                var risques = "";
                if (proposition.Gestion == TypeGestion.GC)
                {
                    var couleurActuelle = CouleursRisques[proposition.risqueMoyenActuel < 2 ? 0 : (proposition.risqueMoyenActuel < 4 ? 1 : 2)];
                    var couleurProposée = CouleursRisques[proposition.risqueMoyenPropose < 2 ? 0 : (proposition.risqueMoyenPropose < 4 ? 1 : 2)];
                    risques = "<div class='c2'><h3>Justification</h3><hr /><label style='color:" + VertCarmignacHtml + ";font-weight:bold;'>" + proposition.Objectif + "</label><br /><br /><label style='font-size:10px'>" + string.Join("<br/>", proposition.Motif.Split(new[] { ';' })) + "</label></div>" +
                                    "<div class='space'></div>" +
                                    "<div class='c2'><h3>Adéquation avec le profil d'investisseur</h3><hr />" +
                                    "Portefeuille actuel : " + profilActuel + " - note pondérée de <label style='color:" + couleurActuelle + ";'>" + proposition.risqueMoyenActuel.ToString("N") + "</label><br />";
                    risques += HtmlTableauRisques(proposition.ProfilActuel);
                    risques += "Portefeuille cible : " + profilCible + " - note pondérée de <label style='color:" + couleurProposée + ";'>" + proposition.risqueMoyenPropose.ToString("N") + "</label><br /><div style='height:3px'></div>";
                    risques += HtmlTableauRisques(proposition.ProfilPropose);
                    risques += "Profil d'investisseur : <label>" + profilKyc + "</label><br /><div style='height:3px'></div>";
                    risques += HtmlTableauRisques(proposition.ProfilKYC);
                    risques += "<div style='font-size:8px'><label style='color:" + CouleursRisques[0] + "'>Prudent</label>&nbsp;-&nbsp;<label style='color:" + CouleursRisques[1] + "'>Equilibré</label>&nbsp;-&nbsp;<label style='color:" + CouleursRisques[2] + "'>Dynamique</label></div>";
                    risques += "<div style='height:7px'></div><div style='display:table-cell;vertical-align:middle;width:320px;height:45px;background-color:#6F6F6F;color:white'><div style='text-align:center;padding:5px;'>" + (proposition.DepassementProfil ? "Le niveau de risque du portefeuille cible n'est pas conforme au profil d'investisseur du client." : "Le niveau de risque du portefeuille cible est conforme au profil d'investisseur du client.") + "</div>";
                    risques += "</ div >";
                    risques += "<div style='height:7px'></div><label style='font-size:8px'>Les risques sont répartis par SRRI (Synthetic Risk and Reward Indicator). Cet indicateur synthétique de risque et de performance figure sur les DICI (Document d'Information Clef pour l'Investisseur) des fonds.</label>";
                    risques += "</ div ><div style='height:20px'></div>";
                    if (proposition.DepassementProfil)
                        risques += "<div style='float:left;background:" + CouleursRisques[2] + ";width:5px;height:65px;'></div><div style='float:left;width:5px;height:50px;'></div><div style='float:left;'><label>Si vous décidez de passer l(es)'ordre(s) d'achat et/ou de vente décrit(s) dans ce document, votre portefeuille global en référence ne sera pas conforme à vos objectifs et à votre situation personnelle. En effet, la note de risque du portefeuille (SRRI pondéré) ne sera pas en adéquation avec votre profil d'investisseur. Cette situation relève d'une décision de votre part, prise en toute connaissance de cause et non de la recommandation initiale formulée par votre Gérant privé.</label></div>";
                    else
                        risques += "<div style='float:left;background:" + CouleursRisques[0] + ";width:5px;height:50px;'></div><div style='float:left;width:5px;height:50px;'></div><div style='float:left;'><label>Si vous décidez de passer l(es)'ordre(s) d'achat et/ou de vente décrit(s) dans ce document, votre portefeuille global en référence sera conforme à vos objectifs et à votre situation personnelle. En effet, la note de risque du portefeuille (SRRI pondéré) sera en adéquation avec votre profil d'investisseur.</label></div>";
                }
                foreach (CodeGlobalisateur v in Enum.GetValues(typeof(CodeGlobalisateur)))
                {
                    if (proposition.RepartitionActuelleClassesActifs != null)
                    {
                        if (!proposition.RepartitionActuelleClassesActifs.Any(x => x.Code == v))
                        {
                            //proposition.RepartitionActuelleClassesActifs.Add(v, 0);
                            proposition.RepartitionActuelleClassesActifs.Add(new RepartitionActifElement
                            {
                                Code = v,
                                Value = 0
                            });
                        }
                    }
                    if (proposition.RepartitionProposeeClassesActifs != null)
                    {
                        if (!proposition.RepartitionProposeeClassesActifs.Any(x => x.Code == v))
                            proposition.RepartitionProposeeClassesActifs.Add(new RepartitionActifElement
                            {
                                Code = v,
                                Value = 0
                            });
                    }
                }

                var pathGraphAvant = (proposition.RepartitionActuelleClassesActifs != null) ? GetGraph(proposition.RepartitionActuelleClassesActifs.OrderBy(x => x.Code.ToString()).ToDictionary(y => y.Code, y => y.Value)) : string.Empty;
                var pathGraphApres = (proposition.RepartitionProposeeClassesActifs != null) ? GetGraph(proposition.RepartitionProposeeClassesActifs.OrderBy(x => x.Code.ToString()).ToDictionary(y => y.Code, y => y.Value)) : string.Empty;

                var legende = "<table style='width:660px'><tr>";
                var sautDePage = "<div style='page-break-before:always'>&nbsp;</div>";
                //List<object[]> legendes = (proposition.RepartitionActuelleClassesActifs != null && proposition.RepartitionProposeeClassesActifs != null) ? CodesGlobalisateurs.Where(x => proposition.RepartitionActuelleClassesActifs[(CodeGlobalisateur)x[0]] != 0 || proposition.RepartitionProposeeClassesActifs[(CodeGlobalisateur)x[0]] != 0).ToList() : null;
                List<object[]> legendes = (proposition.RepartitionActuelleClassesActifs != null && proposition.RepartitionProposeeClassesActifs != null) ?
                        CodesGlobalisateurs.Where(x => (proposition.RepartitionActuelleClassesActifs.Any(o => o.Value != 0.00d && ((CodeGlobalisateur)x[0] == o.Code)) && proposition.RepartitionActuelleClassesActifs.First(o => o.Value != 0.00d).Value != 0) || ((proposition.RepartitionProposeeClassesActifs.Any(o => o.Value != 0.00d && ((CodeGlobalisateur)x[0] == o.Code))) && (proposition.RepartitionProposeeClassesActifs.First(o => o.Value != 0.00d).Value != 0d))).ToList()
                        : null;
                if (legendes != null)
                    for (int i = 0; i < legendes.Count; i++)
                    {
                        var couleur = (Color)legendes[i][2];
                        legende += "<td style='padding: 0px;'><label style='background:#" + couleur.R.ToString("X2") + couleur.G.ToString("X2") + couleur.B.ToString("X2") + ";'>&nbsp;&nbsp;&nbsp;</label>&nbsp;" + legendes[i][1] + "&nbsp;&nbsp;&nbsp;&nbsp;</td>";
                        if (i % 4 == 3) legende += "</tr><tr><td colspan=4 style='height:5px;'></td></tr><tr>";
                    }
                legende += "</tr></table>";
                var graphiques = "<div><h3>Répartition par classes d'actifs</h3><hr />" +
                    "<table style='width:660px'><tr><td align='center'><label style='color:" + VertCarmignacHtml + ";font-weight:bold;font-size:12px'>Portefeuille actuel</label><br /><br /><img width='250' height='250' src='" + pathGraphAvant + "' /></td>" +
                    "<td align='center'><label style='color:" + VertCarmignacHtml + ";font-weight:bold;font-size:12px'>Portefeuille cible</label><br /><br /><img width='250' height='250' src='" + pathGraphApres + "' /></td>" +
                    "</tr></table><div style='height:7px'></div><label style='font-size:10px'>" + legende + "</label></div>";

                var disclaimerAdequation = proposition.Gestion == TypeGestion.GL ? "" : "<div style='font-size:10px'>Le(s) OPC envisagé(s) dans l'arbitrage ne nécessite(nt) pas que vous demandiez en cours d'année un réexamen de l'adéquation de votre portefeuille avec votre profil d'investisseur. Cet examen sera proposé annuellement par Carmignac Gestion, sauf si dans l'intervalle, les conditions de marché ou un changement dans votre profil d'investisseur exigent un réexamen anticipé, conformément à la convention signée entre nous.</div>";

                var style = @"
                table { font-size : 10px; border-collapse:collapse; }
                th { border-bottom: 1px solid #ddd; font-weight:bold; color : " + VertCarmignacHtml + @"; }
                th, td { padding: 4px; }
                td { color : black }
                hr { background-color: #6F6F6F; }
                div { font-size : 12px; }
                .c2 { float : left; width:320px; }
                .c3 { float : left; width:206px; }
                h3 { color : #6F6F6F; font-size : 15px; }
                .space { float : left; width:20px; }
                .footer { border-top: 1px solid #ddd; }
                .rightAlign { text-align: right; }
            ";

                var mif2Frais = ((proposition.Gestion != TypeGestion.GL && proposition.TypeOrdre != ProposalOrdersType.AcheteVendu && proposition.TypeOrdre != ProposalOrdersType.VenteRachat) || (proposition.Gestion != TypeGestion.GL && proposition.TypeOrdre == ProposalOrdersType.VenteRachat && proposition.IsRachatPartiel)) ? BuildMif2FraisSection(proposition.FraisMif2, proposition.TypeEnveloppe, (proposition.Gestion == TypeGestion.GC) ? "Gestion conseillée" : (proposition.Gestion == TypeGestion.GL) ? "Gestion libre" : "", "Sans objet", profilKyc) : string.Empty;
                var graphMif2 = ((proposition.Gestion != TypeGestion.GL && proposition.TypeOrdre != ProposalOrdersType.AcheteVendu && proposition.TypeOrdre != ProposalOrdersType.VenteRachat) || (proposition.Gestion != TypeGestion.GL && proposition.TypeOrdre == ProposalOrdersType.VenteRachat && proposition.IsRachatPartiel)) ? GetGraphMif2(proposition.FraisMif2.TableauGrapheEffetRendement) : string.Empty;
                var htmlGraphMif2 = ((proposition.Gestion != TypeGestion.GL && proposition.TypeOrdre != ProposalOrdersType.AcheteVendu && proposition.TypeOrdre != ProposalOrdersType.VenteRachat) || (proposition.Gestion != TypeGestion.GL && proposition.TypeOrdre == ProposalOrdersType.VenteRachat && proposition.IsRachatPartiel)) ? "<div>" +
                                    "<table style='width:700px'><tr><td align='center'><label style='color:" + VertCarmignacHtml + ";font-weight:bold;font-size:18px'>Effet sur le rendement selon l'année de sortie</label><br /><br /><img width='700' height='500' src='" + graphMif2 + "' /></td>" +
                                    "</tr></table></div>" : string.Empty;

                var sautSection = "<div style='height:30px'></div>";
                using (var reader = GenerateStreamFromString(client + sautSection + tableauActuel + sautSection + tableauOrdres + sautSection + tableauPropose + sautSection + risques + sautSection + sautDePage + graphiques + sautSection + disclaimerAdequation + (((proposition.Gestion != TypeGestion.GL && proposition.TypeOrdre != ProposalOrdersType.AcheteVendu && proposition.TypeOrdre != ProposalOrdersType.VenteRachat) || (proposition.Gestion != TypeGestion.GL && proposition.TypeOrdre == ProposalOrdersType.VenteRachat && proposition.IsRachatPartiel)) ? (sautDePage + mif2Frais + sautSection + sautDePage + htmlGraphMif2) : string.Empty)))
                using (var r2 = GenerateStreamFromString(style))
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, reader, r2);
                if (File.Exists(pathGraphAvant))
                    File.Delete(pathGraphAvant);
                if (File.Exists(pathGraphApres))
                    File.Delete(pathGraphApres);

                if (((proposition.Gestion != TypeGestion.GL && proposition.TypeOrdre != ProposalOrdersType.AcheteVendu && proposition.TypeOrdre != ProposalOrdersType.VenteRachat) || (proposition.Gestion != TypeGestion.GL && proposition.TypeOrdre == ProposalOrdersType.VenteRachat && proposition.IsRachatPartiel)) && File.Exists(graphMif2))
                    File.Delete(graphMif2);
            }
            catch (Exception e)
            {

                throw;
            }

        }

        private static string HtmlTableau(string titre, string[] colonnes, IList lignes, CarmignacPdfProposal proposal)
        {
            var header = "<div><h3>" + titre + "</h3><hr />";
            int width = 660;
            if (lignes != null && lignes.Count > 0 && lignes[0] is OrderToExecute)
                width = 650;
            var tableau = "<table style='width:" + width + "px;repeat-header: yes;'>";
            tableau += "<thead><tr>";
            foreach (var v in colonnes)
                tableau += "<th align='center' " + (v == "Libellé valeur" && titre == "Portefeuille actuel" ? "width='120px'" : "") + ">" + v + "</th>";
            tableau += "</tr></thead>";

            if (lignes != null && lignes.Count > 0 && lignes[0] is CurrentPortfolioLine)
            {
                var sortedList = ((List<CurrentPortfolioLine>)lignes).Where(l => !string.IsNullOrEmpty(l.Isin) && !l.Isin.Trim().Equals("FONDS EURO", StringComparison.OrdinalIgnoreCase)).OrderBy(l => l.LibelleValeur).ToList();
                var cashOrFondsEuroLine = ((List<CurrentPortfolioLine>)lignes).FirstOrDefault(l => string.IsNullOrEmpty(l.Isin) || l.Isin.Trim().Equals("FONDS EURO", StringComparison.OrdinalIgnoreCase));
                if (cashOrFondsEuroLine != null)
                {
                    sortedList.Add(cashOrFondsEuroLine);
                }
                foreach (CurrentPortfolioLine v in sortedList)
                {
                    if (!string.IsNullOrEmpty(v.Isin))
                        tableau += "<tr><td>" + v.LibelleValeur + "</td><td>" + v.Isin + "</td><td class='rightAlign'>" + v.Quantite.ToString("### ### ### ##0.#######") + "</td><td class='rightAlign'>" + v.Cours.ToString("N") + "&nbsp;" + v.SymboleDeviseCours + "</td><td class='rightAlign'>" + v.Valo.ToString("N") + "&nbsp;" + v.SymboleDeviseValo + "</td><td>" + v.DateValo.ToString("dd/MM/yyyy") + "</td><td class='rightAlign'>" + v.Pam.ToString("N") + "&nbsp;" + v.SymboleDevisePam + "</td><td class='rightAlign'>" + v.Pmv.ToString("N") + "&nbsp;" + v.SymboleDevisePmv + "</td><td class='rightAlign'>" + (v.PmvPourcentage * 100).ToString("N") + "&nbsp;%</td><td class='rightAlign'>" + (v.Poids) + "&nbsp;%</td></tr>";
                    else
                    {// cash line
                        tableau += "<tr><td>" + v.LibelleValeur + "</td><td></td><td class='rightAlign'></td><td class='rightAlign'></td><td class='rightAlign'>" + v.Valo.ToString("N") + "&nbsp;" + v.SymboleDeviseValo + "</td><td>" + v.DateValo.ToString("dd/MM/yyyy") + "</td><td class='rightAlign'></td><td class='rightAlign'></td><td class='rightAlign'></td><td class='rightAlign'>" + (v.Poids) + "&nbsp;%</td></tr>";
                    }
                }
                tableau += "<tr><td class='footer' colspan='4'></td><td class='footer rightAlign'>" + proposal.ValoTotaleActuelle.ToString("N") + "&nbsp;" + proposal.SymboleDeviseValoTotaleActuelle + "</td><td class='footer' colspan='2'></td><td class='footer rightAlign'></td><td class='footer rightAlign'></td><td class='footer'></td></tr>";
            }
            if (lignes != null && lignes.Count > 0 && lignes[0] is ProposedPortfolioLine)
            {
                var sortedList = ((List<ProposedPortfolioLine>)lignes).Where(l => !string.IsNullOrEmpty(l.Isin) && !l.Isin.Trim().Equals("FONDS EURO", StringComparison.OrdinalIgnoreCase)).OrderBy(l => l.LibelleValeur).ToList();
                var cashOrFondsEuroLine = ((List<ProposedPortfolioLine>)lignes).FirstOrDefault(l => string.IsNullOrEmpty(l.Isin) || l.Isin.Trim().Equals("FONDS EURO", StringComparison.OrdinalIgnoreCase));
                if (cashOrFondsEuroLine != null)
                {
                    sortedList.Add(cashOrFondsEuroLine);
                }

                foreach (ProposedPortfolioLine v in sortedList)
                {
                    if (!string.IsNullOrEmpty(v.Isin))
                        tableau += "<tr><td>" + v.LibelleValeur + "</td><td>" + v.Isin + "</td><td class='rightAlign'>" + v.Quantite.ToString("### ### ### ##0.#######") + "</td><td class='rightAlign'>" + v.Cours.ToString("N") + "&nbsp;" + v.SymboleDeviseCours + "</td><td class='rightAlign'>" + v.ValoEstimee.ToString("N") + "&nbsp;" + v.SymboleDeviseValoEstimee + "</td><td class='rightAlign'>" + (v.Poids) + "&nbsp;%</td></tr>";
                    else
                    {// cash line
                        tableau += "<tr><td>" + v.LibelleValeur + "</td><td>" + v.Isin + "</td><td class='rightAlign'></td><td class='rightAlign'></td><td class='rightAlign'>" + v.ValoEstimee.ToString("N") + "&nbsp;" + v.SymboleDeviseValoEstimee + "</td><td class='rightAlign'>" + (v.Poids) + "&nbsp;%</td></tr>";
                    }
                }
                tableau += "<tr><td class='footer' colspan='4'></td><td class='footer rightAlign'>" + proposal.ValoTotaleProposee.ToString("N") + "&nbsp;" + proposal.SymboleDeviseValoTotaleProposee + "</td><td class='footer'></td></tr>";
            }
            if (lignes != null && lignes.Count > 0 && lignes[0] is OrderToExecute)
            {
                //1
                //header = "<div><div style='background:" + vertFoncéCarmignacHTML + ";float:left;height:25px;width:5px;'></div><div style='float:left;padding-left:10px'><h3>" + titre + "</h3></div><div></div><hr />";
                //2
                //var carre = "<div style='float:left;padding-top:8px;padding-right:5px;'><div style='background:" + vertFoncéCarmignacHTML + ";height:8px;width:8px;'></div></div>";
                //header = "<div>" + carre + carre + carre + "<div style='float:left;padding-left:5px'><h3>" + titre + "</h3></div><div></div><hr />";
                //3
                //header = "<div><div style='background:" + vertFoncéCarmignacHTML + ";'><h3 style='color:white;padding-bottom:5px'>" + titre + "</h3></div><hr style='margin-top:0px;'/>";
                //4
                //header = "<div><h3 style='text-align:right'>" + titre + "</h3><hr />";
                //5
                //header = "<div><table><tr><td style='border: 1px solid #ddd;'><h3>" + titre + "</h3><hr />";
                var sortedList = ((List<OrderToExecute>)lignes).Where(l => !string.IsNullOrEmpty(l.Isin) && !l.Isin.Trim().Equals("FONDS EURO", StringComparison.OrdinalIgnoreCase)).OrderBy(l => l.LibelleValeur).ToList();
                var cashOrFondsEuroLine = ((List<OrderToExecute>)lignes).FirstOrDefault(l => string.IsNullOrEmpty(l.Isin) || l.Isin.Trim().Equals("FONDS EURO", StringComparison.OrdinalIgnoreCase));
                if (cashOrFondsEuroLine != null)
                {
                    sortedList.Add(cashOrFondsEuroLine);
                }

                foreach (OrderToExecute v in sortedList)
                    tableau += "<tr><td>" + (v.Sens == OrderType.AcheteVendu.ToDescription() ? "A/V" : (v.Sens == OrderType.Souscription.ToDescription() ? "Achat" : "Vente")) + "</td><td>" + v.LibelleValeur + "</td><td>" + v.Isin + "</td><td class='rightAlign'>" + v.Quantite.ToString("### ### ### ##0.#######") + "</td><td class='rightAlign'>" + v.Cours.ToString("N") + "&nbsp;" + v.SymboleDeviseCours + "</td><td class='rightAlign'>" + v.Montant.ToString("N") + "&nbsp;" + v.SymboleDeviseMontant + "</td></tr>";
            }
            tableau += "</table></div>";
            return header + tableau;
        }

        private static string HtmlTableauRisques(Profil profil)
        {
            var res = "<table style='width:320px;height:20px; border-collapse: collapse;border:1px solid #6F6F6F;'><tr>";
            //var deuxiemeLigne = "";
            for (int i = 0; i < 7; i++)
            {
                //Prudent = 1 et 2
                //Equilibré = 3 et 4
                //Dynamique = 5, 6 et 7
                var couleur = i < 2 ? 0 : (i < 4 ? 1 : 2);
                var borderleft = "border-left:1px solid #6F6F6F;";
                res += "<td align='center' style='" + (i != 0 ? borderleft : "") + (couleur == Convert.ToInt32(profil) ? "background:" + CouleursRisques[couleur] : "") + ";color:" + (couleur == Convert.ToInt32(profil) ? "white" : CouleursRisques[couleur]) + "'>" + (i + 1) + "</td>";
                //deuxiemeLigne += "<td align='center' style='width:46px;'>" + (Math.Truncate(moyenne) == i ? moyenne.ToString("F2") : "") + "</td>";
            }
            //res += "</tr></table>" + (moyenne == 0 ? "" : ("<table style='width:320px;height:20px; border-collapse: collapse;'><tr>" + deuxiemeLigne + "</tr></table>"));
            res += "</tr></table>";
            return res;
        }

        private static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        private static string GetGraph(Dictionary<CodeGlobalisateur, double> repartition)
        {
            try
            {

                var tempWorkDirectory = ConfigurationManager.AppSettings["carmiganc.proposals.tempWorkPath"];
                if (Directory.Exists(tempWorkDirectory))
                    Directory.CreateDirectory(tempWorkDirectory);
                var path = Path.Combine(tempWorkDirectory, Guid.NewGuid() + ".png");
                using (var ch = new Chart())
                {
                    ch.Size = new Size(250, 250);
                    ch.ChartAreas.Add(new ChartArea());
                    ch.Palette = ChartColorPalette.None;
                    ch.PaletteCustomColors = CodesGlobalisateurs.Select(x => (Color)x[2]).ToArray();
                    var s = new Series
                    {
                        ChartType = SeriesChartType.Pie,
                        //CustomProperties = "PieStartAngle=270,PieLabelStyle=Outside,PieLineColor=Black"
                        CustomProperties = "PieStartAngle=270,PieLineColor=Black"
                    };
                    foreach (var pnt in repartition) s.Points.AddXY(pnt.Value == 0 ? "" : (pnt.Value).ToString("##0.00") + " %", pnt.Value);
                    s.Points.Where(x => x.YValues[0] == 0).ToList().ForEach(x => x.CustomProperties = "PieLineColor = White");
                    ch.Series.Add(s);
                    ch.SaveImage(path, ChartImageFormat.Png);
                }
                return path;

            }
            catch (Exception e)
            {
                //_logger.LogException(new LogMessage()
                //{
                //    Date = DateTime.Now,
                //    Message = e.SerializeException(),

                //    WebMethod = MethodBase.GetCurrentMethod().Name
                //});
                return string.Empty;
            }

        }

        private static byte[] GetPdfFromPath(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    return File.ReadAllBytes(path);
                }
                return null;
            }
            catch (Exception e)
            {


                return null;
            }
        }

        private static void BuildBTO(CarmignacPdfProposal proposition, PdfWriter writer, Document document, out int signaturePage)
        {
            var pp = "<div class='c2'>" + GetCheckBox(proposition.TypeClient == PersonType.Physique, "PERSONNE PHYSIQUE") + "<br /><table style='width:320px;height:70px;'><tr><td style='border:2px solid #ddd;'>" + GetCheckBox(proposition.Titre == CivilityType.Monsieur, "Monsieur", 10) + GetCheckBox(proposition.Titre == CivilityType.Madame, "Madame", 10) + GetCheckBox(proposition.Titre == CivilityType.MonsieurOuMadame, "Madame et/ou Monsieur", 10) + "<br />Nom / Prénom : " + (proposition.TypeClient == PersonType.Physique ? proposition.Client : "") + "</td></tr></table></div>";
            var pm = "<div class='c2'>" + GetCheckBox(proposition.TypeClient == PersonType.Morale, "PERSONNE MORALE") + "<br /><table style='width:320px;height:70px;'><tr><td style='border:2px solid #ddd;'>Société : " + (proposition.TypeClient == PersonType.Morale ? proposition.Client : "") + "<br /><br />Nom / Prénom mandataire : " + proposition.Mandataire + "</td></tr></table></div>";
            var pppm = pp + pm;
            var compteOperation = "<div>Numéro de compte : " + proposition.NumeroCompte + "</div><div style='height:10px'></div><div><table style='width:650px'><tr><td style='border:2px solid #ddd;'>OPERATION</td><td style='border:2px solid #ddd;border-left:0px;padding:0px;'><table><tr><td>" + GetCheckBox((ProposalOrdersType)proposition.TypeOrdre == ProposalOrdersType.AchatSouscription, "Achat") + "</td><td>" + GetCheckBox((ProposalOrdersType)proposition.TypeOrdre == ProposalOrdersType.VenteRachat, "Vente") + "</td><td>" + GetCheckBox((ProposalOrdersType)proposition.TypeOrdre == ProposalOrdersType.Arbitrage, "Arbitrage") + "</td><td>" + GetCheckBox((ProposalOrdersType)proposition.TypeOrdre == ProposalOrdersType.AcheteVendu, "Acheté/Vendu") + "</td></tr></table></td></tr></table></div>";
            var entetetableauOrdres = "<table style='width:650px;font-size:10px;'><thead><tr><th align='center'>Ordre</th><th align='center'>Type</th><th align='center'>% du portefeuille cible</th><th align='center'>% de la ligne existante</th><th align='center'>Montant*</th><th align='center'>Quantité*</th></tr></thead>";
            var pageFixedNbOrders = 7;
            var pageMaxNbOrders = 8;
            var sautDePage = "<div style='page-break-before:always'>&nbsp;</div>";
            signaturePage = (proposition.Ordres.Count % pageFixedNbOrders);
            var tableauOrdres = entetetableauOrdres;


            var signaturePanelPadding = (proposition.Ordres.Count > pageMaxNbOrders) ? 400 : 220;
            try
            {


                int counter = 0;
                var sortedList = proposition.Ordres.Where(l => !string.IsNullOrEmpty(l.Isin) && !l.Isin.Trim().Equals("FONDS EURO", StringComparison.OrdinalIgnoreCase)).OrderBy(l => l.LibelleValeur).ToList();
                var cashOrFondsEuroLine = proposition.Ordres.FirstOrDefault(l => string.IsNullOrEmpty(l.Isin) || l.Isin.Trim().Equals("FONDS EURO", StringComparison.OrdinalIgnoreCase));
                if (cashOrFondsEuroLine != null)
                {
                    sortedList.Add(cashOrFondsEuroLine);
                }

                foreach (var v in sortedList)
                {
                    if ((counter % pageMaxNbOrders == 0) && (counter != 0))
                    {
                        tableauOrdres += "</table>";
                        tableauOrdres += sautDePage;
                        tableauOrdres += entetetableauOrdres;
                    }

                    var selectedValueStyle = " style='padding-right:0px;padding-left:0px;' ";
                    tableauOrdres += "<tr><td>" + $"{v.LibelleValeur} ( {v.Isin} )" + "</td>" +
                                     "<td align='center'>" + v.Sens + "</td>" +
                                     "<td align='center'" + ((v.SelectedValueType.Trim().Equals("Percent")) ? selectedValueStyle : "") + ">" + (!v.SelectedValueType.Trim().Equals("LinePercent") ? (v.PourcentagePtf).ToString("N") + " %" : "-") + "</td>" +
                                     "<td align='center'" + ((v.SelectedValueType.Trim().Equals("LinePercent")) ? selectedValueStyle : "") + ">" + ((v.SelectedValueType.Trim().Equals("LinePercent")) ? (v.PourcentagePtf).ToString("N") + "%" : "-") + " </td>" +
                                     "<td align='center'" + ((v.SelectedValueType.Trim().Equals("Amount")) ? selectedValueStyle : "") + ">" + v.Montant.ToString("N") + "&nbsp;" + v.SymboleDeviseMontant + "</td>" +
                                     "<td align='center'" + ((v.SelectedValueType.Trim().Equals("Quantity")) ? selectedValueStyle : "") + ">" + v.Quantite.ToString("### ### ### ##0.#######") + "</td></tr>";
                    counter++;


                }
                tableauOrdres += "</table>";

                // to manage signature position : it depends on number of orders and if we are in first page of BTO or not
                if (proposition.Ordres.Count <= pageMaxNbOrders)
                {//we are in first page
                    for (var x = 0; x < proposition.Ordres.Count; x++)
                    {
                        signaturePanelPadding -= (20 + x * 2);
                    }
                }
                else
                {//next pages
                    for (var x = 0; x < (proposition.Ordres.Count % pageMaxNbOrders); x++)
                    {
                        signaturePanelPadding -= (20 + x * 2);
                    }
                }

                var disclaimerCours = "<div style='height:5px'></div><div style='font-size:8px'>*Selon les cours d’exécution, les quantités et/ou montants indiqués sont susceptibles de varier, et de facto les frais</div>";
                tableauOrdres += disclaimerCours;
                tableauOrdres += "<div style='float:right'><table style='font-size:9px'><tr><td>Légende : </td><td style='width:1px;'></td><td style='padding-right:0px;padding-left:0px;' align='center'>Expression de l'ordre</td><td style='width:1px;'></td></tr></table></div>";


                var reglementAchat = "<div class='c2'><table style='width:320px;'><tr><td style='border:2px solid #ddd;' align='center'>REGLEMENT DE L'ACHAT</td></tr><tr><td style='border-left:2px solid #ddd;border-right:2px solid #ddd;padding-top:0px;padding-bottom:0px;'>" + GetCheckBox(proposition.ReglementAchat == ReglementType.cheque, "par chèque", 10) + GetCheckBox(proposition.ReglementAchat == ReglementType.virement, "par virement", 10) + "</td></tr><tr><td style='border-left:2px solid #ddd;border-right:2px solid #ddd;padding-top:0px;padding-bottom:0px;'>" + GetCheckBox(proposition.ReglementAchat == ReglementType.especes, "par utilisation du solde espèces", 10) + GetCheckBox(proposition.ReglementAchat == ReglementType.arbitrage, "Arbitrage", 10) + "</td></tr><tr><td style='height:29px;border-left:2px solid #ddd;border-right:2px solid #ddd;'>&nbsp;</td></tr><tr><td style='border:2px solid #ddd;'>Montant en € : " + proposition.montantReglementAchat.ToString("### ### ### ##0.00") + " </td></tr></table></div>";
                var reglementVente = "<div class='c2'><table style='width:320px;'><tr><td style='border:2px solid #ddd;' align='center'>REGLEMENT DE LA VENTE</td></tr><tr><td style='border-left:2px solid #ddd;border-right:2px solid #ddd;padding-top:0px;padding-bottom:0px;'>" + GetCheckBox(proposition.ReglementVente == ReglementType.cheque, "par chèque", 10) + GetCheckBox(proposition.ReglementVente == ReglementType.virement, "par virement (joindre un RIB)", 10) + "</td></tr><tr><td style='border-left:2px solid #ddd;border-right:2px solid #ddd;padding-top:0px;padding-bottom:0px;'>" + GetCheckBox(proposition.ReglementVente == ReglementType.arbitrage, "Arbitrage", 10) + GetCheckBox(proposition.ReglementVente == ReglementType.especes, "à déposer en solde espèces <br/>&nbsp;(sur le compte de référence)", 10) + "</td></tr><tr><td style='border-left:2px solid #ddd;border-right:2px solid #ddd;padding-top:0px;padding-bottom:0px;'>" + GetCheckBox(proposition.ReglementVente == ReglementType.assurance, "à investir sur le contrat d'assurance vie<br />&nbsp;&nbsp;CARMIGNAC GESTION n° : " + proposition.NumeroAssuranceVie, 10) + "</td></tr><tr><td style='border:2px solid #ddd;'>Montant en € : " + proposition.montantReglementVente.ToString("### ### ### ##0.00") + " </td></tr></table></div>";
                var reglements = "<div  style='padding-top:" + signaturePanelPadding + "px;'>" + reglementAchat + reglementVente + "</div>";
                var divers = "<div style='height:3px'></div><div><table style='width:650px'><tr><td style='border:2px solid #ddd;'>DIVERS</td><td style='border:2px solid #ddd;border-left:0px;'>" + proposition.Divers + "</td></tr></table></div>";
                var signaturesTitulaires = "<div class='c2'><table style='width:320px;'><tr><td style='border:2px solid #ddd;border-bottom:0px'><label style='font-size:8px'>Je déclare avoir pris connaissance du Prospectus des OPCVM choisis, du DICI (Document d'Information Clé pour l'Investisseur) ainsi que des conditions figurant au verso de ce bulletin.</label><br /><label style='font-size:10px'>Fait à " + proposition.VilleSignature + " le " + proposition.DateSignature.ToString("dd/MM/yyyy") + "<br /><i>Signature Titulaire</i>" + string.Concat(Enumerable.Repeat("&nbsp;", 47)) + "<i>Signature Co-titulaire</i></label>" + string.Concat(Enumerable.Repeat("<br />", 8)) + "</td></tr><tr><td style='height:6px;border:2px solid #ddd;border-top:0px'></td></tr></table></div>";
                var signaturesConseiller = "<div style='float:left;width:150px;'><table style='width:140px;'><tr><td style='border:2px solid #ddd;font-size:10px'><i>Cachet du conseiller :</i>" + string.Concat(Enumerable.Repeat("<br />", 13)) + "</td></tr></table></div>";
                var signatureCourrier = "<div style='float:left;width:170px;'><table style='width:170px;'><tr><td style='border:2px solid #ddd;font-size:10px'><i>Réception courrier :<br />" + proposition.ReceptionCourrier + "<br /><br />Code conseiller :<br />" + proposition.CodeCgp + "<br /><br />Poste téléphonique :<br />" + proposition.NumeroPosteTelephonique + string.Concat(Enumerable.Repeat("<br />", 4)) + "Réservé à CARMIGNAC GESTION</i></td></tr></table></div>";
                var signatures = signaturesTitulaires + signaturesConseiller + signatureCourrier;

                var style = @"
                table { font-size:12px; border-collapse:collapse; }
                td { padding: 4px; }
                th { padding-right: 4px;padding-left: 4px; border-bottom: 1px solid #ddd; font-weight:bold; color : " + VertCarmignacHtml + @"; }
                div { font-size:12px; }
                hr { background-color: #6F6F6F; }
                h3 { color : #6F6F6F; font-size : 15px; }
                .c2 { float:left; width:330px; }
                .t2 { color:#4e8267;font-size:14px;font-weight:bold;text-align:center; }
                .t3 { color:" + VertFoncéCarmignacHtml + @";font-size:12px;font-weight:bold; }
            ";

                var sautSection = "<div style='height:10px'></div>";

                //using (var reader = GenerateStreamFromString(sautDePage + pppm + sautSection + compteOperation + sautSection + tableauOrdres + sautSection + reglements + divers + sautSection + signatures + sautDePage + conditions))
                using (var reader = GenerateStreamFromString(sautDePage + pppm + sautSection + compteOperation + sautSection + tableauOrdres + sautSection + reglements + divers + sautSection + signatures))
                using (var r2 = GenerateStreamFromString(style))
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, reader, r2);
            }
            catch (Exception e)
            {

            }

        }
        private static string GetCheckBox(bool isChecked, string texte, int fontSize = 12)
        {
            return "<div style='float:left;'><table style='float:left;'><tr><td><table style='float:left;width:10px;'><tr><td style='border:2px solid #ddd;padding-left:" + (isChecked ? "0" : "1") + "px;padding-right:" + (isChecked ? "0" : "1") + "px;padding-top:1px;padding-bottom:0px;height:18px;' valign='middle' align='center'>" + (isChecked ? "X" : "&nbsp;") + "</td></tr></table></td><td style='font-size:" + fontSize + "px;'>&nbsp;&nbsp;" + texte + "</td></tr></table></div>";
        }

        private static string BuildMif2FraisSection(PdfMif2Frais frais, string typePlacement, string modeGestion, string profilGestionAssocie, string profilInvestisseur)
        {
            try
            {
                var tableau = @"<div style='border: 1px solid #c9ced0;border-radius: 5px;padding: 1%;'>
                                <h4 style='text-align:center;font-weight:bold; color : " + VertCarmignacHtml + @";font-size:22px;'>COUTS ET FRAIS EX-ANTE DETAILLES</h4> <br/><br/>                              
                                <div style='text-align: center;font-size: 10px; padding: 1%;'>
                                    <table>
                                        <tbody>
                                         <tr>
                                            <td style='text-align:right;font-size: 14px;'>Hypothèses sous-jacentes :</td><td style='text-align:right;'></td><td style='text-align:right;'></td>
                                         </tr>
                                         <tr>
                                            <td style='text-align:right;'></td><td style='text-align:right;'>Type de placement : </td><td style='text-align:left;font-weight:bold;'>" + typePlacement + @"</td>
                                         </tr>
                                         <tr>
                                            <td style='text-align:right;'></td><td style='text-align:right;'>Mode de gestion : </td><td style='text-align:left;font-weight:bold;'>" + modeGestion + @"</td>
                                         </tr>
                                         <tr>
                                            <td style='text-align:right;'></td><td style='text-align:right;'>Profil de gestion associé : </td><td style='text-align:left;font-weight:bold;'>" + profilGestionAssocie + @"</td>
                                         </tr>
                                          <tr>
                                            <td style='text-align:right;'></td><td style='text-align:right;'>Profil d'investisseur : </td><td style='text-align:left;font-weight:bold;'>" + profilInvestisseur + @"</td>
                                         </tr>
                                         <tr><td colspan='3'></td></tr>
                                         <tr>
                                            <td style='text-align:right;'></td><td style='text-align:right;'> Montant de de l'investissement / arbitrage : <b>" + $"{frais.MontantArbitrage:### ### ### ### ##0.00} €" + @"</b></td><td style='text-align:right;'></td>
                                         </tr>
                                        </tbody>
                                    </table>                                  
                                </div>
                                <table style='width:660px;'><tr><td> 
                                        <table style='width:630px'>
                                    <thead>
                                        <tr>
                                            <td style='width:60%;text-align:right;font-weight:bold;color:#ffffff; background-color : " + VertCarmignacHtml + @";'></td>
                                            <td style='width:20%;text-align:right;font-weight:bold;color:#ffffff; background-color : " + VertCarmignacHtml + @";'>EUR&nbsp;</td>
                                            <td style='width:20%;text-align:right;font-weight:bold;color:#ffffff; background-color : " + VertCarmignacHtml + @";'>%&nbsp;</td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td style='line-height: 20px !important;background-color :#c9ced0;'> Coût et frais directs liés aux services d’investissement et/ou des services auxiliaires</td>
                                            <td style='text-align: right;background-color :#c9ced0;'>" +
                              $"{frais.CdeaDirectMontantCoutEtFraisServiceInvestEtAux:### ### ### ### ##0.00} €" + @"</td>
                                                <td style='text-align: right;background-color :#c9ced0;'> " +
                              $"{frais.CdeaDirectPourcentageCoutEtFraisServiceInvestEtAux:##0.00} %" + @"&nbsp;</td>
                                            </tr>
                                        <tr>
                                            <td style='line-height: 10px !important;'> Frais uniques<br/> <i>(honoraires pour acte, frais de dossier, frais de résiliation...)</i></td>
                                            <td style='text-align: right;'> " +
                              $"{frais.CdeaDirectMontantFraisUniques:### ### ### ### ##0.00} €" + @" </td>
                                            <td style='text-align: right;'>" +
                              $"{frais.CdeaDirectPourcentageFraisUniques:##0.00} %" + @"&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style='line-height: 10px !important;'> Frais récurrents<br/><i>(consommation fixe du mandat, frais de gestion du contrat, droits de garde...)</i></td>
                                            <td style='text-align: right;'> " +
                              $"{frais.CdeaDirectMontantFraisRecurrents:### ### ### ### ##0.00} €" + @" </td>
                                            < td style='text-align: right;'> " +
                              $"{frais.CdeaDirectPourcentageFraisRecurrents:##0.00} %" + @"&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style='line-height: 10px !important;'> Coûts liés aux transactions<br/><i>(droits d'entrée et frais d'arbitrage ou de transfert non acquis aux fonds)</i></td>
                                            <td style='text-align: right;'> " +
                              $"{frais.CdeaDirectMontantCoutsLiesAuxTransactions:### ### ### ### ##0.00} €" + @" </td>
                                            <td style='text-align: right;'>" +
                              $"{frais.CdeaDirectPourcentageCoutsLiesAuxTransactions:##0.00} %" + @"&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style='line-height: 20px !important;'> Coûts de services auxiliaires, tels que la recherche</td>
                                            <td style='text-align: right;'> " +
                              $"{frais.CdeaDirectMontantCoutsServicesAuxiliaires:### ### ### ### ##0.00} €" + @"</td>    
                                                <td style='text-align: right;'> " +
                              $"{frais.CdeaDirectPourcentageCoutsServicesAuxiliaires:##0.00} %" + @"&nbsp;</td>
                                            </tr>
                                            <tr>
                                            <td style='line-height: 20px !important;'> Coûts marginaux, tels que les frais de surperformance</td>
                                            <td style='text-align: right;'>" +
                              $"{frais.CdeaDirectMontantCoutsMarginaux:### ### ### ### ##0.00} €" + @" </td>
                                                <td style='text-align: right;'> " +
                              $"{frais.CdeaDirectPourcentageCoutsMarginaux:##0.00} %" + @" &nbsp;</td>
                                            </tr>
                                            <tr>
                                            <td style='line-height: 10px !important;background-color :#c9ced0;'> Coût et frais directs liés aux paiements reçus de tiers par la société d’investissement<br /><i>(rétrocessions sur frais de gestion des instruments)</i></td>
                                            <td style='text-align: right;background-color :#c9ced0;'> " +
                              $"{frais.CdeaDirectMontantLiesAuxPaiementsRecus:### ### ### ### ##0.00} €" + @" </td>
                                            <td style='text-align: right;background-color :#c9ced0;'>" +
                              $"{frais.CdeaDirectPourcentageLiesAuxPaiementsRecus:##0.00} %" + @"&nbsp;</td>
                                        </tr>
                                        
                                        <tr>
                                            <td style='line-height: 20px !important; text-align: left !important;background-color :#c9ced0;'> Coûts et frais indirects liés aux instruments financiers</td>
                                            <td style='text-align: right;background-color :#c9ced0;'>" + $"{frais.CdeaIndirectMontantLiesAuxInstruFinanciers:### ### ### ### ##0.00} €" + @"</td>
                                            <td style='text-align: right;background-color :#c9ced0;'>" + $"{frais.CdeaIndirectPourcentageLiesAuxInstruFinanciers:##0.00} %" + @"&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style='line-height: 10px !important;'> Frais uniques<br /><i>(droits d'entrée et frais d'arbitrage acquis aux fonds, frais de distribution)</i> </td>
                                            <td style='text-align: right;'>" + $"{frais.CdeaIndirectMontantFraisUniques:### ### ### ### ##0.00} €" + @" </td>
                                            <td style='text-align: right;'>" + $"{frais.CdeaIndirectPourcentageFraisUniques:##0.00} %" + @"&nbsp;</td>
                                        </tr>
                                       <tr>
                                            <td style='line-height: 10px !important;'> Frais récurrents<br /> <i>(frais de gestion des instruments diminués des rétrocessions éventuelles indiquées dans la rubrique Frais directs)</i></td>
                                            <td style='text-align: right; '> " +
                              $"{frais.CdeaIndirectMontantFraisRecurrents:### ### ### ### ##0.00} €" + @"</td>
                                            <td style='text-align: right;'>" +
                              $"{frais.CdeaIndirectPourcentageFraisRecurrents:##0.00} %" + @"&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style='line-height: 10px !important;'> Coûts liés aux transactions<br /><i>(frais d'exécution, commissions de courtage et de change, taxes, droits d'entrée/sortie payés par les instruments...)</i></td>
                                            <td style='text-align: right;'> " +
                              $"{frais.CdeaIndirectMontantLiesAuxTransactions:### ### ### ### ##0.00} €" + @"</td>
                                            <td style='text-align: right;'>" +
                              $"{frais.CdeaIndirectPourcentageLiesAuxTransactions:##0.00} %" + @"&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style='line-height: 20px !important;'> Coûts de services auxiliaires, tels que la recherche</td>
                                            <td style='text-align: right;'> " +
                              $"{frais.CdeaIndirectMontantCoutsServicesAuxiliaires:### ### ### ### ##0.00} €" + @"</td>
    
                                                <td style='text-align: right;'>" +
                              $"{frais.CdeaIndirectPourcentageCoutsServicesAuxiliaires:##0.00} %" + @"&nbsp;</td>
                                            </tr>
                                            <tr>
                                            <td style='line-height: 20px !important;'> Coûts marginaux, tels que les frais de surperformance</td>
                                            <td style='text-align: right;'> " +
                              $"{frais.CdeaIndirectMontantCoutsMarginaux:### ### ### ### ##0.00} €" + @"</td>
                                                <td style='text-align: right;'>" +
                              $"{frais.CdeaIndirectPourcentageCoutsMarginaux:##0.00} %" + @"&nbsp;</td>
                                            </tr>
                                        </tbody>
                                        <tfoot>
                                                                                       
                                            <tr>
                                            <td style='font-weight:bold;line-height: 20px !important; text-align: left !important;color:#ffffff;background-color : " + VertCarmignacHtml + @";'> Coûts et frais annuels totaux estimés sur la première année</td>
                                            <td style='font-weight:bold;text-align: right;font-weight:bold; color : #ffffff;background-color : " + VertCarmignacHtml + @";'>" +
                              $"{frais.CdeaMontantCoutsEtFraisTotaux:### ### ### ### ##0.00} €" + @"</td>
                                            <td style='font-weight:bold;text-align: right;font-weight:bold; color : #ffffff;background-color : " + VertCarmignacHtml + @";'>" +
                              $"{frais.CdeaPourcentageCoutsEtFraisTotaux:##0.00} %" + @"&nbsp;</td>
                                        </tr>
                                    </tfoot>
                                </table>
                                </td></tr>
                                <tr><td>
                                <h2 style='text-align:center;font-weight:bold;font-size:16px;'>
                                    Illustration de l'impact sur le rendement<br/>
                                </h2>
                                <table style='width:630px'  cellspacing='0'>
                                    <thead>
                                        <tr style='display: none;'>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td colspan='2' style='border:solid 1px #000000;text-align:left;color:#ffffff;font-weight:bold;background-color:" + VertCarmignacHtml + @";'>Effet cumulé sur le rendement annuel sur la période de détention recommandée</td>
                                        </tr>
                                        <tr>
                                            <td  style='border:solid 1px #000000;width: 70%;line-height: 20px !important;'> Durée de détention recommandée</td>
                                            <td  style='border:solid 1px #000000;text-align: right;'>" + frais.EcraDureeDetentionRecommandee + @"&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td  style='border:solid 1px #000000;width: 70%;line-height: 20px !important;'> Coûts et frais totaux estimés annualisées sur cette durée en %</td>
                                            <td  style='border:solid 1px #000000;text-align: right;'>" + $"{frais.EcraPourcentageCoutsEtFraisEstimes:##0.00} %" + @"&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td   style='border:solid 1px #000000;width: 70%;line-height: 20px !important;' > Coûts et frais totaux estimés annualisées en € pour l'arbitrage de : " + $"{frais.MontantArbitrage:### ### ### ### ##0.00} €" + @"</td>
                                            <td  style='border:solid 1px #000000;text-align: right;'> " + $"{frais.EcraMontantCoutsEtFraisEstimes:### ### ### ### ##0.00} €" + @"&nbsp;</td>
                                        </tr>
                                    </tbody>
                                </table></td></tr></table></div> ";


                var style = @"
                table { font-size : 10px; border-collapse:collapse; }
                th { border-bottom: 1px solid #ddd; font-weight:bold; color : " + VertCarmignacHtml + @"; }
                th, td { padding: 4px; }
                td { color : black }
                hr { background-color: #6F6F6F; }
                div { font-size : 12px; }
                .c2 { float : left; width:320px; }
                .c3 { float : left; width:206px; }
                h3 { color : #6F6F6F; font-size : 15px; }
                .space { float : left; width:20px; }
                .footer { border-top: 1px solid #ddd; }
                .rightAlign { text-align: right; }
                .tdBorder {border:solid 1px #000000;}
            ";

                var sautSection = "<div style='height:30px'></div>";
                return sautSection + tableau;

                //if (File.Exists(pathGraph))
                //    File.Delete(pathGraph);

            }
            catch (Exception e)
            {

                throw;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static string GetGraphMif2(List<KeyValue> values)
        {
            try
            {

                var tempWorkDirectory = ConfigurationManager.AppSettings["carmiganc.proposals.tempWorkPath"];
                if (Directory.Exists(tempWorkDirectory))
                    Directory.CreateDirectory(tempWorkDirectory);
                var path = Path.Combine(tempWorkDirectory, Guid.NewGuid() + ".png");
                using (var ch = new Chart())
                {
                    var chArea = new ChartArea();

                    chArea.AxisX.MajorGrid.LineColor = Color.LightGray;
                    chArea.AxisY.MajorGrid.LineColor = Color.LightGray;
                    chArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
                    chArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

                    chArea.AxisY.Minimum = Math.Floor(double.Parse(values.Min(x => x.Value).ToString(CultureInfo.InvariantCulture).Replace(".", ",")));
                    chArea.AxisY.Maximum = Math.Ceiling(double.Parse(values.Max(x => x.Value).ToString(CultureInfo.InvariantCulture).Replace(".", ",")));


                    chArea.AxisY.Title = "Pourcentage";
                    chArea.AxisX.Title = "Année de sortie";

                    ch.Size = new Size(700, 500);
                    ch.ChartAreas.Add(chArea);
                    ch.Palette = ChartColorPalette.None;
                    ch.PaletteCustomColors = CodesGlobalisateurs.Select(x => (Color)x[2]).ToArray();
                    var s = new Series
                    {
                        ChartType = SeriesChartType.Line,


                    };

                    foreach (var pnt in values) s.Points.AddXY(pnt.Key + " an(s)", pnt.Value);

                    ch.Series.Add(s);



                    ch.Series["Series1"].Label = "#VALY %";
                    ch.Series["Series1"].Color = Color.FromArgb(3, 52, 74);
                    ch.Series["Series1"].MarkerStyle = MarkerStyle.Diamond;
                    ch.Series["Series1"].MarkerSize = 12;
                    ch.Series["Series1"].MarkerColor = Color.White;
                    ch.Series["Series1"].MarkerBorderWidth = 3;
                    ch.Series["Series1"].MarkerBorderColor = Color.FromArgb(155, 2, 2);
                    ch.Series["Series1"].BorderWidth = 3;


                    ch.SaveImage(path, ChartImageFormat.Png);
                }
                return path;

            }
            catch (Exception e)
            {

                return string.Empty;
            }

        }
    }
}
