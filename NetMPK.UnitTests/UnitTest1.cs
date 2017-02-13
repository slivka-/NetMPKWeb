using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetMPK.WebUI.Controllers;
using System.Linq;
using Moq;
using NetMPK.WebUI.Models;
using System.Web;

namespace NetMPK.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        /*
        [TestMethod]
        public void CanPaginate()
        {
            //Arrange
            Moq.Mock<IStopRepository> mock = new Moq.Mock<IStopRepository>();
            mock.Setup(m => m.StopNames).Returns(new List<string>() {
                "T1","T2","T3","T4"
            });
            StopsController controller = new StopsController(mock.Object);
            controller.pageSize = 2;
            Isolate.WhenCalled(() => controller.ControllerContext.HttpContext.Session["isLoggedIn"]).WillReturn(false);
            //Act
            IEnumerable<string> result = ((StopsModel)controller.StopsList(2).Model).stopNames;
            //Assert
            string[] nameArr = result.ToArray();
            Assert.IsTrue(nameArr.Length == 2);
            Assert.AreEqual(nameArr[0], "T3");
            Assert.AreEqual(nameArr[1], "T4");

        }*/
    }
}
