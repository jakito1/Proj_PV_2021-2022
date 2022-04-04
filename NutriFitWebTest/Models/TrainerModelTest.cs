﻿using NutriFitWeb.Models;
using System.Collections.Generic;
using Xunit;

namespace NutriFitWebTest.Models
{
    public class TrainerModelTest
    {
        Trainer testCase;

        public TrainerModelTest()
        {
            testCase = new Trainer();
            testCase.TrainerId = 1;
            testCase.TrainerFirstName = "TestFirstName";
            testCase.TrainerLastName = "TestLastName";
            testCase.Gym = null;
            testCase.UserAccountModel = null;
            testCase.Clients = new List<Client>();
        }

        [Fact]
        public void Trainer_TestObjectIsCreated()
        {
            Assert.NotNull(testCase);
        }

        [Fact]
        public void Trainer_TestTrainerIdIsCorrect()
        {
            int expected = 1;

            int actual = testCase.TrainerId;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Trainer_TestTrainerFirstNameIsCorrect()
        {
            string expected = "TestFirstName";

            string actual = testCase.TrainerFirstName;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Trainer_TestTrainerLastNameIsCorrect()
        {
            string expected = "TestLastName";

            string actual = testCase.TrainerLastName;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Trainer_TestTrainerGymIsCorrect()
        {
            Gym expected = null;

            Gym actual = testCase.Gym;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Trainer_TestTrainerUserAccountModelIsCorrect()
        {
            UserAccountModel expected = null;

            UserAccountModel actual = testCase.UserAccountModel;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Trainer_TestTrainerClientsAreCorrect()
        {
            int expected = 0;

            int actual = testCase.Clients.Count;

            Assert.Equal(expected, actual);
        }
    }
}
