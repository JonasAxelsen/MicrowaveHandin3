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
        private IButton _button;

        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _button = Substitute.For<IButton>();
            _cookController = Substitute.For<ICookController>();

            _door = new Door();
            _light = new Light(_output);
            _display = new Display(_output);
            _userInterface = new UserInterface(_button,_button,_button, _door, _display, _light, _cookController);
        }

        [Test]
        public void Door_OpenOnce_NoThrow()
        {
            Assert.That(() => _door.Open(), Throws.Nothing);
        }

        [Test]
        public void Door_OpenTwice_NoThrow()
        {
            _door.Open();
            Assert.That(() => _door.Open(), Throws.Nothing);
        }
    }
}
