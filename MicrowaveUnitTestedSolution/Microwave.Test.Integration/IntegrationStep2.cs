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
        private IUserInterface _fakeInterface;
        private IPowerTube _powerTube;

        [SetUp]
        public void Setup()
        {
            _fakeOutput = Substitute.For<IOutput>();
            _fakeTimer = Substitute.For<ITimer>();
            _display = new Display(_fakeOutput);
            _powerTube = new PowerTube(_fakeOutput);
            _fakeInterface = Substitute.For<IUserInterface>();
            _cookController = new CookController(_fakeTimer, _display, _powerTube, _fakeInterface);
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(50)]
        public void CookController_StartCooking_CookingStartedWithCorrectParam(int power)
        {
            // ACT
            _cookController.StartCooking(power, 10);

            // ASSERT
            _fakeOutput.Received(1).OutputLine($"PowerTube works with {power}");
        }

        [Test]
        public void CookController_StartCooking_CookingAlreadyStarted()
        {
            // ACT
            _cookController.StartCooking(10, 10);

            // ASSERT
            Assert.That(() => _cookController.StartCooking(10, 10), Throws.TypeOf<ApplicationException>());
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(101)]
        public void CookController_StartCooking_CookingDidNotStart(int power)
        {
            Assert.That(() => _cookController.StartCooking(power, 10), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CookController_OnTimerExpired_CookingStoppedByExpiredEvent()
        {
            _cookController.StartCooking(10, 10);

            // ACT
            _fakeTimer.Expired += Raise.Event();

            // ASSERT
            _fakeOutput.Received(1).OutputLine($"PowerTube turned off");
        }

        [TestCase(100000)]
        public void CookCOntroler_OnTimerick_DlayRecievedCorrectParamsAtCookingStart(int time) // ERROR DETECTED: time has to be formatted to secs before being used
        {
            int mins = ((time-1000)/1000) / 60; //Subtract 1000 to emulate timer-tick
            int secs = ((time-1000)/1000) % 60;

            _fakeTimer.TimeRemaining.Returns(time-1000); // Fake return value for private property

            _cookController.StartCooking(10, time);

            // ACT
            _fakeTimer.TimerTick += Raise.Event();

            //ASSERT
            _fakeOutput.Received(1).OutputLine($"Display shows: {mins:D2}:{secs:D2}");
        }
    }
}
