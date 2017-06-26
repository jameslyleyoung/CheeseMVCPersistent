using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller 
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            IList<Menu> menus = context.Menus.ToList();

            return View(menus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {

            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };

                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }
            return View(addMenuViewModel);
        }

        public IActionResult ViewMenu(int id)
        {
            Menu menu = context.Menus.Single(c => c.ID == id);

            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel
            {
                Menu = menu,
                Items = items
            };

            return View(viewMenuViewModel);
        }

        public IActionResult AddItem(int id)
        {
            IList<Cheese> cheeses = context.Cheeses.Include(c => c.Category).ToList();
            Menu menu = context.Menus.Single(c => c.ID == id);

            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu, cheeses);
            
            return View(addMenuItemViewModel);
        }
         
        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(cm => cm.CheeseID == addMenuItemViewModel.CheeseID)
                    .Where(cm => cm.MenuID == addMenuItemViewModel.MenuID)
                    .ToList();
                if (existingItems.Count == 0)
                {
                    CheeseMenu newCheeseMenu = new CheeseMenu
                    {
                        //TODO 2: Am I usuing the ID prop here to set the prop?
                        MenuID = addMenuItemViewModel.MenuID,
                        CheeseID = addMenuItemViewModel.CheeseID
                    };
                    context.CheeseMenus.Add(newCheeseMenu);
                    context.SaveChanges();

                    return Redirect(string.Format("/Menu/ViewMenu/{0}", newCheeseMenu.MenuID));
                }
                return Redirect(string.Format("/Menu/ViewMenu/{0}", addMenuItemViewModel.MenuID));

            }

            return View(addMenuItemViewModel);
        }
    }
}
