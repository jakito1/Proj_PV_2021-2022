using NutriFitWeb.Models;
using System.Collections.Generic;
using Xunit;

namespace NutriFitWebTest.Models
{
    public class GymModelTest
    {
        private readonly Gym testCase;

        public GymModelTest()
        {
            testCase = new Gym
            {
                GymId = 1,
                GymName = "TestGymName",
                UserAccountModel = null,
                Clients = new List<Client>(),
                Nutritionists = new List<Nutritionist>(),
                Trainers = new List<Trainer>()
            };
        }

        [Fact]
        public void Gym_TestObjectIsCreated()
        {
            Assert.NotNull(testCase);
        }

        [Fact]
        public void Gym_TestGymIdIsCorrect()
        {
            int expected = 1;

            int actual = testCase.GymId;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Gym_TestGymNameIsCorrect()
        {
            string expected = "TestGymName";

            string actual = testCase.GymName;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Gym_TestGymUserAccountModelIsCorrect()
        {
            UserAccountModel expected = null;

            UserAccountModel actual = testCase.UserAccountModel;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Gym_TestGymClientsAreCorrect()
        {
            int expected = 0;

            int actual = testCase.Clients.Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Gym_TestGymNutritionistsAreCorrect()
        {
            int expected = 0;

            int actual = testCase.Nutritionists.Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Gym_TestGymTrainersAreCorrect()
        {
            int expected = 0;

            int actual = testCase.Trainers.Count;

            Assert.Equal(expected, actual);
        }
    }
}
