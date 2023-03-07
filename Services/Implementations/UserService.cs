using Abstractions.Services;
using Abstractions.UnitOfWork;
using AutoMapper;
using Dtos;
using Dtos.InputDtos;
using Entities.Entities;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Services.Implementations
{
    public class UserService : IUserService
    {
        #region Variables
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IRepository<User> _userRepository => _unitOfWork.GetRepository<User>();
        #endregion

        #region Constructor
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Public methods
        public List<UserDto> GetAll()
        {
            return _userRepository.GetAll().Select(x => _mapper.Map<UserDto>(x)).ToList();
        }

        /// <summary>
        /// Get user info by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
        {
            var userToCreate = _mapper.Map<User>(userDto);
            userToCreate.Password= BCryptNet.HashPassword(userDto.Password);
            userToCreate.CreatedAt= DateTime.Now;

            var user = await _userRepository.InsertAndGetAsync(userToCreate);

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Check email exists in the system or not?
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> IsExistEmailAsync(string email)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Email.Equals(email));

            return user != null;
        }

        /// <summary>
        /// User authentication
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UserDto?> AuthenticateAsync(LoginDto dto)
        {
            var userToVerify = await _userRepository.GetFirstOrDefaultAsync(x => x.Email == dto.Email);

            // validate password
            if (userToVerify == null || !BCryptNet.Verify(dto.Password, userToVerify.Password))
                return null;

            return _mapper.Map<UserDto>(userToVerify);
        }
        #endregion
    }
}
