using FinaveoV2_PDF.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaveoV2_PDF
{
    public class PdfBuilderService
    {
        private const string TypeDocumentBsCarmignac = @"PROP_ARB_CARM";
        private string BsRootPath
        {
            get { return ConfigurationManager.AppSettings.Get("upsideo.bs.rootpath"); }
            set { }
        }

        public IMethodResult<ProposalDocumentDto> BuildCarmignacPdfProposal(CarmignacPdfProposal proposal)
        {
            try
            {
                //_logger.LogTrace(new LogMessage()
                //{
                //    Date = DateTime.Now,
                //    Message = JsonConvert.SerializeObject(proposal),
                //    WebMethod = MethodBase.GetCurrentMethod().Name,
                //    Label = "Automatic Trace"
                //});

                var documentPath = Path.Combine(BsRootPath, @"Uploads\Clients");
                //var account = _accountRepository.GetAccount(proposal.NumeroCompte);
                var account = "Account_Number";
                ProposalDocumentDto result = null;
                if (account != null)
                {


                    documentPath = Path.Combine(documentPath, TypeDocumentBsCarmignac, $"{TypeDocumentBsCarmignac}-{account/*.numcompte*/}-{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.pdf");
                    //var isOk = documentPath.CreateDirectoriesFromPath();
                    var targetDirectory = Path.Combine(Path.Combine(BsRootPath, @"Uploads\Clients"), TypeDocumentBsCarmignac);
                    if (Directory.Exists(targetDirectory))
                    {
                        // generate PDF bytes
                        result = new ProposalDocumentDto
                        {
                            Content = proposal.BuildCarmignacPdfProposal(),
                            Path = string.Empty
                        };
                        File.WriteAllBytes(documentPath, result.Content);
                    }
                    else
                    {
                        //_logger.LogError(new LogMessage()
                        //{
                        //    Date = DateTime.Now,
                        //    Message = $@"Le répertoire [{targetDirectory}] n'existe pas",
                        //    Label = @"Répertoire manquant",
                        //    WebMethod = MethodBase.GetCurrentMethod().Name
                        //});

                    }

                }

                //return result
                return new MethodResult<ProposalDocumentDto>()
                {
                    Result = result
                };
            }
            catch (Exception e)
            {
               

                return new MethodResult<ProposalDocumentDto>()
                {
                    IsExecutionOk = false,
                    Error = new ExecutionError()
                    {
                        Message = e.Message,
                        Code = "ERROR"
                    },
                    Result = null
                };
            }
        }



    }
}
