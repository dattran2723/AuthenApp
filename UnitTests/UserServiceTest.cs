using Abstractions.UnitOfWork;
using AutoMapper;
using Entities.Entities;
using Moq;
using Services.Implementations;

namespace UnitTests
{
    public class UserServiceTest
    {
        private Mock<IRepository<User>> _userRepo = new Mock<IRepository<User>>();
        private Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
        private Mock<IMapper> _mapper = new Mock<IMapper>();
        private UserService _userService;


        public UserServiceTest()
        {
            _userService = new UserService(_unitOfWork.Object, _mapper.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnUsers_WhenUserExists()
        {
            //Arrange
            var fakeUsers = new List<User>
            {
                new User
                {
                    Id = 1,
                    Email = "vandat@gmail.com",
                    FirstName = "Tran",
                    LastName = "Dat",
                    Password = "password",
                },
                new User
                {
                    Id = 2,
                    Email = "vandat2@gmail.com",
                    FirstName = "Tran",
                    LastName = "Dat",
                    Password = "password",
                },
                new User
                {
                    Id = 3,
                    Email = "vandat3@gmail.com",
                    FirstName = "Tran",
                    LastName = "Dat",
                    Password = "password",
                }
            };

            _unitOfWork.Setup(x => x.GetRepository<User>()).Returns(_userRepo.Object);
            _unitOfWork.Setup(x => x.GetRepository<User>().GetAll()).Returns(fakeUsers.AsQueryable());

            // Act
            var users = _userService.GetAll();

            // Assert
            Assert.Equal(fakeUsers.Count, users.Count);

            //Sorry, I haven't performed a unit test before. :D
        }

        //[Fact]
        //public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
        //{
        //    //Arrange
        //    
        //    var fakeUser = new User
        //    {
        //        Id = 1,
        //        Email = "vandat@gmail.com",
        //        FirstName = "Tran",
        //        LastName = "Dat",
        //        Password = "password",
        //    };

        //    var userId = 1;

        //    _unitOfWork.Setup(x => x.GetRepository<User>()).Returns(_userRepo.Object);
        //    _unitOfWork.Setup(x => x.GetRepository<User>().GetAsync(userId)).ReturnsAsync(fakeUser);

        //    // Act

        //    var user = await _userService.GetByIdAsync(1);

        //    // Assert
        //    Assert.Equal(userId, user.Id);
        //    Assert.Equal(fakeUser.Email, user.Email);
        //    Assert.Equal(fakeUser.FirstName, user.FirstName);
        //    Assert.Equal(fakeUser.LastName, user.LastName);
        //}

        //[Fact]
        //public async Task CreateUser_ShouldReturnUser_WhenSuccess()
        //{
        //    //Arrange
        //    var fakeUser = new User
        //    {
        //        Id = 1,
        //        Email = "vandat@gmail.com",
        //        FirstName = "Tran",
        //        LastName = "Dat",
        //        Password = "password",
        //    };

        //    _unitOfWork.Setup(x => x.GetRepository<User>()).Returns(_userRepo.Object);
        //    _unitOfWork.Setup(x => x.GetRepository<User>().InsertAndGetAsync(fakeUser)).ReturnsAsync(fakeUser);
        //    _unitOfWork.Setup(x => x.Complete()).Returns(1);

        //    // Act
        //    var user = await _userService.GetByIdAsync(1);

        //    // Assert
        //    Assert.Equal(fakeUser.Id, user.Id);
        //    Assert.Equal(fakeUser.Email, user.Email);
        //    Assert.Equal(fakeUser.FirstName, user.FirstName);
        //    Assert.Equal(fakeUser.LastName, user.LastName);
        //}
    }
}