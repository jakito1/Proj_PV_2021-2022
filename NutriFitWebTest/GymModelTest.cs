using NutriFitWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest
{
    public class GymModelTest
    {
        Gym testCase;

        public GymModelTest()
        {
            testCase = new Gym();
            testCase.GymId = 1;
            testCase.GymName = "TestGymName";
            testCase.UserAccountModel = null;
            testCase.Clients = new List<Client>();
            testCase.Nutritionists = new List<Nutritionist>();
            testCase.Trainers = new List<Trainer>();
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
