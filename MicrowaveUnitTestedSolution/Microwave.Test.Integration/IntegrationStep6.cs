using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IntegrationStep6
    {
        private Light _light;
        private Display _display;
        private ICookController _cookController;
        private IOutput _output;
        private IButton _powerButton;
        private IButton _timeButton;
        private Button _startCancelButton;
        private IDoor _door;

        private UserInterface _UserInterface;

        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _cookController = Substitute.For<ICookController>();
            _light = new Light(_output);
            _display = new Display(_output);

            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = new Button();

            _door = Substitute.For<IDoor>();

            _UserInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void StartCancelPressed_InSETPOWERState_NOTLightTurnOff() // Negative test because invisible error in code which should have been found in unit test of Userinterface.cs
        {
            // Arrange
            _powerButton.Pressed += Raise.Event();
            _output.ClearReceivedCalls();

            // Act
            _startCancelButton.Press();

            // Assert
            _output.DidNotReceive().OutputLine($"Light is turned off");
        }

        [Test]
        public void StartCancelPressed_InSETPOWERState_DisplayClear()
        {
            // Arrange
            _powerButton.Pressed += Raise.Event();
            _output.ClearReceivedCalls();

            // Act
            _startCancelButton.Press();

            // Assert
            _output.Received(1).OutputLine($"Display cleared");
        }

        [Test]
        public void StartCancelPressed_InSETTIMEState_LightTurnOn()
        {
            // Arrange
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _output.ClearReceivedCalls();

            // Act
            _startCancelButton.Press();

            // Assert
            _output.Received(1).OutputLine($"Light is turned on");
        }

        [Test]
        public void StartCancelPressed_InCOOKINGState_LightTurnOff()
        {
            // Arrange
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _startCancelButton.Press();
            _output.ClearReceivedCalls();

            // Act
            _startCancelButton.Press();

            // Assert
            _output.Received(1).OutputLine($"Light is turned off");
        }

        [Test]
        public void StartCancelPressed_InCOOKINGState_DisplayClear()
        {
            // Arrange
            _powerButton.Pressed += Raise.Event();
            _timeButton.Pressed += Raise.Event();
            _startCancelButton.Press();
            _output.ClearReceivedCalls();

            // Act
            _startCancelButton.Press();

            // Assert
            _output.Received(1).OutputLine($"Display cleared");
        }

        [Test]
        public void StartCancelPressed_InDOOROPENState_OutputNotCalled()
        {
            // Arrange
            _door.Opened += Raise.Event();
            _output.ClearReceivedCalls();

            // Act
            _startCancelButton.Press();

            // Assert
            _output.DidNotReceiveWithAnyArgs().OutputLine("");
        }
    }
}
