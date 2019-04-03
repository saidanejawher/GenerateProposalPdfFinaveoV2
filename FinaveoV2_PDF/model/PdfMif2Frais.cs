using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaveoV2_PDF.model
{
    public class PdfMif2Frais
    {
        // tableau côut détaillé ex-ante
        public decimal MontantArbitrage { get; set; }

        // coûts directs
        public decimal CdeaDirectMontantCoutEtFraisServiceInvestEtAux { get; set; }
        public decimal CdeaDirectMontantFraisUniques { get; set; }
        public decimal CdeaDirectMontantFraisRecurrents { get; set; }
        public decimal CdeaDirectMontantCoutsLiesAuxTransactions { get; set; }
        public decimal CdeaDirectMontantCoutsServicesAuxiliaires { get; set; }
        public decimal CdeaDirectMontantCoutsMarginaux { get; set; }
        public decimal CdeaDirectMontantLiesAuxPaiementsRecus { get; set; }
        public decimal CdeaMontantCoutsEtFraisTotaux { get; set; }

        public decimal CdeaDirectPourcentageCoutEtFraisServiceInvestEtAux { get; set; }
        public decimal CdeaDirectPourcentageFraisUniques { get; set; }
        public decimal CdeaDirectPourcentageFraisRecurrents { get; set; }
        public decimal CdeaDirectPourcentageCoutsLiesAuxTransactions { get; set; }
        public decimal CdeaDirectPourcentageCoutsServicesAuxiliaires { get; set; }
        public decimal CdeaDirectPourcentageCoutsMarginaux { get; set; }
        public decimal CdeaDirectPourcentageLiesAuxPaiementsRecus { get; set; }
        public decimal CdeaPourcentageCoutsEtFraisTotaux { get; set; }
        // coûts indirects
        public decimal CdeaIndirectMontantLiesAuxInstruFinanciers { get; set; }
        public decimal CdeaIndirectMontantFraisUniques { get; set; }
        public decimal CdeaIndirectMontantFraisRecurrents { get; set; }
        public decimal CdeaIndirectMontantLiesAuxTransactions { get; set; }
        public decimal CdeaIndirectMontantCoutsServicesAuxiliaires { get; set; }
        public decimal CdeaIndirectMontantCoutsMarginaux { get; set; }


        public decimal CdeaIndirectPourcentageLiesAuxInstruFinanciers { get; set; }
        public decimal CdeaIndirectPourcentageFraisUniques { get; set; }
        public decimal CdeaIndirectPourcentageFraisRecurrents { get; set; }
        public decimal CdeaIndirectPourcentageLiesAuxTransactions { get; set; }
        public decimal CdeaIndirectPourcentageCoutsServicesAuxiliaires { get; set; }
        public decimal CdeaIndirectPourcentageCoutsMarginaux { get; set; }

        // tableau : effet cumulé sur le rendement annuel : ecra
        public string EcraDureeDetentionRecommandee { get; set; }
        public decimal EcraPourcentageCoutsEtFraisEstimes { get; set; }
        public decimal EcraMontantCoutsEtFraisEstimes { get; set; }

        public List<KeyValue> TableauGrapheEffetRendement { get; set; }

    }

    public class KeyValue
    {
        public int Key { get; set; }

        public decimal Value { get; set; }
    }
}
