using NutriFitWeb.Models;
using System.Collections.Generic;
using Xunit;

namespace NutriFitWebTest.Models
***REMOVED***
    public class TrainerModelTest
    ***REMOVED***
        private readonly Trainer testCase;

        public TrainerModelTest()
        ***REMOVED***
            testCase = new Trainer
            ***REMOVED***
                TrainerId = 1,
                TrainerFirstName = "TestFirstName",
                TrainerLastName = "TestLastName",
                Gym = null,
                UserAccountModel = null,
                Clients = new List<Client>()
        ***REMOVED***;
    ***REMOVED***

        [Fact]
        public void Trainer_TestObjectIsCreated()
        ***REMOVED***
            Assert.NotNull(testCase);
    ***REMOVED***

        [Fact]
        public void Trainer_TestTrainerIdIsCorrect()
        ***REMOVED***
            int expected = 1;

            int actual = testCase.TrainerId;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Trainer_TestTrainerFirstNameIsCorrect()
        ***REMOVED***
            string expected = "TestFirstName";

            string? actual = testCase.TrainerFirstName;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Trainer_TestTrainerLastNameIsCorrect()
        ***REMOVED***
            string expected = "TestLastName";

            string? actual = testCase.TrainerLastName;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Trainer_TestTrainerGymIsCorrect()
        ***REMOVED***
            Gym? expected = null;

            Gym? actual = testCase.Gym;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Trainer_TestTrainerUserAccountModelIsCorrect()
        ***REMOVED***
            UserAccountModel? expected = null;

            UserAccountModel? actual = testCase.UserAccountModel;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Trainer_TestTrainerClientsAreCorrect()
        ***REMOVED***
            int expected = 0;

            int? actual = testCase.Clients?.Count;

            Assert.Equal(expected, actual);
    ***REMOVED***
***REMOVED***
***REMOVED***
