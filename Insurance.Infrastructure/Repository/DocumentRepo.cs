using AutoMapper;
using Insurance.Application.DTO.Claim;
using Insurance.Application.DTO.Document;
using Insurance.Application.Interface;
using Insurance.Domain.Models;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Infrastructure.Repository
{
    public class DocumentRepo : IDocumentRepo
    {
        ApplicationDbContext db;
        IMapper mapper;

        public DocumentRepo(ApplicationDbContext db,
                            IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<List<DocumentDTO>> GetAll()
        {
            var data = await db.Documents
                .Include(x => x.Claim)
                .Include(x => x.Customer)
                .Include(x => x.Policy)
                .ToListAsync();

            return mapper.Map<List<DocumentDTO>>(data);
        }

        public async Task<DocumentDTO> GetById(int id)
        {
            var data = await db.Documents
                .Include(x => x.Claim)
                .Include(x => x.Customer)
                .Include(x => x.Policy)
                .FirstOrDefaultAsync(x => x.DocumentId == id);

            return mapper.Map<DocumentDTO>(data);
        }

        public async Task<string> Add(DocumentCreateDTO dto)
        {
            var document = mapper.Map<Document>(dto);

            document.Status = "Pending";
            document.UploadDate = DateTime.Now;
            document.CreatedAt = DateTime.Now;

            await db.Documents.AddAsync(document);

            await db.SaveChangesAsync();

            return "Document Added Successfully";
        }

        public async Task<string> Update(int id, DocumentCreateDTO dto)
        {
            var document = await db.Documents.FindAsync(id);

            if (document == null)
                return "Document Not Found";

            document.ClaimId = dto.ClaimId;
            document.CustomerId = dto.CustomerId;
            document.PolicyId = dto.PolicyId;
            document.EntityType = dto.EntityType;
            document.FileName = dto.FileName;
            document.FilePath = dto.FilePath;
            document.ModifiedAt = DateTime.Now;

            await db.SaveChangesAsync();

            return "Document Updated Successfully";
        }

        public async Task<string> Delete(int id)
        {
            var document = await db.Documents.FindAsync(id);

            if (document == null)
                return "Document Not Found";

            db.Documents.Remove(document);

            await db.SaveChangesAsync();

            return "Document Deleted Successfully";
        }
        public async Task<DocumentDTO> DownloadDocument(int id)
        {
            var data = await db.Documents
                .FirstOrDefaultAsync(x => x.DocumentId == id);

            if (data == null)
                return null;

            return mapper.Map<DocumentDTO>(data);
        }
        public async Task<string> UploadCustomerKyc(CustomerKycDTO dto)
        {
            Document document = new Document()
            {
                CustomerId = dto.CustomerId,
                PolicyId = dto.PolicyId,

                EntityType = "CustomerKYC",

                FileName = dto.FileName,
                FilePath = dto.FilePath,

                Status = "Pending",

                UploadDate = DateTime.Now,
                CreatedAt = DateTime.Now,

                CreatedBy = dto.CreatedBy
            };

            await db.Documents.AddAsync(document);

            await db.SaveChangesAsync();

            return "Customer KYC Uploaded Successfully";
        }

        public async Task<string> UploadAgentKyc(AgentKycDTO dto)
        {
            Document document = new Document()
            {
                CustomerId = dto.CustomerId,
                PolicyId = dto.PolicyId,

                EntityType = "AgentKYC",

                FileName = dto.FileName,
                FilePath = dto.FilePath,

                Status = "Pending",

                UploadDate = DateTime.Now,
                CreatedAt = DateTime.Now,

                CreatedBy = dto.CreatedBy
            };

            await db.Documents.AddAsync(document);

            await db.SaveChangesAsync();

            return "Agent KYC Uploaded Successfully";
        }

        public async Task<List<DocumentDTO>> GetAllKycDocuments()
        {
            var data = await db.Documents .Where(x =>x.EntityType == "CustomerKYC" || x.EntityType == "AgentKYC").ToListAsync();

            return mapper.Map<List<DocumentDTO>>(data);
        }

        public async Task<List<DocumentDTO>> GetKycByCustomerId(int customerId)
        {
            var data = await db.Documents.Where(x => x.CustomerId == customerId).ToListAsync();

            return mapper.Map<List<DocumentDTO>>(data);
        }

        public async Task<List<DocumentDTO>> GetKycByAgentId(int agentId)
        {
            var data = await db.Documents.Where(x => x.CreatedBy == agentId && x.EntityType == "AgentKYC") .ToListAsync();

            return mapper.Map<List<DocumentDTO>>(data);
        }
    }
}

