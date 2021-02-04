using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MultiHouse.Database;
using MultiHouse.Models;

namespace MultiHouse.Controllers
{
    public class RequestController : Controller
    {

        private readonly MHContext _context;

        public RequestController(MHContext context)
        {
            _context = context;
        }
        
        
        // GET
        public IActionResult Index()
        {

            bool authorized = Helpers.DataHelper.IsAdminAuthorized(HttpContext);

            if (!authorized)
            {
                return Redirect("/Admin");
            }
            
            return View(_context.HousesRequests.Select(x=>x).ToList());
        }


        public IActionResult RequestsView([FromQuery]RequestsFilter filter)
        {
            
            bool authorized = Helpers.DataHelper.IsAdminAuthorized(HttpContext);

            if (!authorized)
            {
                return Redirect("/Admin");
            }
            
            var requests = _context.HousesRequests.Select(x => x);
            
            if (filter.Status != null && filter.Status != "")
            {
                requests = requests.Where(x => x.Status == filter.Status);
            }

            if (filter.UserName != null && filter.UserName != "")
            {
                requests = requests.Where(x =>
                    (x.FirstName + " " + x.LastName + " " + x.Patronymic).Contains(filter.UserName));
            }


            if (filter.ExcludeChecked != "" && filter.ExcludeChecked != null)
            {
                requests = requests.Where(x => x.Status != "проверено");
            }

            if (filter.Exclude != "" && filter.Exclude != null)
            {
                requests = requests.Where(x => x.Status != filter.Exclude);
            }
            
            
            
            return View("Index",requests.ToList());
        }
        
        

        public IActionResult RequestView(int id)
        {
            bool authorized = Helpers.DataHelper.IsAdminAuthorized(HttpContext);

            if (!authorized)
            {
                return Redirect("/Admin");
            }
            
            return View(_context.HousesRequests.First(x=>x.Id==id));
        }

        public IActionResult EditStatus()
        {
            bool authorized = Helpers.DataHelper.IsAdminAuthorized(HttpContext);

            if (!authorized)
            {
                return Redirect("/Admin");
            }
            
            int hRequestId = Convert.ToInt32(Request.Query["id"]);
            HouseRequest hr = _context.HousesRequests.First(x => x.Id == hRequestId);

            hr.Status = Request.Form["status"];

            _context.HousesRequests.Update(hr);

            _context.SaveChanges();

            return View("RequestView", hr);
        }

        public IActionResult Create([FromForm]HouseRequest houseRequest)
        {

            if (houseRequest.EmailAddress != null && houseRequest.EmailAddress != "")
            {
                SendMail("smtp.gmail.com", PersonalData.EmailAddress, PersonalData.EMailPassword, houseRequest.EmailAddress,
                    "Уведомление об успешной подачи заявки в сервисе MultiHouse",
                    "Благодарим за обращение, с вами свяжутся в ближайшее время", null);

            }
            
            
            _context.HousesRequests.Add(houseRequest);
            _context.SaveChanges();
            
            if (houseRequest.EmailAddress!=null && houseRequest.EmailAddress!="")
            {
                ViewData["status"] = "заявка принята, проверьте почту";
            }
            else
            {
                ViewData["status"] = "заявка принята, с вами свяжутся позднее";
            }
            
            
            
            return View(new HouseRequest());
        }
        
        public IActionResult CreateView(int? id)
        {
            var houseRequest = new HouseRequest();
            if (id != null)
            {
                var house = _context.Houses2.First(x => x.Id == id);

                houseRequest = new HouseRequest()
                {
                    Address = house.Address,
                    RoomCount = house.RoomCount,
                    HouseId = house.Id
                };
            }
            
            return View("Create", houseRequest);
        }

        
        
        /// <summary>
        /// Отправка письма на почтовый ящик C# mail send
        /// </summary>
        /// <param name="smtpServer">Имя SMTP-сервера</param>
        /// <param name="from">Адрес отправителя</param>
        /// <param name="password">пароль к почтовому ящику отправителя</param>
        /// <param name="mailto">Адрес получателя</param>
        /// <param name="caption">Тема письма</param>
        /// <param name="message">Сообщение</param>
        /// <param name="attachFile">Присоединенный файл</param>
        public static void SendMail(string smtpServer, string from , string password,
            string mailto, string caption, string message, string attachFile = null)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient();
                client.Host = smtpServer;
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }
        
        
        
    }
}