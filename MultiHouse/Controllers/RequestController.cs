using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            return View(_context.HousesRequests.Select(x=>x).ToList());
        }


        public IActionResult RequestsView([FromQuery]RequestsFilter filter)
        {
            var requests = _context.HousesRequests.Select(x => x);
            
            if (filter.Status != null && filter.Status != "")
            {
                requests = requests.Where(x => x.Status == filter.Status);
            }
            
            
            
            return View("Index",requests.ToList());
        }
        
        

        public IActionResult RequestView(int id)
        {
            return View(_context.HousesRequests.First(x=>x.Id==id));
        }

        public IActionResult EditStatus()
        {
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
                SendMail("smtp.gmail.com", "qwertyqwerty30792@gmail.com", PersonalData.GMailPassword, houseRequest.EmailAddress,
                    "Уведомление об успешной подачи заявки в сервисе MultiHouse", "Ваша заявка принята, с вами свяжутся позднее", null);

            }

            
            _context.HousesRequests.Add(houseRequest);
            _context.SaveChanges();

            ViewData["status"] = "заявка принята, проверьте почту";
            
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




        public static void SendEmail()
        {
            SendEmail2();
        }
        
        
        public static void SendEmail1()
        {
            MailAddress from = new MailAddress("kesha.tkachenko2017@mail.ru", "Necromant");
            MailAddress to = new MailAddress("innokentijtkacenko@gmail.com");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Тест";
            m.Body = "Письмо-тест 2 работы smtp-клиента";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("kesha.tkachenko2017@mail.ru", "Kesha03x_kesha03x");
            smtp.EnableSsl = true;
            smtp.SendMailAsync(m);
        }
        
        public static void SendEmail2()
        {
            
            // SendMail("smtp.gmail.com", "qwertyqwerty30792@gmail.com", "kesha03x#", "kkeshax@gmail.com", "Тема письма", "Тело письма", null);
            // SendMail("smtp.gmail.com", "qwertyqwerty30792@gmail.com", "kesha03x#", "kesha.tkachenko2017@mail.ru", "Тема письма", "Тело письма", null);

            SendMail("smtp.gmail.com", "qwertyqwerty30792@gmail.com", "kesha03x#", "klfkb'dsjb'jbshax@gmail.com", "Тема письма", "Тело письма", null);

            
            
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