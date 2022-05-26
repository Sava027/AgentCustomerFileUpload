
namespace AgentCustomer.FileDataAccess
{
    public abstract class BaseRepository
    {
        protected FileDBContext Db { get; set; }
        protected BaseRepository(FileDBContext db)
        {
            Db = db;
        }

    }
}
