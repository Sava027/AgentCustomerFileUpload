using AgentCustomer.Files;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AgentCustomer.FileDataAccess
{
    public class FileRepository : BaseRepository, IFileRepository
    {
        private readonly IMapper _mapper;
        public FileRepository(FileDBContext db, IMapper mapper) : base(db)
        {
            _mapper = mapper;
        }

        public void AddTracking(TrackingInfo info)
        {
            var tracking = _mapper.Map<FileTracking>(info);

            Db.FileTrackings.Add(tracking);
        }

        public async Task UpdateTrackingStatus(long id, FileTrackingInfoStatus fileTrackingStatus)
        {
            var trackingDb = await Db.FileTrackings
                  .SingleOrDefaultAsync(x => x.FileTrackingId == id);

            if (trackingDb == null)
                return;

            trackingDb.Status = fileTrackingStatus.GetDescription();

            await SaveChangesAsync();

        }

        public void AddFileInfo(CustomerFile customerFile)
        {
            var fileInfo = _mapper.Map<FileInfo>(customerFile);

            Db.FileInfos.Add(fileInfo);
        }

        public async Task<TrackingInfo> GetTracking(string pulicId)
        {
            var result = await Db.FileTrackings
                .SingleOrDefaultAsync(x => x.PulicTrackingId == pulicId);

            if (result == null)
                return null;

            var mappedResult = _mapper.Map<TrackingInfo>(result);
            return mappedResult;

        }

        public async Task<IEnumerable<CustomerFile>> GetFilesByAgentAndCustomer(string agentId, string customerId)
        {
            throw new NotImplementedException();
        }    

        // General 
        public async Task<int> SaveChangesAsync()
        {
            //TODO pass in context user name
            return await Db.SaveChanges("DefaultUser");
        }
    }
}
