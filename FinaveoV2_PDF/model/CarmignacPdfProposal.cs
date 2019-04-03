using FinaveoV2_PDF.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaveoV2_PDF
{
    public enum TypeGestion
    {
        [Description("Gestion libre")]
        GL,
        [Description("Gestion conseillée")]
        GC
    }

    public enum Profil
    {
        [Description("Prudent")]
        Pru = 0,
        [Description("Equilibré")]
        Equ = 1,
        [Description("Dynamique")]
        Dyn = 2
    }

    public enum CodeGlobalisateur
    {
        ACT,
        CASH,
        CERTIF,
        DROITS,
        FDS_EUR,
        OBL,
        OPC_ACT,
        OPC_ALT,
        OPC_DIV,
        OPC_EUR,
        OPC_FOR,
        OPC_FORA,
        OPC_FORD,
        OPC_FORT,
        OPC_MON,
        OPC_OBL,
        OPC_SPE,
        OPCVM,
        OTHERS
    }
    /// <summary>
    /// 
    /// </summary>
    //public enum ProposalTransmissionType
    //{
    //    /// <summary>
    //    /// Internet - Internet (All electronic => CGP and Client both have internet access)
    //    /// </summary>
    //    InternetInternet,
    //    /// <summary>
    //    /// Internet - Papier (Semi electronic => CGP has internet access, client doesn't have)
    //    /// </summary>
    //    InternetPapier
    //}

    /// <summary>
    /// 
    /// </summary>
    //public enum ProposalStatus
    //{
    //    /// <summary>
    //    /// Draft
    //    /// </summary>
    //    [Description("Sauvegardée")]
    //    Sauvegardee,

    //    /// <summary>
    //    /// Send to the client
    //    /// </summary>
    //    [Description("Envoyée au client")]
    //    EnvoyeeAuClient,
    //    /// <summary>
    //    /// Saved and printed
    //    /// </summary>
    //    [Description("Sauvegardée et imprimée")]
    //    SauvegardeeEtImprimee,

    //    /// <summary>
    //    /// Refused by the client
    //    /// </summary>
    //    [Description("Refusée par le client")]
    //    RefuseeParClient,

    //    /// <summary>
    //    /// Refused by the client
    //    /// </summary>
    //    [Description("Acceptée par le client")]
    //    AccepteeParClient,

    //    /// <summary>
    //    /// Send to the depositaire
    //    /// </summary>
    //    [Description("Envoyée au dépositaire")]
    //    EnvoyeeAuDepositaire,

    //    /// <summary>
    //    /// Executed by the super admin
    //    /// </summary>
    //    [Description("Exécutée")]
    //    Executee,

    //    /// <summary>
    //    /// Expiré
    //    /// </summary>
    //    [Description("Expirée")]
    //    Expiree,

    //    /// <summary>
    //    /// Annulé
    //    /// </summary>
    //    [Description("Annulée")]
    //    Annulee,

    //    /// <summary>
    //    /// Signed but not by all clients
    //    /// </summary>
    //    [Description("Signée partiellement")]
    //    SigneePartiellement,
    //}

    /// <summary>
    /// 
    /// </summary>
    public enum ProposalOrdersType
    {
        [Description("Non défini")]
        NonDefini = 0,
        [Description("Achat/Souscription")]
        AchatSouscription = 1,
        [Description("Vente/Rachat")]
        VenteRachat = 2,
        [Description("Arbitrage")]
        Arbitrage = 3,
        [Description("Achete/Vendu")]
        AcheteVendu = 4
    }

    public enum ReglementType
    {
        cheque,
        virement,
        especes,
        assurance,
        arbitrage
    }


    /// <summary>
    /// Describes the type of a order 
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// SOUSCRIPTION = Achat
        /// </summary>
        [Description("Souscription")]
        Souscription = 0,
        /// <summary>
        /// RACHAT = vendre
        /// </summary>
        [Description("Rachat")]
        Rachat = 1,
        /// <summary>
        /// Arbitrage
        /// </summary>
        [Description("Arbitrage")]
        Arbitrage = 2,
        /// <summary>
        /// Arbitrage
        /// </summary>
        [Description("A/V")]
        AcheteVendu = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    //public enum OrderStatus
    //{
    //    /// <summary>
    //    /// Draft
    //    /// </summary>
    //    [Description("Sauvegardé")]
    //    Sauvegarde,

    //    /// <summary>
    //    /// Send to the client
    //    /// </summary>
    //    [Description("Envoyé au client")]
    //    EnvoyeAuClient,
    //    /// <summary>
    //    /// Saved and printed
    //    /// </summary>
    //    [Description("Sauvegardé et imprimé")]
    //    SauvegardeEtImprime,

    //    /// <summary>
    //    /// Refused by the client
    //    /// </summary>
    //    [Description("Refusé par le client")]
    //    RefuseParClient,

    //    /// <summary>
    //    /// Refused by the client
    //    /// </summary>
    //    [Description("Accepté par le client")]
    //    AccepteParClient,

    //    /// <summary>
    //    /// Send to the depositaire
    //    /// </summary>
    //    [Description("Envoyé au dépositaire")]
    //    EnvoyeAuDepositaire,

    //    /// <summary>
    //    /// Executed by the super admin
    //    /// </summary>
    //    [Description("Exécuté")]
    //    Execute,

    //    /// <summary>
    //    /// Expiré
    //    /// </summary>
    //    [Description("En cours")]
    //    EnCours,

    //    /// <summary>
    //    /// Expiré
    //    /// </summary>
    //    [Description("Expiré")]
    //    Expire,

    //    /// <summary>
    //    /// Annulé
    //    /// </summary>
    //    [Description("Annulé")]
    //    Annule,

    //    /// <summary>
    //    /// None
    //    /// </summary>
    //    None,

    //    /// <summary>
    //    /// Annulé
    //    /// </summary>
    //    [Description("Signé partiellement")]
    //    SignePartiellement,
    //}

    /// <summary>
    /// 
    /// </summary>
    //public enum SortDirection
    //{
    //    Ascending = 0,
    //    Descending = 1
    //}

    /// <summary>
    /// 
    /// </summary>
    //public enum EnvelopeType
    //{
    //    Bancking = 0,
    //    Insurance = 1,
    //    All = 2
    //}

    /// <summary>
    /// 
    /// </summary>
    //public enum ProductType
    //{
    //    Opcvm = 0,
    //    Titre = 1,
    //    Emtn = 2
    //}

    /// <summary>
    /// 
    /// </summary>
    public enum PersonType
    {
        /// <summary>
        /// personne physique
        /// </summary>
        [Description("PP")]
        Physique,
        /// <summary>
        /// Morale
        /// </summary>
        [Description("PM")]
        Morale
    }

    /// <summary>
    /// 
    /// </summary>
    public enum CivilityType
    {
        /// <summary>
        /// personne physique
        /// </summary>
        [Description("Monsieur")]
        Monsieur,
        /// <summary>
        /// Morale
        /// </summary>
        [Description("Madame")]
        Madame,
        /// <summary>
        /// Morale
        /// </summary>
        [Description("Monsieur ou Madame")]
        MonsieurOuMadame,

    }

    //public enum ValueUnit
    //{
    //    [Description("Amount")]
    //    Amount,
    //    [Description("Percent")]
    //    Percent,
    //    [Description("")]
    //    None
    //}

    /// <summary>
    /// 
    /// </summary>
    //public enum ProposalProofChoices
    //{
    //    ChangementGestion,
    //    DynamisationPortefeuille,
    //    SecurisationPlusValue,
    //    Autre
    //}

    //public enum TypePdfDocument
    //{
    //    PROP_ARB_CARM = 0,
    //    JUSTIF_PROP = 1
    //}
    public class CarmignacPdfProposal
    {
        public CarmignacPdfProposal()
        {
        }


        public Guid IdProposal;
        /// Client
        public PersonType TypeClient; //PP ou PM
        public string Client; //raison sociale pour les PM, [prénom nom] pour les PP
        public string Mandataire; //pour les PM uniquement
        public string CodeClient;
        public CivilityType Titre; // "Monsieur" ,"Madame" , "Monsieur ou Madame"

        /// Proposition
        public DateTime DateProposition; // Date de création de la proposition
        public string NumeroProposition; // Numéro de la paroposition
        public ProposalOrdersType TypeOrdre; // Type "Souscription","Rachat", "Acheté Vendu", "Arbitrage"

        /// Compte
        public string TypeEnveloppe; // Enveloppe : Compte titre, PEA, PEA/PME, Assurance Vie...
        public string NumeroCompte; // Numéro du compte
        public TypeGestion Gestion; // type de gestion  "Gestion conseiller", "Gestion libre"

        ///CGP
        public string Cgp; //[prénom nom]
        public string CodeCgp;// Code du CGP
        public string TelCGP;// uméro de téléphone du CGP
        public string NumeroPosteTelephonique;

        ///Portefeuille actuel
        public double risqueMoyenActuel; // risque moyen du portefeuille actuel
        public List<CurrentPortfolioLine> PortefeuilleActuel; // toutes les lignes de supports + liquidités du portefeuille actuel
        public Profil ProfilActuel; // profil du portefeuille actuel : "Prudent", "Dynamique"...
        public List<RepartitionActifElement> RepartitionActuelleClassesActifs;
        public double ValoTotaleActuelle;// valorisation totale du portefeuille actuel
        public char SymboleDeviseValoTotaleActuelle; // Devise de la valorisation totale de portefeuille actuel (example '€','£','$'...)
        public double PMVTotale; // Plus/Moins value latente totale du portefeuille actuel
        public char SymboleDevisePMVTotale; // Devise de la plus/moins value latente du portefeuille actuel (example '€','£','$'...)
        public double PMVPourcentMoyen;// Plus/Moins value totale moyenne (en %)

        ///Portefeuille proposé
        public double risqueMoyenPropose; // risque moyen du portefeuille proposé
        public List<ProposedPortfolioLine> PortefeuillePropose;  // toutes les lignes de supports + liquidités du portefeuille proposé
        public Profil ProfilPropose; // profil du portefeuille proposé : "Prudent", "Dynamique"...
        public Profil ProfilKYC; // profil du dans le KYC
        public List<RepartitionActifElement> RepartitionProposeeClassesActifs;
        public double ValoTotaleProposee;// valorisation totale du portefeuille proposé
        public char SymboleDeviseValoTotaleProposee;// Devise de la valorisation totale du portefeuille proposé (example '€','£','$'...)

        public bool DepassementProfil;//indique s'il y a dépassement de profil

        ///Justification
        public string Objectif; //un des 4 objectifs à cocher
        public string Motif; //texte justificatif

        //Ordres à exécuter
        public List<OrderToExecute> Ordres;// liste des ordres à exécuter
        public double SommeAchats; // somme de tous les ordres d'achats de la proposition
        public char SymboleDeviseSommeAchats;// Devise de la somme de toutes les ordres d'achats de la proposition (example '€','£','$'...)
        public double SommeVentes;// somme de tous les ordres de ventes de la proposition
        public char SymboleDeviseSommeVentes;// Devise de la somme de toutes les ordres de ventes de la proposition (example '€','£','$'...)
        public double SommeAchatsVentes;//  somme de tous les ordres de ventes et d'achats de la proposition
        public char SymboleDeviseSommeAchatsVentes;// Devise de la somme de toutes les ordres de ventes et d'achats de la proposition (example '€','£','$'...)
        public double FraisGlobaux;// Montant des frais globaux saisis
        public char SymboleDeviseFraisGlobaux; // Devise ou pourcentage (selon la valeur de la propriété [FraisGlobauxPourcent] ) des frais globaux saisis (example '%','€','£','$'...) 
        public double FraisGlobauxPourcent; //entre 0 (en devise ) et 1 (en pourcentage)

        //public List<string> CheminsDICI; //chemins des DICI sur le réseau

        public ReglementType ReglementAchat; // type de règlement "chèque, virement,..."
        public ReglementType ReglementVente; // type de règlement "chèque, virement,..."
        public string NumeroAssuranceVie; //si reglementVente==assurance, alors remplir ce champs
        public double montantReglementAchat;// montant du règlement de l'achat en '€'
        public double montantReglementVente;// montant du règlement de la vente en '€'
        public string Divers;

        public string VilleSignature; // 
        public DateTime DateSignature; // Date de signature de la proposition

        public string ReceptionCourrier; // vide

        public Guid? IdProposalAttachment;

        public Guid? IdProposalRib;

        public PdfMif2Frais FraisMif2 { get; set; }

        public bool IsRachatPartiel { get; set; }
    }
    public class CurrentPortfolioLine
    {
        public string LibelleValeur; // Libellé du support
        public string Isin; // code isin du support
        public double Quantite; // quantité
        public double Cours; // cours 
        public char SymboleDeviseCours; // Devise du cours ('€','$',...)
        public double Valo; // valorisation du support dans le portefeuille
        public char SymboleDeviseValo; // Devise de la valorisation support 
        public DateTime DateValo; // Date de valorisation 
        public double Pam; // PAM
        public char SymboleDevisePam; // Devise du PAM ('€','$',...)
        public double Pmv; // PMV (Plus/moins value latent) 
        public char SymboleDevisePmv;// Devise ou Symbole de la PMV  ('%','€','$',...)
        public double PmvPourcentage; //Pourcentage du PMV
        public double Poids; // Poids de la ligne du support dans le portefeuille 
    }
    public class RepartitionActifElement
    {
        public CodeGlobalisateur Code { get; set; }

        public double Value { get; set; }
    }
    public class ProposedPortfolioLine
    {
        public string LibelleValeur;// Libellé du support
        public string Isin; // Code isin du support
        public double Quantite; // Quantité
        public double Cours; // Cours
        public char SymboleDeviseCours; // Devise du cours ('$','€',..)
        public double ValoEstimee;// Valorisation de la ligne 
        public char SymboleDeviseValoEstimee; // Devise de la valorisation estimée ('$','€',..) 
        public double Poids; //Poids de la ligne (en %) dans le portefeuille
    }
    public class OrderToExecute
    {
        public string Sens; //A ou V
        public string LibelleValeur;// Libellé du support
        public string Isin; // code isin du support
        public double Quantite;// Quantité
        public double Cours; // cours du support
        public char SymboleDeviseCours; // Devise du support ('€','$',...)
        public double Montant;// Montant de la ligne 
        public char SymboleDeviseMontant;// devise du montant de la ligne
        public double PourcentagePtf; //pourcentage de la ligne
        public string SelectedValueType; //"Quantity", "Percent","Amount","LinePercent"
    }
}
