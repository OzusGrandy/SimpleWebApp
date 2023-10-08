using SimpleWebApp.CommonModels;
using SimpleWebApp.Storage.Models.Projects;

namespace SimpleWebApp.BusinessLogic.Project
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static Project FromEntityModel(DatabaseProject entity)
        {
            return new Project
            {
                Id = Guid.Parse(entity.Id),
                Name = entity.Name,
                Description = entity.Description,
                CreatedAt = CommonMethods.ConvertToDateTime(entity.CreatedAt),
                UpdatedAt = CommonMethods.ConvertToDateTime(entity.UpdatedAt)
            };
        }
    }
}
