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
    public class IntegrationStep1
    {
        public Light _light;
        public Display _display;
        public ICookController _cookController;
        public IOutput _output;
        public IButton _powerButton;
        public IButton _timeButton;
        public IButton startCancelButton;
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
            _timeButton = Substitute.For<IButton>();
            startCancelButton = Substitute.For<IButton>();

            _door = Substitute.For<IDoor>();

            _UserInterface = new UserInterface(_powerButton, _timeButton, startCancelButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void sutmigaf()
        {
            //Act
            _door.Opened += Raise.Event();
            // assert lys tænd

            _output.Received(1).OutputLine("Light is turned on");
        }
    }
}
