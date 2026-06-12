using Insurance.Application.DTO.Claim;
using Insurance.Application.DTO.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.Interface
{
    public interface IDocumentRepo
    {
        Task<List<DocumentDTO>> GetAll();

        Task<DocumentDTO> GetById(int id);

        Task<string> Add(DocumentCreateDTO dto);

        Task<string> Update(int id, DocumentCreateDTO dto);

        Task<string> Delete(int id);
        Task<DocumentDTO> DownloadDocument(int id);

        Task<string> UploadCustomerKyc(CustomerKycDTO dto);

        Task<string> UploadAgentKyc(AgentKycDTO dto);

        Task<List<DocumentDTO>> GetAllKycDocuments();

        Task<List<DocumentDTO>> GetKycByCustomerId(int customerId);

        Task<List<DocumentDTO>> GetKycByAgentId(int agentId);
    }
}
