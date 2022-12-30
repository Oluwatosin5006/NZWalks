﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;


namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;
         
        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //Assign new ID
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await nZWalksDbContext.Walks.FindAsync(id);

            if (existingWalk == null)
            {
                return null;
            }

            // Delete the region
            nZWalksDbContext.Walks.Remove(existingWalk);
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await 
                nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync(); 
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            var walk = await nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
            return walk;
        }


        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            //var existingWalk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            //if (existingWalk == null)
            //{
            //    return null;
            //}


            //existingWalk.Name = walk.Name;
            //existingWalk.Length = walk.Length;
            //existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
            //existingWalk.RegionId = walk.RegionId;

            //await nZWalksDbContext.SaveChangesAsync();

            //return existingWalk;

            //OR

            var existingWalk = await nZWalksDbContext.Walks.FindAsync(id);

            if (existingWalk != null)
            {
                existingWalk.Name = walk.Name;
                existingWalk.Length = walk.Length;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;

                await nZWalksDbContext.SaveChangesAsync();

                return existingWalk;
                
            }

            return null;

        }
    }
}
