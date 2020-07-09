using System;
using System.Collections.Generic;
using Xunit;
using MoviesAPI.Models;

namespace unit_tests
{
    public class ValidationTests
    {

        [Fact]
        public void TestValidateID()
        {
            Assert.True(Validation.ValidateID(4));
            Assert.False(Validation.ValidateID(0));
            Assert.False(Validation.ValidateID(-2));
        }
    }
}
