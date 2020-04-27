using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

//+--------+------+---------------+-------+-------------+------------+--------------------+---------+----------------+-----------+-------+--------+
//| Step # | Door | UserInterface | Light | PowerButton | TimeButton | Start-CancelButton | Display | CookController | PowerTube | Timer | Output |
//+--------+------+---------------+-------+-------------+------------+--------------------+---------+----------------+-----------+-------+--------+
//| 3      |  T   |      X        |   X   |     S       |     S      |         S          |    X    |       S        |           |       |   S    |
//+--------+------+---------------+-------+-------------+------------+--------------------+---------+----------------+-----------+-------+--------+

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IntegrationStep3_Door  
    {
        private Door _door;
        private UserInterface _userInterface;
        private Light _light;
        private Display _display;
        private IOutput _output;
        private ICookController _cookController;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;

        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _cookController = Substitute.For<ICookController>();

            _door = new Door();
            _light = new Light(_output);
            _display = new Display(_output);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        #region Test throws

        [Test]
        public void UserInterfaceReadyState_Open_NoThrow()
        {
            Assert.That(() => _door.Open(), Throws.Nothing);
        }

        [Test]
        public void UserInterfaceOpenState_Open_NoThrow()
        {
            _door.Open();
            Assert.That(() => _door.Open(), Throws.Nothing);
        }

        [Test]
        public void UserInterfacePowerState_Open_NoThrow()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(() => _door.Open(), Throws.Nothing);
        }

        [Test]
        public void UserInterfaceTimeState_Open_NoThrow()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(() => _door.Open(), Throws.Nothing);
        }

        [Test]
        public void UserInterfaceCookingState_Open_NoThrow()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(() => _door.Open(), Throws.Nothing);
        }

        [Test]
        public void UserInterfaceReadyState_Close_NoThrow() 
        {
            Assert.That(() => _door.Close(), Throws.Nothing);
        }

        [Test]
        public void UserInterfaceOpenState_Close_NoThrow()
        {
            _door.Open();

            Assert.That(() => _door.Close(), Throws.Nothing);
        }

        [Test]
        public void UserInterfacePowerState_Close_NoThrow()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(() => _door.Close(), Throws.Nothing);
        }

        [Test]
        public void UserInterfaceTimeState_Close_NoThrow()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(() => _door.Close(), Throws.Nothing);
        }

        [Test]
        public void UserInterfaceCookingState_Close_NoThrow()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(() => _door.Close(), Throws.Nothing);
        }

        #endregion

        #region Test for right output

        [Test]
        public void UserInterfaceReadyState_Open_OutputRecievesCorrektString()
        {
            _door.Open();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
        }

        [Test]
        public void UserInterfacePowerState_Open_OutputRecievesCorrektString()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _door.Open();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Display cleared")));
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
        }

        [Test]
        public void UserInterfaceTimeState_Open_OutputRecievesCorrektString()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _door.Open();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Display cleared")));
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
        }

        [Test]
        public void UserInterfaceCookingState_Open_OutputRecievesCorrektString()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _door.Open();

            _cookController.Received(1).Stop();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Display cleared"))); //Lavede fejl i koden
        }

        [Test]
        public void UserInterfaceOpenState_OpenClose_OutputRecievesCorrektString()
        {
            _door.Open();
            _door.Close();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
        }

        [Test]
        public void Door_Close_OutputRecievesNoCall()
        {
            _door.Close();

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }


        #endregion

    }
}
