using System;
using System.Collections.Generic;
using FactoryWindowGUI.ViewModel;
using NUnit.Framework;

namespace FactoryWindowGUI.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        [TestCase(0 ,0,0)]
        [TestCase(1 ,0,10)]
        [TestCase( 31,5,10)]
        [TestCase(143,23,50)]
        [TestCase(133 ,22,10)]
        public void ConvertIndexToDateTimeTest(int index,int hour, int minute )
        {
            var systemControlViewModel = new SystemControlViewModel();

            var convertIndexToDateTime = systemControlViewModel.ConvertIndexToDateTime(index);
            
            var expectedDateTime=new DateTime(1, 1, 1, hour, minute, 0);

            Assert.IsTrue(convertIndexToDateTime==expectedDateTime);
        }
    }
}