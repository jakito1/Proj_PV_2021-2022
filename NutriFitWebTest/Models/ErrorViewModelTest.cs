using NutriFitWeb.Models;
using Xunit;

namespace NutriFitWebTest.Models
***REMOVED***
    public class ErrorViewModelTest
    ***REMOVED***
        private readonly ErrorViewModel testCase;

        public ErrorViewModelTest()
        ***REMOVED***
            testCase = new ErrorViewModel
            ***REMOVED***
                RequestId = "TestRequestId"
        ***REMOVED***;
    ***REMOVED***

        [Fact]
        public void ErrorViewModel_TestObjectIsCreated()
        ***REMOVED***
            Assert.NotNull(testCase);
    ***REMOVED***

        [Fact]
        public void ErrorViewModel_TestRequestIdIsCorrect()
        ***REMOVED***
            string expected = "TestRequestId";

            string? actual = testCase.RequestId;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void ErrorViewModel_TestShowRequestIdReturnsTrueOnRequestIdPresent()
        ***REMOVED***
            bool expected = true;

            bool actual = testCase.ShowRequestId;

            Assert.Equal(expected, actual);
    ***REMOVED***

        [Fact]
        public void ErrorViewModel_TestShowRequestIdReturnsFalseOnRequestIdNotPresent()
        ***REMOVED***
            testCase.RequestId = "";
            bool expected = false;

            bool actual = testCase.ShowRequestId;

            Assert.Equal(expected, actual);
    ***REMOVED***

***REMOVED***
***REMOVED***
