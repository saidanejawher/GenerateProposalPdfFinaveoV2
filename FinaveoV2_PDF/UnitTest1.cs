using System;
using System.Collections.Generic;
using System.IO;
using FinaveoV2_PDF.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinaveoV2_PDF
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void BuildFinaveo_V2_PDF()
        {
            //var proposal = propositionExemple2();
            //var result = new PdfBuilderService().BuildCarmignacPdfProposal(proposal);
            //Assert.IsNotNull(result.Result.Content);
        }
        [TestMethod]
        public void Build_Test()
        {
            var proposal = propositionExemple2();
            var documentPath = Path.Combine(@"c:\Temp\Uploads\Clients", "PROP_ARB_CARM", $"Test-App-{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.pdf");
            var result =  BuildPdfFinaveoV2.Build_Pdf_Finaveo_V2(documentPath, proposal);
        }
        private CarmignacPdfProposal propositionExemple2()
        {
            var proposition = new CarmignacPdfProposal();
            var la = new CurrentPortfolioLine() { LibelleValeur = "Liquidités", Isin = "", Quantite = 1, Cours = 0, SymboleDeviseCours = '€', Valo = 3000, SymboleDeviseValo = '€', DateValo = DateTime.Now, Pam = 0, SymboleDevisePam = '€', Pmv = 0, SymboleDevisePmv = '€', PmvPourcentage = 0, Poids = 23.8 }; var la2 = new CurrentPortfolioLine() { LibelleValeur = "CARMIGNAC EURO-ENTREPRENEURS", Isin = "FR0010149112", Quantite = 36, Cours = 277.7, SymboleDeviseCours = '€', Valo = 1000, SymboleDeviseValo = '€', DateValo = DateTime.Now, Pam = 300, SymboleDevisePam = '€', Pmv = 0, SymboleDevisePmv = '€', PmvPourcentage = 0, Poids = 76.92 };
            //////////////// //////////////// 
            var lp = new ProposedPortfolioLine() { LibelleValeur = "CARMIGNAC EURO-ENTREPRENEURS", Isin = "FR0010149112", Quantite = 37, Cours = 277.7, SymboleDeviseCours = '€', ValoEstimee = 10274.9, SymboleDeviseValoEstimee = '€', Poids = 79.04, };
            var lp2 = new ProposedPortfolioLine() { LibelleValeur = "Liquidités", Isin = "", Quantite = 1, Cours = 0, SymboleDeviseCours = '€', ValoEstimee = 2725.1, SymboleDeviseValoEstimee = '€', Poids = 20.96, }; //////////////// //////////////// 
            var o = new OrderToExecute() { Sens = "Souscription", LibelleValeur = "CARMIGNAC EURO-ENTREPRENEURS", Isin = "FR0010149112", Quantite = 1, Cours = 0, SymboleDeviseCours = '$', Montant = 274.9, SymboleDeviseMontant = '€', SelectedValueType = "Amount" }; ///////////////// 
            proposition.IdProposal = new Guid("8D6A33AD-321D-4BA5-BCA0-F35E12B22892");
            proposition.PortefeuilleActuel = new List<CurrentPortfolioLine>(); proposition.PortefeuillePropose = new List<ProposedPortfolioLine>(); proposition.Ordres = new List<OrderToExecute>(); proposition.PortefeuilleActuel.Add(la); // 
            proposition.PortefeuilleActuel.Add(la2); // 
            proposition.PortefeuillePropose.Add(lp);// 
            proposition.PortefeuillePropose.Add(lp2);// 
            proposition.Ordres.Add(o);// 
                                      /* 
                                      proposition.Titre = ""; 
                                      proposition.TypeClient = "";*/
            proposition.Client = "Georges DURAND";//; 
            proposition.ProfilActuel = (Profil)2; //
            proposition.ProfilPropose = (Profil)2;// 
            proposition.ProfilKYC = (Profil)0;
            proposition.Gestion = (TypeGestion)1;
            proposition.NumeroCompte = "30000068261";// 
            proposition.TypeEnveloppe = "Plan d'épargne en actions";// 
            proposition.Cgp = "Carmignac Admin"; // 
            proposition.TelCGP = "+261334519009";// 
            proposition.CodeCgp = "";// 
            proposition.NumeroProposition = "1"; // 
            proposition.DateProposition = DateTime.Now;
            proposition.Objectif = "";// 
            proposition.Motif = "";// 
            proposition.Mandataire = @"AOUNI WALID";
            proposition.DepassementProfil = true;// 
            proposition.risqueMoyenActuel = 4.06;
            proposition.risqueMoyenPropose = 4.16;
            ////proposition.RepartitionActuelleClassesActifs = new Dictionary<CodeGlobalisateur, double> { { CodeGlobalisateur.ACT, 0.12 }, { CodeGlobalisateur.CASH, 0.26 }, { CodeGlobalisateur.OPC_FORT, 0 }, { CodeGlobalisateur.CERTIF, 0.36 }, { CodeGlobalisateur.DROITS, 0.1 }, { CodeGlobalisateur.FDS_EUR, 0.16 } }; 
            proposition.RepartitionActuelleClassesActifs = new List<RepartitionActifElement>() { new RepartitionActifElement { Code = CodeGlobalisateur.FDS_EUR, Value = 35d }, new RepartitionActifElement { Code = CodeGlobalisateur.OPCVM, Value = 55d } };// //
            //proposition.RepartitionProposeeClassesActifs = new Dictionary<CodeGlobalisateur, double> { { CodeGlobalisateur.ACT, 0.43 }, { CodeGlobalisateur.CASH, 0.06 }, { CodeGlobalisateur.CERTIF, 0.05 }, { CodeGlobalisateur.DROITS, 0.09 }, { CodeGlobalisateur.OBL, 0.05 }, { CodeGlobalisateur.OPCVM, 0.27 }, { CodeGlobalisateur.OPC_ACT, 0.05 } }; 
            proposition.RepartitionProposeeClassesActifs = new List<RepartitionActifElement>() { new RepartitionActifElement { Code = CodeGlobalisateur.OBL, Value = 10d }, new RepartitionActifElement { Code = CodeGlobalisateur.CERTIF, Value = 10d }, new RepartitionActifElement { Code = CodeGlobalisateur.CASH, Value = 10d }, new RepartitionActifElement { Code = CodeGlobalisateur.OPC_DIV, Value = 10d }, new RepartitionActifElement { Code = CodeGlobalisateur.DROITS, Value = 10d }, new RepartitionActifElement { Code = CodeGlobalisateur.FDS_EUR, Value = 35d }, new RepartitionActifElement { Code = CodeGlobalisateur.OPCVM, Value = 55d } };// //
            proposition.ValoTotaleActuelle = 13000;// 
            proposition.SymboleDeviseValoTotaleActuelle = '€';// 
            proposition.PMVTotale = 0;// 
            proposition.SymboleDevisePMVTotale = '€';// 
            proposition.PMVPourcentMoyen = -433.33333333333331;//
            proposition.ValoTotaleProposee = 123456999.26;// 
            proposition.SymboleDeviseValoTotaleProposee = '€';// 
            proposition.SommeAchats = 274.9;// 
            proposition.SymboleDeviseSommeAchats = '€';// 
            proposition.SommeVentes = 0; proposition.SymboleDeviseSommeVentes = '€';
            proposition.SommeAchatsVentes = 274.9;// 
            proposition.SymboleDeviseSommeAchatsVentes = '€';// 
            proposition.FraisGlobaux = 123.45;
            proposition.SymboleDeviseFraisGlobaux = '€';// 
            proposition.FraisGlobauxPourcent = 0.0252;// 
            proposition.Objectif = "Les taux allemands et américains se sont détendus sur la période dans le sillage d’une hausse des risques politiques aux Etats-Unis et géopolitiques avec la Corée du Nord. Dans ce contexte, nos positions vendeuses sur les taux cœurs européens, en particulier allemands et français, ont pénalisé la performance. Toutefois, le Fonds a non seulement tiré parti de ses emprunts d’Etat émergents, notamment argentins et brésiliens, mais aussi de ses emprunts périphériques européens. Au sein de notre composante crédit, notre exposition aux secteurs de la finance et de l’énergie ainsi que notre crédit structuré ont enregistré des performances positives. Par ailleurs, l’euro s’est apprécié au cours du mois face au dollar bénéficiant du dynamisme de l’économie européenne. Ainsi sur la partie devises, notre sous-pondération au billet vert a nettement soutenu la performance relative du Fonds. Nous disposons d’une sensibilité aux taux d’intérêt proche de zéro, d’une exposition élevée à l’euro au détriment du dollar et d’un poste de liquidités significatif.";
            proposition.Motif = "Dynamisation du portefeuille; Sécurisation de la plus value";
            proposition.ReglementAchat = ReglementType.arbitrage;
            proposition.ReglementVente = ReglementType.arbitrage;
            proposition.IdProposalRib = new Guid("103E9AAD-9C7D-4064-9C32-2C42BF4489F3");
            proposition.FraisMif2 = new model.PdfMif2Frais()
            {
                CdeaDirectMontantCoutEtFraisServiceInvestEtAux = 105,
                CdeaDirectMontantCoutsLiesAuxTransactions = 2354,
                CdeaDirectMontantCoutsMarginaux = 656,
                CdeaDirectMontantCoutsServicesAuxiliaires = 5454,
                CdeaDirectMontantFraisRecurrents = 5751,
                CdeaDirectMontantFraisUniques = 65434,
                CdeaDirectMontantLiesAuxPaiementsRecus = 545,
                CdeaDirectPourcentageCoutEtFraisServiceInvestEtAux = 5,
                CdeaDirectPourcentageCoutsLiesAuxTransactions = 8,
                CdeaDirectPourcentageCoutsMarginaux = 1,
                CdeaDirectPourcentageCoutsServicesAuxiliaires = 4,
                CdeaDirectPourcentageFraisRecurrents = 8,
                CdeaDirectPourcentageFraisUniques = 5,
                CdeaDirectPourcentageLiesAuxPaiementsRecus = 7,
                CdeaIndirectMontantCoutsMarginaux = 875,
                CdeaIndirectMontantCoutsServicesAuxiliaires = 6540.564m,
                CdeaIndirectMontantFraisRecurrents = 256.33m,
                CdeaIndirectMontantFraisUniques = 56456.51m,
                CdeaIndirectMontantLiesAuxInstruFinanciers = 977.00m,
                CdeaIndirectMontantLiesAuxTransactions = 54254.12m,
                CdeaIndirectPourcentageCoutsMarginaux = 11,
                CdeaIndirectPourcentageCoutsServicesAuxiliaires = 1.26m,
                CdeaIndirectPourcentageFraisRecurrents = 3.11m,
                CdeaIndirectPourcentageFraisUniques = 7.5m,
                CdeaIndirectPourcentageLiesAuxInstruFinanciers = 5.5m,
                CdeaIndirectPourcentageLiesAuxTransactions = 1.3m,
                CdeaMontantCoutsEtFraisTotaux = 4878,
                CdeaPourcentageCoutsEtFraisTotaux = 4.36m,
                EcraDureeDetentionRecommandee = "5 ans",
                EcraMontantCoutsEtFraisEstimes = 1478.12m,
                EcraPourcentageCoutsEtFraisEstimes = 6.1m,
                MontantArbitrage = 13643435.12m,
                TableauGrapheEffetRendement = new List<KeyValue>() { new KeyValue() { Key = 2, Value = 10.7m }, new KeyValue() { Key = 3, Value = 9.2m }, new KeyValue() { Key = 4, Value = 7.2m }, new KeyValue() { Key = 5, Value = 6.9m }, new KeyValue() { Key = 6, Value = 5.0m } },
            };
            return proposition;
        }
    }
}
