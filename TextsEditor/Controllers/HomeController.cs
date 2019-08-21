using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using System.Collections.Generic;
using TextsEditor.Models;
using PagedList;
using TextsEditor.Filters;
using System.Threading.Tasks;


namespace TextsEditor.Controllers
{
    public class HomeController : Controller
    {
        IFileRepository fileRepository;
        public HomeController()
        {
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IFileRepository>().To<FileRepository>();
            fileRepository = ninjectKernel.Get<IFileRepository>();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateFile()
        {
            return View();
        }

        [HttpPost]
        [FilesError]
        public async Task<ActionResult> CreateFile(string name, string text = null)
        {
            await fileRepository.Add(name, text);

            return View();
        }
        [HttpGet]
        [FilesError]
        public async Task<ActionResult> OpenFile(int id)
        {
            ViewBag.Id = id;

            return View(await fileRepository.GetFile(id));

        }
        [HttpPost]
        [FilesError]
        public async Task<ActionResult> OpenFile(int _id, string name, string text)
        {
            await fileRepository.SaveChanges(_id, name, text);

            return RedirectToAction("OpenFile", "Home", new { id = _id });

        }

        [FileTypeError]
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        [FileTypeError]
        public async Task<ActionResult> Upload(HttpPostedFileBase upload)
        {
            if (upload != null )
            {
                
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                if (fileName.EndsWith(".txt"))
                {
                    fileName = fileName.Replace(".txt", "");
                    byte[] array = null;
                    using (var readText = new System.IO.BinaryReader(upload.InputStream))
                    {
                        array = readText.ReadBytes(upload.ContentLength);
                    }
                    string text = System.Text.Encoding.Default.GetString(array);

                    await fileRepository.Add(fileName, text);
                }
                else
                {
                    return View("ErrorType");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ErrorType()
        {
            return View();
        }

        [HttpGet]
        [FilesError]
        public async Task<ActionResult> GetFile(int id)
        {
            Models.File file = await fileRepository.GetFile(id);

            byte[] array = System.Text.Encoding.Default.GetBytes(file.Text);

            string file_type = "text/xml";

            string file_name = file.Name + ".txt";

            return File(array, file_type, file_name);


        }


        public async Task<ActionResult> AllFile(int? page, string sort, string search)
        {
            ViewBag.Sort = sort;
            ViewBag.Page = page;
            ViewBag.Search = search;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IEnumerable<File> files = await fileRepository.ShowFiles(search);
            switch (sort)
            {
                case "name A-Z":
                    return View(files.OrderBy(n => n.Name).ToPagedList(pageNumber, pageSize));
                case "name Z-A":
                    return View(files.OrderByDescending(n => n.Name).ToPagedList(pageNumber, pageSize));
                default:
                    return View(files.OrderBy(n => n.Name).ToPagedList(pageNumber, pageSize));
            }

        }

        [FilesError]
        public async Task<ActionResult> RemoveFile(int id)
        {
            await fileRepository.Remove(id);

            return RedirectToAction("AllFile", "Home", await fileRepository.ShowFiles(null));
        }


        public ActionResult Error()
        {
            return View();
        }
    }
}