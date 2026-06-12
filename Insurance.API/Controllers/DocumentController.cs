using Insurance.Application.DTO.Claim;
using Insurance.Application.DTO.Document;
using Insurance.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
      IDocumentRepo repo;

        public DocumentController(IDocumentRepo repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await repo.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await repo.GetById(id);

            if (data == null)
                return NotFound("Document Not Found");

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Add(DocumentCreateDTO dto)
        {
            return Ok(await repo.Add(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,DocumentCreateDTO dto)
        {
            return Ok(await repo.Update(id, dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await repo.Delete(id));
        }
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var document = await repo.DownloadDocument(id);

            if (document == null)
                return NotFound("Document Not Found");

            if (!System.IO.File.Exists(document.FilePath))
                return NotFound("File Not Found");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(document.FilePath);

            var fileName = document.FileName;

            return File(fileBytes,"application/octet-stream",fileName);
        }
        [HttpPost("customer-kyc")]
        public async Task<IActionResult> UploadCustomerKyc( CustomerKycDTO dto)
        {
            return Ok(await repo.UploadCustomerKyc(dto));
        }

        [HttpPost("agent-kyc")]
        public async Task<IActionResult> UploadAgentKyc( AgentKycDTO dto)
        {
            return Ok(await repo.UploadAgentKyc(dto));
        }

        [HttpGet("kyc")]
        public async Task<IActionResult> GetAllKyc()
        {
            return Ok(await repo.GetAllKycDocuments());
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerKyc( int customerId)
        {
            return Ok( await repo.GetKycByCustomerId(customerId));
        }

        [HttpGet("agent/{agentId}")]
        public async Task<IActionResult> GetAgentKyc(int agentId)
        {
            return Ok( await repo.GetKycByAgentId(agentId));
        }
    }
}
