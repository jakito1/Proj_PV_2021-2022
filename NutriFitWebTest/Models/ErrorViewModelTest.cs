using NutriFitWeb.Models;
using Xunit;

namespace NutriFitWebTest.Models
{
    public class ErrorViewModelTest
    {
        ErrorViewModel testCase;

        public ErrorViewModelTest()
        {
            testCase = new ErrorViewModel();
            testCase.RequestId = "TestRequestId";
        }

        [Fact]
        public void ErrorViewModel_TestObjectIsCreated()
        {
            Assert.NotNull(testCase);
        }

        [Fact]
        public void ErrorViewModel_TestRequestIdIsCorrect()
        {
            string expected = "TestRequestId";

            string actual = testCase.RequestId;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ErrorViewModel_TestShowRequestIdReturnsTrueOnRequestIdPresent()
        {
            bool expected = true;

            bool actual = testCase.ShowRequestId;

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ErrorViewModel_TestShowRequestIdReturnsFalseOnRequestIdNotPresent()
        {
            testCase.RequestId = "";
            bool expected = false;

            bool actual = testCase.ShowRequestId;

            Assert.Equal(expected, actual);
        }

    }
}
