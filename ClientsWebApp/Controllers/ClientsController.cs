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

        public async Task<IActionResult> Index()
        {
            ViewClientsModel viewData = new ViewClientsModel
            {
                Clients = await dataContext.Clients.ToListAsync(),
                Founders = await dataContext.Founders.ToListAsync()
            };
            return View(viewData);
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

            bool uniqInns = true;
            uniqInns &= CheckClientINN(clientData.Client.INN);
            foreach (var founder in clientData.Founders)
            {
                uniqInns &= CheckFounderINN(founder.INN);
            }

            if (!uniqInns)
            {
                return GetErrorView("Ошибка добавления клиента", "ИНН уже существует или написан неверно");
            }


            for (int i = 0; i < clientData.Founders.Count; i++) 
            {
                //if (selection[i].UseExistingFounder.StartsWith("t")) //добавление существующего учредиля к ЮЛ
                //{
                //    if (int.TryParse(selection[i].FounderData.Substring(selection[i].FounderData.IndexOf(';') + 1), out int id)
                //        && dataContext.Founders.Where(n => n.Id == id).Count() != 0)
                //    {
                //        founders[i].Id = id;
                //    }
                //    else //id существующего учредителя не распознано или его нет в базе
                //    {
                //        await Response.WriteAsync("<script>alert('Error: Founder was not found (id=null)');</script>");
                //        return null;
                //    }
                //}
                //else
                //{
               
                clientData.Founders[i].AddDate = DateTime.Now;
                clientData.Founders[i].UpdateDate = DateTime.Now;
                dataContext.Founders.Add(clientData.Founders[i]);

                //}
            }

            clientData.Client.AddDate = DateTime.Now;
            clientData.Client.UpdateDate = DateTime.Now;
            dataContext.Clients.Add(clientData.Client);
            await dataContext.SaveChangesAsync();

            foreach (var founder in clientData.Founders) //many-to-many изменить поведение EF
            {
                Relation relation = new Relation()
                {
                    ClientId = clientData.Client.Id,
                    FounderId = founder.Id
                };
                dataContext.Relations.Add(relation);
            }
            await dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //[Bind("Id,INN,Name")] Client сlient
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

            return RedirectToAction(nameof(Index));

            //if (!ModelState.IsValid)
            //{
            //    if (clientData == null)
            //    {
            //        return NotFound();
            //    }



            //        clientData.Founders = await GetRelatedFounders(clientData.Client.Id);

            //    return View(clientData);
            //}

            //if (!CheckClientINN(clientData.Client.INN))
            //{
            //    return GetErrorView("Ошибка добавления клиента", "ИНН уже существует или написан неверно");
            //}

            //Client clientToChange = await dataContext.Clients.FindAsync(clientData.Client.Id);
            //if (clientToChange == null)
            //{
            //    return NotFound();
            //}

            //clientToChange.INN = clientData.Client.INN;
            //clientToChange.Name = clientData.Client.Name;
            //clientToChange.UpdateDate = DateTime.Now;
            //await dataContext.SaveChangesAsync();

            //return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeFounderClient( Client сlient)
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
                return GetErrorView("Ошибка добавления клиента", "ИНН написан неверно");
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

            return RedirectToAction(nameof(Index));
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

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClient(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFounder(int? id)
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

        public async Task<IActionResult> ChangeFounderClient(int? id)
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

        async Task<IEnumerable<Founder>> GetRelatedFounders(int id) => 
            await dataContext.Relations.Where(n => n.ClientId == id)
                    .Join(dataContext.Founders,
                    r => r.FounderId,
                    f => f.Id,
                    (r, f) => new Founder
                    {
                        INN = f.INN,
                        NameSurname = f.NameSurname
                    }).ToListAsync();

        public async Task<IActionResult> ChangeFounder(int? id)
        {
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

        async Task<IEnumerable<Client>> GetRelatedClients(int id) => 
            await dataContext.Relations.Where(n => n.FounderId == id)
                 .Join(dataContext.Clients,
                 r => r.ClientId,
                 c => c.Id,
                 (r, c) => new Client
                 {
                     INN = c.INN,
                     ClientName = c.ClientName,
                     Type = c.Type,
                 }).ToListAsync();

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
    }
}
