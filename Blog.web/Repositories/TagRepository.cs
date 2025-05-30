using Blog.web.Data;
using Blog.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.web.Repositories
{
    public class TagRepository:ITagRepository
    {
        private readonly BlogDbContext blogDbContext;

        public TagRepository(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            //Adds the data to the Tag table
            await blogDbContext.Tags.AddAsync(tag);
            //Savs the data to the database
            await blogDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var exisitng=await blogDbContext.Tags.FindAsync(id);
            if (exisitng != null)
            {
                blogDbContext.Tags.Remove(exisitng);
                await blogDbContext.SaveChangesAsync();
                return exisitng;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
           return await blogDbContext.Tags.ToListAsync();
        }

        public Task<Tag?> GetAsync(Guid id)
        {
            return blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {

            //this checks either there is any tags on the database or not
            //If there exists then store to the existingTag variable
           
            var existingTag = await blogDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                await blogDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }
    }
}
