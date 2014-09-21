using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WecareMVC.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using System.Collections.Specialized;
using System.Configuration;

namespace WecareMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StoreManagerController : Controller
    {
        private MusicStoreEntities db = new MusicStoreEntities();

        // GET: StoreManager/Upload/5
        public async Task<ActionResult> Upload(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            return View(album);
        }

        // POST: StoreManager/Upload/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload([Bind(Include = "AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album, int id, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    album.AlbumId = id;
                   
                    if (file.ContentLength > 0)
                    {
                        //只是存資料夾
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/Images"),   fileName);
                        file.SaveAs(path);

                        //建立容器
                        //this.CreateContainerExists();

                        //HttpPostedFileBase轉成byte
                        //MemoryStream target = new MemoryStream();
                        //file.InputStream.CopyTo(target);
                        //byte[] data = target.ToArray();
                        //SaveImage(Guid.NewGuid().ToString(), file.FileName, file.ContentType, data);
                        //SaveImage(file);

                        //album.AlbumArtUrl = "https://wecaremvc.blob.core.windows.net/photos/" + file.FileName;
                        album.AlbumArtUrl = path;

                        db.Entry(album).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                //ViewBag.Message = "Upload successful";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                //return RedirectToAction("Uploads");
                ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
                ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
                return View(album);
            }
        }

        private void SaveImage(HttpPostedFileBase data)
        {
            CloudBlockBlob blob = this.GetContainer().GetBlockBlobReference(data.FileName);
            blob.Properties.ContentType = data.ContentType;
            using (var fileStream = data.InputStream)
            {
                blob.UploadFromStream(fileStream);
            } 
        }

        //存取Image
        private void SaveImage(string id, string fileName, string contentType, byte[] data)
        {
            //利用GetBlobReference來取得Blob，第一個參數會帶進檔案名稱，
            //而此檔案名稱會化成Uri的一部分。https://wecaremvc.blob.core.windows.net/photos/fileName
            CloudBlockBlob blob = this.GetContainer().GetBlockBlobReference(fileName);            
            //檔案型態
            blob.Properties.ContentType = contentType;
            blob.UploadFromByteArray(data,0,data.Length);  
        }

        private CloudBlobContainer GetContainer()
        {
            //取得Storage Account。
            CloudStorageAccount account = CloudStorageAccount.Parse( 
                                 ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            //取得Storage的Client
            CloudBlobClient client = account.CreateCloudBlobClient();
            //取得Container關聯。
            return client.GetContainerReference("photos");
        }

        private void CreateContainerExists()
        {
            CloudBlobContainer container = this.GetContainer();
            //假如沒有這個Container，就建立一個。
            container.CreateIfNotExists();

            container.SetPermissions(
             new BlobContainerPermissions
             {
                    PublicAccess = BlobContainerPublicAccessType.Blob
            });
        }


        // GET: StoreManager
        public async Task<ActionResult> Index()
        {
            var albums = db.Albums.Include(a => a.Artist).Include(a => a.Genre);
            return View(await albums.ToListAsync());
        }

        // GET: StoreManager/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: StoreManager/Create
        public ActionResult Create()
        {
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name");
            return View();
        }

        // POST: StoreManager/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album, HttpPostedFileBase file)
        {
            if (ModelState.IsValid&& file!=null)
            {
                //圖片存資料夾
                
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images/Upload"), fileName);
                    file.SaveAs(path);
                    album.AlbumArtUrl = "/Content/Images/Upload/" + fileName;


                    db.Albums.Add(album);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = album.AlbumId });   
            }

            


            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.uploadInfo = "請選擇圖片！";
            return View(album);
        }

        // GET: StoreManager/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            return View(album);
        }

        // POST: StoreManager/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album, int id, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                album.AlbumId = id;

                //if (file.ContentLength!=0 && file!= null)
                //{
                //    SaveImage(file);
                //    album.AlbumArtUrl = "https://wecaremvc.blob.core.windows.net/photos/" + file.FileName;
                //}
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images/Upload"), fileName);
                    file.SaveAs(path);
                    album.AlbumArtUrl = "/Content/Images/Upload/" + fileName;
                }
                db.Entry(album).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new {id = album.AlbumId });
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            return View(album);
        }

        // GET: StoreManager/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: StoreManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Album album = await db.Albums.FindAsync(id);
            db.Albums.Remove(album);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
