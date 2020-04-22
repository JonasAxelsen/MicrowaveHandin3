using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IntegrationStep2
    {
        private ICookController _cookController;
        private IOutput _fakeOutput;
        private ITimer _fakeTimer;
        private IDisplay _display;
        private IPowerTube _powerTube;

        [SetUp]
        public void Setup()
        {
            _fakeOutput = Substitute.For<Output>();
            _fakeTimer = Substitute.For<Timer>();
            _display = new Display(_fakeOutput);
            _powerTube = new PowerTube(_fakeOutput);
            _cookController = new CookController(_fakeTimer, _display, _powerTube);
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(50)]
        public void CookController_StartCooking_ChargingStartedWithCorrectParam(int power)
        { 
            _cookController.StartCooking(power, 10);
            _fakeOutput.Received(1).OutputLine($"PowerTube works with {power}");
        }

        [Test]
        public void CookController_StartCooking_ChargingAlreadyStarted()
        {
            _cookController.StartCooking(10, 10);

            Assert.That(() => _cookController.StartCooking(10, 10), Throws.TypeOf<ApplicationException>());
        }

        [TestCase(0)]
        [TestCase(101)]
        public void CookController_StartCooking_ChargingDidNotStart(int power)
        {
            Assert.That(() => _cookController.StartCooking(power, 10), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
