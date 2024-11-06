using Microsoft.EntityFrameworkCore;
using SignalRLab3.DataAccess;
using SignalRLab3.Entities;

namespace SignalRLab3.Helper
{
    public interface IDataBuilderService
    {
        Task InitData();
    }
    public class DataBuilderService : IDataBuilderService
    {
        private readonly SignalRLab3DbContext _context;

        public DataBuilderService(SignalRLab3DbContext context)
        {
            _context = context;
        }

        public async Task InitData()
        {
            //create group default
            var group = new Group()
            {
                Id = new Guid("B82F6971-2A22-416F-A74D-0EA03CA4F95C"),
                Name = "Group default"
            };

            var isExistDefaultGroup = await _context.Groups.AnyAsync(g => g.Id == group.Id);
            if(!isExistDefaultGroup)
            {
                await _context.Groups.AddAsync(group);
                await _context.SaveChangesAsync();
            }

            //create user default
            var userDefault = new User()
            {
                Id = new Guid("B3BCDAD2-241A-4590-9A68-08DCFE845D15"),
                FullName = "User default",
                Email = "user.default@gmail.com",
                GroupId = group.Id,
                Password = PasswordHelper.Encrypt("123456")
            };
            var isExistUserDefault = await _context.Users.AnyAsync(u => u.Id == userDefault.Id);
            if (!isExistUserDefault)
            {
                await _context.Users.AddAsync(userDefault);
                await _context.SaveChangesAsync();
            }
        }
    }
}
