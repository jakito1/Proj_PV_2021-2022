﻿using NutriFitWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest
{
    public class ClientModelTest
    {
        Client testCase;
        public ClientModelTest()
        {
            //Setup
            testCase = new Client();
            testCase.ClientId = 1;
            testCase.ClientFirstName = "TestFirstName";
            testCase.ClientLastName = "TestLastName";
            testCase.ClientBirthday = new DateTime(1986, 12, 29);
            testCase.Weight = 70.3;
            testCase.Height = 175;
            testCase.Gym = null;
            testCase.Nutritionist = null;
            testCase.Trainer = null;
            testCase.UserAccountModel = null;
        }
        

        [Fact]
        public void Client_InitializesCorrectly()
        {
            Assert.NotNull(testCase);
        }

        [Fact]
        public void Client_TestClientIdIsCorrect()
        {
            //Arrange
            int expected = 1;
            //Act
            int actual = testCase.ClientId;
            //Assert
            Assert.Equal(expected, actual);

        }
        
        [Fact]
        public void Client_TestClientFirstNameIsCorrect()
        {
            //Arrange
            string expected = "TestFirstName";
            //Act
            string actual = testCase.ClientFirstName;
            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Client_TestClientDoBIsCorrect()
        {
            //Arrange
            DateTime expected = new DateTime(1986, 12, 29);
            //Act
            DateTime? actual = testCase.ClientBirthday;
            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Client_TestClientWeightIsCorrect()
        {
            //Arrange
            double expected = 70.3;
            //Act
            double? actual = testCase.Weight;
            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Client_TestClientHeightIsCorrect()
        {
            double expected = 175;

            double? actual = testCase.Height;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Client_TestClientGymIsCorrect()
        {
            Gym expected = null;

            Gym? actual = testCase.Gym;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Client_TestClientNutritionistIsCorrect()
        {
            Nutritionist expected = null;

            Nutritionist? actual = testCase.Nutritionist;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Client_TestClientTrainerIsCorrect()
        {
            Trainer expected = null;

            Trainer? actual = testCase.Trainer;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Client_TestClientUserAccountModelIsCorrect()
        {
            UserAccountModel expected = null;

            UserAccountModel actual = testCase.UserAccountModel;

            Assert.Equal(expected, actual);
        }
    }
}
