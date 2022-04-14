using NutriFitWeb.Models;
using System.Collections.Generic;
using Xunit;

namespace NutriFitWebTest.Models
{
    public class NutritionistModelTest
    {
        private readonly Nutritionist testCase;

        public NutritionistModelTest()
        {
            testCase = new Nutritionist
            {
                NutritionistId = 1,
                NutritionistFirstName = "TestFirstName",
                NutritionistLastName = "TestLastName",
                Gym = null,
                UserAccountModel = null,
                Clients = new List<Client>()
            };
        }

        [Fact]
        public void Nutritionist_TestObjectIsCreated()
        {
            Assert.NotNull(testCase);
        }

        [Fact]
        public void Nutritionist_TestNutritionistIdIsCorrect()
        {
            int expected = 1;

            int actual = testCase.NutritionistId;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Nutritionist_TestNutritionistFirstNameIsCorrect()
        {
            string expected = "TestFirstName";

            string? actual = testCase.NutritionistFirstName;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Nutritionist_TestNutritionistLastNameIsCorrect()
        {
            string expected = "TestLastName";

            string? actual = testCase.NutritionistLastName;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Nutritionist_TestNutritionistGymIsCorrect()
        {
            Gym? expected = null;

            Gym? actual = testCase.Gym;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Nutritionist_TestNutritionistUserAccountModelIsCorrect()
        {
            UserAccountModel? expected = null;

            UserAccountModel? actual = testCase.UserAccountModel;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Nutritionist_TestNutritionistClientsAreCorrect()
        {
            int expected = 0;

            int? actual = testCase.Clients?.Count;

            Assert.Equal(expected, actual);
        }
    }
}
