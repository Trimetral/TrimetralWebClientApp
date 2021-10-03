using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientsWebApp.Data;
using ClientsWebApp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClientsWebApp.Controllers
{//сделать автозаполнение формы при использовании существующего учредителя
    public class ClientsController : Controller
    {
        readonly DataContext dataContext;

        public ClientsController(DataContext _dataContext)
        {
            dataContext = _dataContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddClient()
        {
            AddClientModel clientData = new AddClientModel()
            {
                ExistingFounders = new SelectList(await dataContext.Founders.ToListAsync())
            };
            return View(clientData);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClient(AddClientModel clientData)
        {
            if (!ModelState.IsValid)
            {
                clientData.ExistingFounders = new SelectList(await dataContext.Founders.ToListAsync());
                return View(clientData);
            }

            if (clientData == null || clientData.Client == null || clientData.Founders == null)
            {
                return GetErrorView("Ошибка добавления клиента", "Данные для добавления не найдены");
            }

            if (!CheckClientINN(clientData.Client.INN)) //Проверка ИНН нового клиента
            {
                return GetErrorView("Ошибка добавления клиента", "ИНН клиента уже существует или написан неверно");
            }

            for (int i = 0; i < clientData.Founders.Count; i++) 
            {
                if (clientData.Founders[i].Id == 0) //новый учредитель
                {
                    if (!CheckFounderINN(clientData.Founders[i].INN)) //Проверка ИНН нового учредителя
                    {
                        return GetErrorView("Ошибка добавления клиента", "ИНН уже существует или написан неверно");
                    }

                    clientData.Founders[i].AddDate = DateTime.Now;
                    clientData.Founders[i].UpdateDate = DateTime.Now;
                    dataContext.Founders.Add(clientData.Founders[i]);
                }
                else //существующий учредитель
                {   //Проверка наличия существующего учредителя по ID
                    var founder = await dataContext.Founders.FirstOrDefaultAsync(n => n.Id == clientData.Founders[i].Id);
                    if (founder == null)
                    {
                        return GetErrorView("Ошибка добавления клиента", "Существующий учредитель не найден");
                    }
                    clientData.Founders[i] = founder;
                }
            }

            clientData.Client.AddDate = DateTime.Now;
            clientData.Client.UpdateDate = DateTime.Now;
            clientData.Client.Founders = clientData.Founders;
            dataContext.Clients.Add(clientData.Client);
            await dataContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeClient([Bind("Id,INN,ClientName")] Client сlient)
        {
            if (!ModelState.IsValid)
            {
                if (сlient == null)
                {
                    return NotFound();
                }

                EditClientModel clientData = new EditClientModel()
                {
                    Client = сlient,
                    Founders = await GetRelatedFounders(сlient.Id)
                };
                ModelState.ClearValidationState(nameof(EditClientModel));
                TryValidateModel(clientData, nameof(EditClientModel));

                return View(clientData);
            }

            if (!CheckClientINN(сlient.INN, false))
            {
                return GetErrorView("Ошибка добавления клиента", "ИНН уже существует или написан неверно");
            }

            Client clientToChange = await dataContext.Clients.FindAsync(сlient.Id);
            if (clientToChange == null)
            {
                return NotFound();
            }

            clientToChange.INN = сlient.INN;
            clientToChange.ClientName = сlient.ClientName;
            clientToChange.UpdateDate = DateTime.Now;
            await dataContext.SaveChangesAsync();

            return RedirectToAction(nameof(ViewClients));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeFounder([Bind("Id,INN,NameSurname")] Founder founder)
        {
            if (!ModelState.IsValid)
            {
                if (founder == null)
                {
                    return NotFound();
                }

                EditFounderModel founderData = new EditFounderModel()
                {
                    Founder = founder,
                    Clients = await GetRelatedClients(founder.Id)
                };
                ModelState.ClearValidationState(nameof(EditFounderModel));
                TryValidateModel(founderData, nameof(EditFounderModel));

                return View(founderData);
            }

            if (!CheckFounderINN(founder.INN, false))
            {
                return GetErrorView("Ошибка добавления клиента", "ИНН написан неверно");
            }

            Founder founderToChange = await dataContext.Founders.FindAsync(founder.Id);
            if (founderToChange == null)
            {
                return NotFound();
            }

            founderToChange.INN = founder.INN;
            founderToChange.NameSurname = founder.NameSurname;
            founderToChange.UpdateDate = DateTime.Now;
            await dataContext.SaveChangesAsync();

            return RedirectToAction(nameof(ViewFounders));
        }

        public IActionResult DeleteClient(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            return View(Id);
        }

        [HttpPost, ActionName("DeleteClient")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmClient(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delClient = await dataContext.Clients.FindAsync(id);
            if(delClient == null)
            {
                return NotFound();
            }

            dataContext.Clients.Remove(delClient);
            await dataContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteFounder(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            return View(Id);
        }

        [HttpPost, ActionName("DeleteFounder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmFounder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delFounder = await dataContext.Founders.FindAsync(id);
            if (delFounder == null)
            {
                return NotFound();
            }

            dataContext.Founders.Remove(delFounder);
            await dataContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangeClient(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await dataContext.Clients.FirstOrDefaultAsync(n => n.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            EditClientModel editData = new EditClientModel()
            {
                Client = client,
                Founders = await GetRelatedFounders(client.Id)
            };

            return View(editData);
        }

        async Task<IEnumerable<Founder>> GetRelatedFounders(int id)
        {
            var data = await dataContext.Clients.Where(n => n.Id == id).Select(n => n.Founders).ToListAsync();
            return data[0];
        }

        public async Task<IActionResult> ChangeFounder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var founder = await dataContext.Founders.FirstOrDefaultAsync(n => n.Id == id);
            if (founder == null)
            {
                return NotFound();
            }

            EditFounderModel editData = new EditFounderModel()
            {
                Founder = founder,
                Clients = await GetRelatedClients(founder.Id)
            };

            return View(editData);
        }

        async Task<IEnumerable<Client>> GetRelatedClients(int id)
        {
            var data = await dataContext.Founders.Where(n => n.Id == id).Select(n => n.Clients).ToListAsync();
            return data[0];
        }

        IActionResult GetErrorView(string title, string body)
        {
            return View(nameof(Error), new ErrorModel() { ErrorBody = body, ErrorTitle = title });
        }

        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Проверить ИНН нового клиента
        /// </summary>
        /// <param name="inn">ИНН нового клиента</param>
        /// <param name="checkDoubles">Нужна ли проверка на дубликат ИНН</param>
        /// <returns>true - прошёл проверку, false - нет</returns>
        bool CheckClientINN(string inn, bool checkDoubles = true)
        {
            if (inn == null) return false;
            if (long.TryParse(inn, out long _))
            {
                if (!checkDoubles) return true;
                return !dataContext.Clients.Where(n => n.INN == inn).Any();
            }
            return false;
        }

        /// <summary>
        /// Проверить ИНН нового учредителя
        /// </summary>
        /// <param name="inn">ИНН учредителя</param>
        /// <param name="checkDoubles">Нужна ли проверка на дубликат ИНН</param>
        /// <returns>true - прошёл проверку, false - нет</returns>
        bool CheckFounderINN(string inn, bool checkDoubles = true)
        {
            if (inn == null) return false;
            if (long.TryParse(inn, out long _))
            {
                if (!checkDoubles) return true;
                return !dataContext.Founders.Where(n => n.INN == inn).Any();
            }
            return false;
        }

        public async Task<IActionResult> ViewClients(ViewClientsModel viewClients)
        {
            IQueryable<Client> clients = dataContext.Clients;
            if (viewClients != null && viewClients.Search != null)
            {
                if (viewClients.Search.ClientINN != null) 
                    clients = clients.Where(n => viewClients.Search.ClientINN.Length == 10 ? n.INN == viewClients.Search.ClientINN : n.INN.Contains(viewClients.Search.ClientINN));
                if (viewClients.Search.ClientName != null) 
                    clients = clients.Where(n => n.ClientName.Contains(viewClients.Search.ClientName));
                if (viewClients.Search.ClientType != null)
                {
                    if (viewClients.Search.ClientType.ToUpper() == "ИП")
                        clients = clients.Where(n => n.Type == false);
                    if (viewClients.Search.ClientType.ToUpper() == "ЮЛ")
                        clients = clients.Where(n => n.Type == true);
                }
            }
            int count = await clients.CountAsync();
            int pages = count / viewClients.Count;
            if (pages * viewClients.Count < count) pages++;

            viewClients.Pages = pages;
            viewClients.Clients = await clients.OrderBy(n => n.ClientName).Skip((viewClients.Page - 1) * viewClients.Count).Take(viewClients.Count).ToListAsync();
            return View(viewClients);
        }

        public async Task<IActionResult> ViewFounders(ViewFoundersModel viewFounders)
        {
            IQueryable<Founder> founders = dataContext.Founders;
            if (viewFounders != null && viewFounders.Search != null)
            {
                if (viewFounders.Search.FounderINN != null) 
                    founders = founders.Where(n => viewFounders.Search.FounderINN.Length == 12 ? n.INN == viewFounders.Search.FounderINN : n.INN.Contains(viewFounders.Search.FounderINN));
                if (viewFounders.Search.FounderName != null) 
                    founders = founders.Where(n => n.NameSurname.Contains(viewFounders.Search.FounderName));
            }
            int count = await founders.CountAsync();
            int pages = count / viewFounders.Count;
            if (pages * viewFounders.Count < count) pages++;

            viewFounders.Pages = pages;
            viewFounders.Founders = await founders.OrderBy(n => n.NameSurname).Skip((viewFounders.Page - 1) * viewFounders.Count).Take(viewFounders.Count).ToListAsync();
            return View(viewFounders);
        }
    }
}
