using NutriFitWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest
***REMOVED***
    public class NutritionistModelTest
    ***REMOVED***
        Nutritionist testCase;

        public NutritionistModelTest()
        ***REMOVED***
            testCase = new Nutritionist();
            testCase.NutritionistId = 1;
            testCase.NutritionistFirstName = "TestFirstName";
            testCase.NutritionistLastName = "TestLastName";
            testCase.Gym = null;
            testCase.UserAccountModel = null;
            testCase.Clients = new List<Client>();
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
