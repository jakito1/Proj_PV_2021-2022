using Moq;
using NutriFitWeb.Models;
using System;
using Xunit;

namespace NutriFitWebTest.Models
***REMOVED***
    public class ClientModelTest
    ***REMOVED***
        private readonly Client testCase;
        public ClientModelTest()
        ***REMOVED***
            //Setup
            testCase = new Client
            ***REMOVED***
                ClientId = 1,
                ClientFirstName = "TestFirstName",
                ClientLastName = "TestLastName",
                ClientBirthday = new DateTime(1986, 12, 29),
                Weight = 70.3,
                Height = 175,
                Gym = null,
                Nutritionist = null,
                Trainer = null,
                UserAccountModel = null
        ***REMOVED***;
    ***REMOVED***


        [Fact]
        public void Client_InitializesCorrectly()
        ***REMOVED***
            Assert.NotNull(testCase);
    ***REMOVED***

        [Fact]
        public void Client_TestClientIdIsCorrect()
        ***REMOVED***
            //Arrange
            int expected = 1;
            //Act
            int actual = testCase.ClientId;
            //Assert
            Assert.Equal(expected, actual);

    ***REMOVED***

        [Fact]
        public void Client_TestClientFirstNameIsCorrect()
        ***REMOVED***
            //Arrange
            string expected = "TestFirstName";
            //Act
            string? actual = testCase.ClientFirstName;
            //Assert
            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Client_TestClientDoBIsCorrect()
        ***REMOVED***
            //Arrange
            DateTime expected = new DateTime(1986, 12, 29);
            //Act
            DateTime? actual = testCase.ClientBirthday;
            //Assert
            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Client_TestClientWeightIsCorrect()
        ***REMOVED***
            //Arrange
            double expected = 70.3;
            //Act
            double? actual = testCase.Weight;
            //Assert
            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Client_TestClientHeightIsCorrect()
        ***REMOVED***
            double expected = 175;

            double? actual = testCase.Height;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Client_TestClientGymIsCorrect()
        ***REMOVED***
            Gym? expected = null;

            Gym? actual = testCase.Gym;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Client_TestClientNutritionistIsCorrect()
        ***REMOVED***
            Nutritionist? expected = null;

            Nutritionist? actual = testCase.Nutritionist;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Client_TestClientTrainerIsCorrect()
        ***REMOVED***
            Trainer? expected = null;

            Trainer? actual = testCase.Trainer;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Client_TestClientUserAccountModelIsCorrect()
        ***REMOVED***
            UserAccountModel? expected = null;

            UserAccountModel? actual = testCase.UserAccountModel;

            Assert.Equal(expected, actual);
    ***REMOVED***
***REMOVED***
***REMOVED***
