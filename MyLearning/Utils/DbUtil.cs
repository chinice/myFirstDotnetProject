using System.Threading.Tasks;

namespace MyLearning.Utils
{
    public class DbUtil
    {
        public async Task<bool> SaveChanges(MyLearningDbContext myLearningDbContext)
        {
            var isSaved = await myLearningDbContext.SaveChangesAsync();
            if (isSaved == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}