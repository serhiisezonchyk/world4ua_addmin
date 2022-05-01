using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using world4admin.ua.DataContext;
using world4admin.ua.Models;

namespace world4admin.ua.Controllers
{
    public class empController : Controller
    {
        private DataSet dataSet = null;
        public ActionResult Index()
        {
            return View();
        }
        // GET: emp
        public ActionResult Main()
        {
            try {
                return View(((ApplicationDbContext)Session["db"]).Empobj.ToList());
            }
            catch (Exception e) {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "host")] string host, [Bind(Include = "port")] string port, [Bind(Include = "database")] string database, [Bind(Include = "user")] string user, [Bind(Include = "pass")] string pass)
        {
            if (ModelState.IsValid)
            {
                string conString = "Server=" + host + ";Port=" + port + ";Database=" + database + ";User Id=" + user + ";Password=" + pass + ";";
                Session["db"] = new ApplicationDbContext(conString);
                Session["connection"] = Connect(host,Convert.ToInt32(port),database,user,pass);
                return RedirectToAction("Main");
            }
            else
                return Index();
            }
        // GET: emp/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            empClass empClass = ((ApplicationDbContext)Session["db"]).Empobj.Find(id);
            if (empClass == null)
            {
                return HttpNotFound();
            }
            return View(empClass);
        }

        // GET: emp/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            empClass empClass = ((ApplicationDbContext)Session["db"]).Empobj.Find(id);
            if (empClass == null)
            {
                return HttpNotFound();
            }
            return View(empClass);
        }

        // POST: emp/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "gid,name,fugitive_status,general_info,entry_doc,reg_doc,transport,housing,nutrition,pets,charity,add_info")] empClass empClass)
        {
            /*            if (ModelState.IsValid)
                        {

                            ((ApplicationDbContext)Session["db"]).Entry(empClass).State = EntityState.Modified;

                            ((ApplicationDbContext)Session["db"]).SaveChanges();
                            return RedirectToAction("Main");
                        }
                        return View(empClass);*/

            if (ModelState.IsValid)
            {
                //Retrieve from base by id
                empClass objFromBase = ((ApplicationDbContext)Session["db"]).Empobj.Find(empClass.gid);

                //This will put all attributes of subnetsettings in objFromBase
                objFromBase.add_info = empClass.add_info;
                objFromBase.charity = empClass.charity;
                objFromBase.color_code = empClass.color_code;
                objFromBase.continent = empClass.continent;
                objFromBase.entry_doc = empClass.entry_doc;
                objFromBase.french_shor = empClass.french_shor;
                objFromBase.fugitive_status = empClass.fugitive_status;
                objFromBase.general_info = empClass.general_info;
                objFromBase.geom = empClass.geom;
                objFromBase.housing = empClass.housing;
                objFromBase.iso3 = empClass.iso3;
                objFromBase.iso_3166_1_ = empClass.iso_3166_1_;
                objFromBase.name = empClass.name;
                objFromBase.nutrition = empClass.nutrition;
                objFromBase.pets = empClass.pets;
                objFromBase.region = empClass.region;
                objFromBase.reg_doc = empClass.reg_doc;
                objFromBase.status = empClass.status;
                objFromBase.transport = empClass.transport;
                ((ApplicationDbContext)Session["db"]).SaveChanges();

                return RedirectToAction("Main");
            }
            return View(empClass);
        }

        public ActionResult Savejson()
        {

            string path = @"D:\Territories_2.js";

            NpgsqlDataAdapter jsonDataAdapter = new NpgsqlDataAdapter(
            "select st_asgeojson(world_boundaries.*) from world_boundaries;", ((NpgsqlConnection)Session["connection"]));
            new NpgsqlCommandBuilder(jsonDataAdapter);
            jsonDataAdapter.Fill(getDataSet(), "Json");
            DataTable dt = getDataSet().Tables["Json"];
            if (dt.Rows.Count > 0)
            {
                string str = "var json_Territories_2 = {\n"
                + "\"type\": \"FeatureCollection\",\n"
                + "\"name\": \"Territories_2\",\n"
                + "\"crs\": { \"type\": \"name\", \"properties\": { \"name\": \"urn:ogc:def:crs:OGC:1.3:CRS84\" } },\n"
                + "\"features\": [\n";
                foreach (DataRow dr in dt.Rows)
                    str += dr[0].ToString() + ",\n";
                str = str.Remove(str.Length - 2);
                str += "]\n}";
                System.IO.File.WriteAllText(path, str);
            }
            return RedirectToAction("Main");

        }

        public ActionResult Logout()
        {
           
            Session["db"] = null;
            Session["connection"] = null;
            return RedirectToAction("Index");
        }

        public NpgsqlConnection Connect(string host, int port, string database,
 string user, string parol)
        {
            NpgsqlConnectionStringBuilder stringBuilder =
            new NpgsqlConnectionStringBuilder();
            stringBuilder.Host = host;
            stringBuilder.Port = port;
            stringBuilder.Username = user;
            stringBuilder.Password = parol;
            stringBuilder.Database = database;
            stringBuilder.Timeout = 30;
            NpgsqlConnection connection =
            new NpgsqlConnection(stringBuilder.ConnectionString);
            connection.Open();
            return connection;
        }
        private DataSet getDataSet()
        {
            if (dataSet == null)
            {
                dataSet = new DataSet();
                dataSet.Tables.Add("Json");
            }
            return dataSet;
        }

    }
}
