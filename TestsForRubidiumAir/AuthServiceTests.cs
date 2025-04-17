using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rubidium;

namespace AuthServiceTests
{
    [TestClass]
    public class AuthServiceTests
    {
        private Mock<IUserRepo> _mockUserRepo;
        private Mock<IPasswordHasher> _mockPasswordHasher;
        private AuthService _authService;

        [TestInitialize]
        public void Setup()
        {
            _mockUserRepo = new Mock<IUserRepo>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _authService = new AuthService(_mockUserRepo.Object, _mockPasswordHasher.Object);
        }

        [TestMethod]
        public void Register_NewUser_ReturnsTrueAndSavesUser()
        {
            string username = "testUser";
            string password = "securePass123";
            string hashedPassword = "hashedSecurePass123";

            _mockUserRepo
                .Setup(repo => repo.GetByUsername(username))
                .Returns((User)null);

            _mockPasswordHasher
                .Setup(hasher => hasher.HashPassword(password))
                .Returns(hashedPassword);

            bool result = _authService.Register(username, password);

            Assert.IsTrue(result);
            _mockUserRepo.Verify(repo => repo.Add(It.Is<User>(u =>
                u.Username == username &&
                u.PasswordHash == hashedPassword)), Times.Once);
            _mockUserRepo.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        public void Register_ExistingUser_ReturnsFalse()
        {
            string username = "existingUser";
            string password = "password123";

            _mockUserRepo
                .Setup(repo => repo.GetByUsername(username))
                .Returns(new User { Username = username });

            bool result = _authService.Register(username, password);

            Assert.IsFalse(result);
            _mockUserRepo.Verify(repo => repo.Add(It.IsAny<User>()), Times.Never);
            _mockUserRepo.Verify(repo => repo.Save(), Times.Never);
            _mockPasswordHasher.Verify(hasher => hasher.HashPassword(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Login_ValidCredentials_ReturnsUser()
        {
            string username = "admin";
            string password = "correctPass";
            var expectedUser = new User { Username = username };

            _mockUserRepo
                .Setup(repo => repo.GetByUsername(username))
                .Returns(expectedUser);

            _mockUserRepo
                .Setup(repo => repo.ValidateCredentials(username, password))
                .Returns(true);

            var result = _authService.Login(username, password);

            Assert.AreEqual(expectedUser, result);
        }

        [TestMethod]
        public void Login_InvalidPassword_ReturnsNull()
        {
            string username = "admin";
            string password = "wrongPass";
            var existingUser = new User { Username = username };

            _mockUserRepo
                .Setup(repo => repo.GetByUsername(username))
                .Returns(existingUser);

            _mockUserRepo
                .Setup(repo => repo.ValidateCredentials(username, password))
                .Returns(false);

            var result = _authService.Login(username, password);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Login_NonExistentUser_ReturnsNull()
        {
            string username = "nonExistent";
            string password = "anyPassword";

            _mockUserRepo
                .Setup(repo => repo.GetByUsername(username))
                .Returns((User)null);

            var result = _authService.Login(username, password);

            Assert.IsNull(result);
            _mockUserRepo.Verify(repo => repo.ValidateCredentials(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        //[TestMethod]
        //public void FailingTest_ForVerification_ThisShouldFail()
        //{
        //    string username = "shouldFail";
        //    string password = "wrongPass";

        //    _mockUserRepo
        //        .Setup(repo => repo.GetByUsername(username))
        //        .Returns(new User { Username = username });

        //    var result = _authService.Login(username, password);

        //    Assert.IsNotNull(result, "Этот тест ДОЛЖЕН падать, так как мы не настроили ValidateCredentials");
        //}
    }
}