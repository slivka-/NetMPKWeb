using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetMPK.Domain.Abstract;
using NetMPK.WebUI.Controllers;
using System.Linq;
using Moq;

namespace NetMPK.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanPaginate()
        {
            Mock<IStopRepository> mock = new Mock<IStopRepository>();
            mock.Setup(m => m.StopNames).Returns(new List<string>() {
                "T1","T2","T3","T4"
            });

            StopsController controller = new StopsController(mock.Object);
            controller.pageSize = 2;

            IEnumerable<string> result = (IEnumerable<string>)controller.StopsList(2).Model;

            string[] nameArr = result.ToArray();
            Assert.IsTrue(nameArr.Length == 2);
            Assert.AreEqual(nameArr[0], "T3");
            Assert.AreEqual(nameArr[1], "T4");

        }
    }
}
