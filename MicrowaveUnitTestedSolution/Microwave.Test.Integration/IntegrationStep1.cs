﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IntegrationStep1
    {
        [Test]
        public void TestTest()
        {
            Assert.That(true,Is.EqualTo(true));
        }
    }
}
