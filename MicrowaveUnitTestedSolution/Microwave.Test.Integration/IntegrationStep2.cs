using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IntegrationStep2
    {
        [TestFixture]
        public class IT2_DisplayPowertube
        {
            private Display _display;
            private PowerTube _powerTube;
            private IOutput _output;
            private ITimer _timer;
            private CookController _cookController;

            [SetUp]
            public void SetUp()
            {
                _timer = Substitute.For<ITimer>();
                _output = Substitute.For<IOutput>();
                _display = new Display(_output);
                _powerTube = new PowerTube(_output);
                _cookController = new CookController(_timer, _display, _powerTube);
            }
            [Test]
            public void Dosomething()
            {
                _timer.Start(2);
                _cookController.StartCooking();
            }
        }
        [Test]
        public void TestTest()
        {
            Assert.That(true,Is.EqualTo(true));
        }
    }
}
