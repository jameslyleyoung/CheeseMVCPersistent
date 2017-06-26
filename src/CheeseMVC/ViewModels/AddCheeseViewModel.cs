using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheeseMVC.ViewModels
{
    public class AddCheeseViewModel
    {
        [Required]
        [Display(Name = "Cheese Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must give your cheese a description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public AddCheeseViewModel(IEnumerable<CheeseCategory> categories)
        {
            Categories = new List<SelectListItem>();

            foreach (var category in categories)
            {
                Categories.Add(new SelectListItem {
                    Value = category.ID.ToString(),
                    Text = category.Name
                });
            }
            /*
            CheeseTypes = new List<SelectListItem>();

            // <option value="0">Hard</option>
            CheeseTypes.Add(new SelectListItem {
                Value = ((int) Categories.Name).ToString(),
                Text = CheeseType.Hard.ToString()
            });

            CheeseTypes.Add(new SelectListItem
            {
                Value = ((int)CheeseType.Soft).ToString(),
                Text = CheeseType.Soft.ToString()
            });

            CheeseTypes.Add(new SelectListItem
            {
                Value = ((int)CheeseType.Fake).ToString(),
                Text = CheeseType.Fake.ToString()
            });
            */
        }

        public AddCheeseViewModel() { }

        
    }
}
