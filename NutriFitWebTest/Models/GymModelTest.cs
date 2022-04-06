using NutriFitWeb.Models;
using System.Collections.Generic;
using Xunit;

namespace NutriFitWebTest
***REMOVED***
    public class GymModelTest
    ***REMOVED***
        private readonly Gym testCase;

        public GymModelTest()
        ***REMOVED***
            testCase = new Gym
            ***REMOVED***
                GymId = 1,
                GymName = "TestGymName",
                UserAccountModel = null,
                Clients = new List<Client>(),
                Nutritionists = new List<Nutritionist>(),
                Trainers = new List<Trainer>()
        ***REMOVED***;
    ***REMOVED***

        [Fact]
        public void Gym_TestObjectIsCreated()
        ***REMOVED***
            Assert.NotNull(testCase);
    ***REMOVED***

        [Fact]
        public void Gym_TestGymIdIsCorrect()
        ***REMOVED***
            int expected = 1;

            int actual = testCase.GymId;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Gym_TestGymNameIsCorrect()
        ***REMOVED***
            string expected = "TestGymName";

            string actual = testCase.GymName;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Gym_TestGymUserAccountModelIsCorrect()
        ***REMOVED***
            UserAccountModel expected = null;

            UserAccountModel actual = testCase.UserAccountModel;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Gym_TestGymClientsAreCorrect()
        ***REMOVED***
            int expected = 0;

            int actual = testCase.Clients.Count;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Gym_TestGymNutritionistsAreCorrect()
        ***REMOVED***
            int expected = 0;

            int actual = testCase.Nutritionists.Count;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Gym_TestGymTrainersAreCorrect()
        ***REMOVED***
            int expected = 0;

            int actual = testCase.Trainers.Count;

            Assert.Equal(expected, actual);
    ***REMOVED***
***REMOVED***
***REMOVED***
