using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SafeAuto;
using SafeAuto.Controllers;

namespace SafeAuto.Tests.Controllers
{
    [TestClass]
    public class SafeAutoControllerTest
    {
        #region Global Variables
        HttpPostedFileBase file;
        SafeAutoController controller;
        JsonResult result;
        #endregion

        #region Methods
        [TestInitialize]
        public void Inicializar()
        {
            controller = new SafeAutoController();
        }

        [TestMethod]
        public void LoadFile()
        {
            result = controller.LoadFile(file);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetEstimation()
        {
            result = controller.GetEstimation();
            Assert.IsNotNull(result);
        }
        #endregion
    }
}
