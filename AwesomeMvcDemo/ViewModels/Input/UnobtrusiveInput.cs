using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.ViewModels.Input
{
    public class UnobtrusiveInput
    {
        [Required]
        [AdditionalMetadata("placeholder", "write something")]
        [AdditionalMetadata("maxLength", 13)]
        [DisplayName("Textbox")]
        public string Name { get; set; }

        [Required]
        [AdditionalMetadata("placeholder", "only numbers here")]
        [AdditionalMetadata("decimals", 2)]
        [DisplayName("Numeric textbox")]
        public float? Number { get; set; }

        [Required]
        [UIHint("Autocomplete")]
        [AdditionalMetadata("placeholder", "try Mango")]
        [AdditionalMetadata("controller", "MealAutocomplete")]
        [DisplayName("Autocomplete")]
        public string MealAuto { get; set; }

        [Required]
        [AdditionalMetadata("Placeholder", "pick a date")]
        [DisplayName("DatePicker")]
        public DateTime? Date { get; set; }

        [Required]
        [UIHint("AjaxCheckboxList")]
        [AweUrl(Action = "GetCategories", Controller = "Data")]
        [DisplayName("CheckboxList")]
        public IEnumerable<int> Categories { get; set; }

        [Required]
        [UIHint("AjaxRadioList")]
        [AweUrl(Action = "GetCategories", Controller = "Data")]
        [DisplayName("RadioList")]
        public int? Category2 { get; set; }

        [Required]
        [DisplayName("Dropdown")]
        [AweUrl(Action = "GetCategoriesFirstOption", Controller = "Data")]
        [UIHint("AjaxDropdown")]
        public int? CategoryFo { get; set; }

        [Required]
        [UIHint("Lookup")]
        [AdditionalMetadata("ClearButton", true)]
        [DisplayName("Lookup")]
        public int? Meal { get; set; }

        [Required]
        [UIHint("MultiLookup")]
        [AdditionalMetadata("ClearButton", true)]
        [DisplayName("MultiLookup")]
        public IEnumerable<int> Meals { get; set; }

        [Required]
        [UIHint("Odropdown")]
        [DisplayName("Odropdown")]
        [AweUrl(Action = "GetAllMeals", Controller = "Data")]
        public int? MealsOdd { get; set; }

        [Required]
        [UIHint("OdropdownFavs")]
        [DisplayName("Odropdown Favs")]
        [AweUrl(Action = "GetAllMeals", Controller = "Data")]
        public int? MealsOddFavs { get; set; }

        [Required]
        [UIHint("ButtonGroupRadio")]
        [DisplayName("ButtonGroup")]
        [AweUrl(Action = "GetCategories", Controller = "Data")]
        public int? CategoryBgrId { get; set; }

        [Required]
        [UIHint("ButtonGroupCheckbox")]
        [DisplayName("ButtonGroup Multi")]
        [AweUrl(Action = "GetCategories", Controller = "Data")]
        public int[] CategoriesBgcIds { get; set; }

        [Required]
        [UIHint("Multiselect")]
        [DisplayName("Multiselect")]
        [AweUrl(Action = "GetCategories", Controller = "Data")]
        public int[] CategoriesMultiIds { get; set; }

        [Required]
        [UIHint("ColorDropdown")]
        [DisplayName("ColorPicker")]
        public string ColorId { get; set; }

        [Required]
        [UIHint("Combobox")]
        [DisplayName("Combobox")]
        [AweUrl(Action = "GetAllMeals", Controller = "Data")]
        public string MealComboId { get; set; }
        
        [DisplayName("Checkbox")]
        public bool Organic { get; set; }

        [UIHint("ChkEmpVal")]
        [Required(ErrorMessage = "This must be checked, in order to submit the form")]
        [DisplayName("Checkbox (Required)")]
        public bool Organic2 { get; set; }

        [UIHint("TogglEmpVal")]
        [Required(ErrorMessage = "This must be Yes, in order to submit the form")]
        [DisplayName("Toggle Button")]
        public bool Organic3 { get; set; }

        [UIHint("SchkEmpVal")]
        [Required(ErrorMessage = "This must be checked, in order to submit the form")]
        public bool OrganicSim { get; set; }
    }
}