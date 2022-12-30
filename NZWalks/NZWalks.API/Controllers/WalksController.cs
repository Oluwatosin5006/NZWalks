using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            // Fetch Data from database - Domain Walks
            var walksDomain = await walkRepository.GetAllAsync();

            // Convert Domin Walks to DTO Walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            // Return response
            return Ok(walksDTO);

        }


        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            // Get Walk Domain object from database
            var walkDomain = await walkRepository.GetAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            // Convert Domain object to DTO (map the var walkDomain with/to the DTO)
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // Return response
            return Ok(walkDTO);
        }


        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            // Convert the request(DTO) to Domain Object
            var walkDomain = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId


            };

            // Pass Domain Object to Repository
            walkDomain = await walkRepository.AddAsync(walkDomain);

            // Convert the Domain Object back to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            // Send DTO Responses back to Client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // Convert the DTO to Domain model
            var walkDomain = new Models.Domain.Walk()
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
                
            };

            // Update Walk using Repository
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            //  if Null, then NotFound
            if (walkDomain == null)
            {
                return NotFound();
            }

            // Convert Domain Data back to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            // Return OK response
            return Ok(walkDTO);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            // Get the walk from the database to Delete
            var walkDomain = await walkRepository.DeleteAsync(id);

            // If we don't get a walk (null), return NotFound
            if (walkDomain == null)
            {
                return NotFound();
            }

            // if we found something, we convert the response to DTO region
            
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            //            OR
            //var walkDTO = new Models.DTO.Walk
            //{
            //    Id = walkDomain.Id,
            //    Length = walkDomain.Length,
            //    Name = walkDomain.Name,
            //    RegionId = walkDomain.RegionId,
            //    WalkDifficultyId = walkDomain.WalkDifficultyId,
            //};

            // return OK response
            return Ok(walkDTO);
        }
    }
}
