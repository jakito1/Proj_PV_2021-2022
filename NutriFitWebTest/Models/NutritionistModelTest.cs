using NutriFitWeb.Models;
using System.Collections.Generic;
using Xunit;

namespace NutriFitWebTest.Models
***REMOVED***
    public class NutritionistModelTest
    ***REMOVED***
        private readonly Nutritionist testCase;

        public NutritionistModelTest()
        ***REMOVED***
            testCase = new Nutritionist
            ***REMOVED***
                NutritionistId = 1,
                NutritionistFirstName = "TestFirstName",
                NutritionistLastName = "TestLastName",
                Gym = null,
                UserAccountModel = null,
                Clients = new List<Client>()
        ***REMOVED***;
    ***REMOVED***

        [Fact]
        public void Nutritionist_TestObjectIsCreated()
        ***REMOVED***
            Assert.NotNull(testCase);
    ***REMOVED***

        [Fact]
        public void Nutritionist_TestNutritionistIdIsCorrect()
        ***REMOVED***
            int expected = 1;

            int actual = testCase.NutritionistId;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Nutritionist_TestNutritionistFirstNameIsCorrect()
        ***REMOVED***
            string expected = "TestFirstName";

            string actual = testCase.NutritionistFirstName;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Nutritionist_TestNutritionistLastNameIsCorrect()
        ***REMOVED***
            string expected = "TestLastName";

            string actual = testCase.NutritionistLastName;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Nutritionist_TestNutritionistGymIsCorrect()
        ***REMOVED***
            Gym expected = null;

            Gym actual = testCase.Gym;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Nutritionist_TestNutritionistUserAccountModelIsCorrect()
        ***REMOVED***
            UserAccountModel expected = null;

            UserAccountModel actual = testCase.UserAccountModel;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void Nutritionist_TestNutritionistClientsAreCorrect()
        ***REMOVED***
            int expected = 0;

            int actual = testCase.Clients.Count;

            Assert.Equal(expected, actual);
    ***REMOVED***
***REMOVED***
***REMOVED***
