using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

//Propios
using SafeAuto.Common.Modelos;

namespace SafeAuto.Controllers
{
    public class SafeAutoController : Controller
    {
        #region Properties
        public static List<Driver> lstDrivers = new List<Driver>();
        public static List<Trip> lstTrips = new List<Trip>();
        public static List<Estimation> lstEstimation = new List<Estimation>();
        public static List<DriverTripRpt> lstRptDriverTrip = new List<DriverTripRpt>();
        #endregion

        #region Actions
        [HttpPost]
        public JsonResult LoadFile(HttpPostedFileBase file)
        {
            LimpiarVariables();
            string result = "Archivo no valido";

            if (file == null || file.ContentLength == 0)
            {
                result = "Debe seleccionar un archivo";
                return Json(result);
            }

            if (Path.GetExtension(file.FileName) != ".txt")
            {
                result = "Archivo no valido. Solo txt";
            }

            try
            {
                StreamReader sr = new StreamReader(file.InputStream);
                while (!sr.EndOfStream)
                {
                    string[] currentLineArray = sr.ReadLine().Split(' ');
                    switch (currentLineArray[0])
                    {
                        case "Driver":
                            Driver driver = new Driver()
                            {
                                name = currentLineArray[1]
                            };
                            if (!lstDrivers.Exists(x => x.name == currentLineArray[1]))
                            {
                                lstDrivers.Add(driver);

                                //Add to report
                                DriverTripRpt rptDriverTrip = new DriverTripRpt()
                                {
                                    driver = driver
                                };
                                lstRptDriverTrip.Add(rptDriverTrip);
                            }
                            break;
                        case "Trip":
                            Driver driverTrip = new Driver()
                            {
                                name = Convert.ToString(currentLineArray[1])
                            };
                            Trip trip = new Trip()
                            {
                                driver = driverTrip,
                                initialTime = TimeSpan.Parse(currentLineArray[2]),
                                finalTime = TimeSpan.Parse(currentLineArray[3]),
                                milles = Convert.ToDouble(currentLineArray[4].ToString().Replace(".", ",")),
                            };
                            int hours = trip.finalTime.Hours - trip.initialTime.Hours;
                            trip.minutes = (hours*60) + (trip.finalTime.Minutes - trip.initialTime.Minutes);
                            trip.velocity = Convert.ToInt16(Math.Round((60 * trip.milles) / trip.minutes));


                            //Add to list of trips
                            if (trip.velocity > 5 && trip.velocity < 101)
                            {
                                lstTrips.Add(trip);

                                //Add to report
                                DriverTripRpt rptDriverTrip2 = lstRptDriverTrip.Find(x => x.driver.name == trip.driver.name);
                                if (rptDriverTrip2.lstTrips == null)
                                {
                                    List<Trip> lstTrips2 = new List<Trip>();
                                    lstTrips2.Add(trip);
                                    rptDriverTrip2.lstTrips = lstTrips2;
                                }
                                else
                                {
                                    rptDriverTrip2.lstTrips.Add(trip);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            result = "OK";
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetEstimation()
        {
            for (int i = 0; i < lstRptDriverTrip.Count; i++)
            {
                Estimation estimation = new Estimation()
                {
                    driver = lstRptDriverTrip[i].driver,
                };
                double milles = 0;
                int minutes = 0;
                if (lstRptDriverTrip[i].lstTrips!=null)
                {
                    foreach (Trip currentTrip in lstRptDriverTrip[i].lstTrips)
                    {
                        minutes += currentTrip.minutes;
                        milles += currentTrip.milles;
                    }
                    estimation.milles = Math.Round(milles);
                    estimation.velocity = Math.Round((estimation.milles * 60) / minutes);
                }
                else
                {
                    estimation.milles = 0;
                    estimation.velocity = 0;
                }
                lstEstimation.Add(estimation);
            }

            //Reorder the output
            IEnumerable<Estimation> query = lstEstimation.OrderByDescending(est => est.milles);
            return Json(query);
        }
        #endregion

        #region Methods
        public void LimpiarVariables()
        {
            lstDrivers.Clear();
            lstTrips.Clear();
            lstEstimation.Clear();
            lstRptDriverTrip.Clear();
        }
        #endregion
    }
}