﻿using System;
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

    //+--------+------+---------------+-------+-------------+------------+--------------------+---------+----------------+-----------+-------+--------+
    //| Step # | Door | UserInterface | Light | PowerButton | TimeButton | Start-CancelButton | Display | CookController | PowerTube | Timer | Output |
    //+--------+------+---------------+-------+-------------+------------+--------------------+---------+----------------+-----------+-------+--------+
    //| 5      |  S   |      X        |   X   |     S       |     T      |         S          |    X    |       S        |           |       |   S    |
    //+--------+------+---------------+-------+-------------+------------+--------------------+---------+----------------+-----------+-------+--------+

    [TestFixture]
    public class IntegrationStep5
    {
        public Light _light;
        public Display _display;
        public ICookController _cookController;
        public IOutput _output;
        public IButton _powerButton;
        public Button _timeButton;
        public IButton _startCancelButton;
        public IDoor _door;

        public UserInterface _UserInterface;

        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _cookController = Substitute.For<ICookController>();
            _light = new Light(_output);
            _display = new Display(_output);

            _powerButton = Substitute.For<IButton>();
            _timeButton = new Button();
            _startCancelButton = Substitute.For<IButton>();

            _door = Substitute.For<IDoor>();

            _UserInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void TimeButtonPressedInSETPOWERStateMakesDisplayShowTime()
        {
            int min = 1;
            int sec = 0;

            // Arrange: Bring User Interface to SETPOWER State
            _powerButton.Pressed += Raise.Event();

            // Act
            _timeButton.Press();

            // Assert
            _output.Received(1).OutputLine($"Display shows: {min:D2}:{sec:D2}");
        }

        [Test]
        public void TimeButtonPressedInSETTIMEStateMakesDisplayShowTime()
        {
            int min = 2;
            int sec = 0;

            // Arrange: Bring User Interface to SETPOWER State
            _powerButton.Pressed += Raise.Event();
            _timeButton.Press();

            // Act
            _timeButton.Press();

            // Assert
            _output.Received(1).OutputLine($"Display shows: {min:D2}:{sec:D2}");
        }
    }
}
