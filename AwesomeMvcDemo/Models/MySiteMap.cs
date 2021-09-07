using System.Collections.Generic;

namespace AwesomeMvcDemo.Models
{
    public static class MySiteMap
    {
        private static readonly IList<SiteMapItem> items = new List<SiteMapItem>();

        public static IEnumerable<SiteMapItem> GetAll()
        {
            return items;
        }

        static MySiteMap()
        {
            var grid = new SiteMapItem { Name = "Grid", Keywords = "gridview" };

            items.Add(grid);
            items.Add(new SiteMapItem { Name = "Overview", Controller = "GridDemo", Action = "Index", Parent = grid });
            items.Add(new SiteMapItem { Name = "Custom Format", Controller = "GridDemo", Action = "CustomFormat", Parent = grid });
            items.Add(new SiteMapItem { Name = "Grouping/Aggregates", Controller = "GridDemo", Action = "Grouping", Parent = grid, Keywords = "sum max count total" });

            var filtering = new SiteMapItem
            {
                Name = "Filtering", Controller = "GridDemo", Action = "Filtering", Parent = grid,
                Keywords = "search filtering query"
            };
            
            items.Add(filtering);
            items.Add(new SiteMapItem { Name = "FilterRow server data", Controller = "GridFilterRowServerSideData", Action = "Index", Parent = filtering, Keywords = "search filtering query" });
            items.Add(new SiteMapItem { Name = "FilterRow client data", Controller = "GridFilterRowClientData", Action = "Index", Parent = filtering, Keywords = "search filtering query" });
            items.Add(new SiteMapItem { Name = "FilterRow large data source", Controller = "GridFilterRowServerSideData", Action = "LargeDataSource", Parent = filtering, 
                Keywords = "search filtering query large data source" });

            items.Add(new SiteMapItem { Name = "Save filters", Controller = "GridSaveFilters", Action = "Index", Parent = filtering, Keywords = "persistence save filter row" });
            
            items.Add(new SiteMapItem { Name = "Frozen Columns", Controller = "GridFrozenColumns", Action = "Index", Parent = grid, Keywords = "freeze pinned right left" });
            items.Add(new SiteMapItem { Name = "Custom Querying", Controller = "GridDemo", Action = "CustomQuerying", Parent = grid, Keywords = "database async await sql" });
            items.Add(new SiteMapItem { Name = "Client Side API", Controller = "GridDemo", Action = "ClientSideApi", Parent = grid, Keywords = "refresh reload" });
            items.Add(new SiteMapItem { Name = "Nesting", Controller = "GridNestingDemo", Action = "Index", Keywords = "master detail hierarchical", Parent = grid });
            items.Add(new SiteMapItem { Name = "Tree Grid", Controller = "TreeGrid", Action = "Index", Parent = grid });
            items.Add(new SiteMapItem { Name = "Header Groups", Controller = "GridMultiRowHeaderGroups", Action = "Index", Keywords = "editable table", Parent = grid });
            items.Add(new SiteMapItem { Name = "Custom Render", Controller = "GridCustomRender", Action = "Index", Keywords = "editable table cards view", Parent = grid });
            items.Add(new SiteMapItem { Name = "Infinite Scrolling", Controller = "GridInfiniteScrollingDemo", Action = "Index", Parent = grid });

            var demos = new SiteMapItem { Name = "Demos", Parent = grid };
            items.Add(demos);
            
            var inlineEdit = new SiteMapItem
            {
                Name = "Grid Inline Editing", Controller = "GridInlineEditDemo", Action = "Index",
                Keywords = "crud signalr", Parent = demos, Collapsed = false
            };

            items.Add(inlineEdit);

            items.Add(new SiteMapItem { Name = "Grid Inline Batch Editing", Controller = "GridInlineBatchEditing", Action = "Index", Keywords = "editing save crud", Parent = inlineEdit });
            items.Add(new SiteMapItem { Name = "Client Validation", Controller = "GridInlineClientValidation", Action = "Index", Keywords = "ovld editing", Parent = inlineEdit });
            items.Add(new SiteMapItem { Name = "Client Save", Controller = "GridInlineEditDemo", Action = "ClientSave", Keywords = "editing save crud", Parent = inlineEdit });

            items.Add(new SiteMapItem
            {
                Name = "Grid Inline Editing Conditional",
                Controller = "GridInlineEditDemo",
                Action = "ConditionalDemo",
                Parent = inlineEdit
            });

            items.Add(new SiteMapItem
            {
                Name = "Grid Inline Multiple Editors",
                Controller = "GridInlineEditDemo",
                Action = "MultiEditorsDemo",
                Parent = inlineEdit
            });

            items.Add(new SiteMapItem
            {
                Name = "TreeGrid inline editing",
                Controller = "TreeGridInlineEditing",
                Action = "Index",
                Parent = inlineEdit
            });

            items.Add(new SiteMapItem { Name = "Grid Crud (Popup)", Controller = "GridCrudDemo", Action = "Index", Keywords = "editing", Parent = demos });
            items.Add(new SiteMapItem { Name = "Master Detail crud", Controller = "MasterDetailCrudDemo", Action = "Index", Parent = demos });
            items.Add(new SiteMapItem { Name = "Grid Client Data", Controller = "GridClientDataDemo", Action = "Index", Parent = demos });
            items.Add(new SiteMapItem { Name = "Scheduler", Controller = "SchedulerDemo", Action = "Index", Parent = demos });
            items.Add(new SiteMapItem { Name = "Grid Mailbox", Controller = "MailboxDemo", Action = "Index", Parent = demos });
            items.Add(new SiteMapItem { Name = "Grid In Nest Editing", Controller = "GridNestingDemo", Action = "Index", Anchor = "#In-nest-editing-grid",
                Keywords = "inline crud", Parent = demos });
            items.Add(new SiteMapItem { Name = "TreeGrid Crud", Controller = "TreeGrid", Action = "Index", Anchor = "#Tree-Grid-with-CRUD-operations", Parent = demos });
            items.Add(new SiteMapItem { Name = "Grid with checkboxes", Controller = "GridCheckboxesDemo", Action = "Index", Parent = demos });
            items.Add(new SiteMapItem { Name = "Spreadsheet Grid", Controller = "GridSpreadsheetDemo", Action = "Index", Keywords = "inline", Parent = demos });
            
            items.Add(new SiteMapItem { Name = "Autocomplete Cells Spreadsheet Grid", Controller = "GridSpreadsheetDemo", Action = "Autocomplete",
                Keywords = "inline", Parent = demos, NoMenu = true });
            items.Add(new SiteMapItem { Name = "Keyboard Navigation", Controller = "KeyboardNavigationDemo", Action = "Index", Parent = demos });

            var lookup = new SiteMapItem { Name = "Lookup", Collapsed = true };

            items.Add(lookup);
            items.Add(new SiteMapItem { Name = "Quick View", Controller = "LookupDemo", Action = "Index", Parent = lookup });
            items.Add(new SiteMapItem { Name = "Custom Search", Controller = "LookupDemo", Action = "CustomSearch", Parent = lookup });
            items.Add(new SiteMapItem { Name = "Misc", Controller = "LookupDemo", Action = "Misc", Parent = lookup });

            var multilookup = new SiteMapItem { Name = "MultiLookup", Collapsed = true };

            items.Add(multilookup);
            items.Add(new SiteMapItem { Name = "Quick View", Controller = "MultiLookupDemo", Action = "Index", Parent = multilookup });
            items.Add(new SiteMapItem { Name = "Custom Search", Controller = "MultiLookupDemo", Action = "CustomSearch", Parent = multilookup });
            items.Add(new SiteMapItem { Name = "Misc", Controller = "MultiLookupDemo", Action = "Misc", Parent = multilookup });

            var awesome = new SiteMapItem { Name = "Awesome" };

            items.Add(awesome);
            items.Add(new SiteMapItem { Name = "Drag And Drop", Controller = "DragAndDropDemo", Action = "Index", Parent = awesome, Keywords = "grid reorder rows move movable" });
            items.Add(new SiteMapItem { Name = "Dropmenu", Controller = "Dropmenu", Action = "Index", Parent = awesome });
            items.Add(new SiteMapItem { Name = "AjaxDropdown", Controller = "AjaxDropdownDemo", Action = "Index", Parent = awesome });

            var arl = new SiteMapItem
            {
                Name = "AjaxRadioList", Controller = "AjaxRadioListDemo", Action = "Index",
                Keywords = "odropdown buttongroup select combobox colordropdown timepicker ",
                Parent = awesome
            };
            
            items.Add(arl);

            items.Add(new SiteMapItem { Name = "Odropdown", Controller = "Odropdown", Action = "Index", Parent = arl, Keywords = "select single remote search autosearch"});

            items.Add(new SiteMapItem { Name = "AjaxCheckboxList", Controller = "AjaxCheckboxListDemo", Action = "Index", Keywords = "multiselect dropdown buttongroup", Parent = awesome });
            items.Add(new SiteMapItem { Name = "Autocomplete", Controller = "AutocompleteDemo", Action = "Index", Parent = awesome });
            items.Add(new SiteMapItem { Name = "Popup", Controller = "PopupDemo", Action = "Index", Parent = awesome, Keywords = "open hint tooltip hover" });
            items.Add(new SiteMapItem { Name = "PopupForm", Controller = "PopupFormDemo", Action = "Index", Parent = awesome, Keywords = "open" });
            items.Add(new SiteMapItem { Name = "Popup mod", Controller = "PopupDemo", Action = "PopupMod", Parent = awesome, NoMenu = true});
            items.Add(new SiteMapItem { Name = "Call", Controller = "CallDemo", Action = "Index", Parent = awesome });
            items.Add(new SiteMapItem { Name = "DatePicker", Controller = "DatePickerDemo", Action = "Index", Parent = awesome, Keywords = "timepicker calendar" });
            items.Add(new SiteMapItem { Name = "TextBox", Controller = "TextBoxDemo", Action = "Index", Parent = awesome, Keywords = "numeric"});
            items.Add(new SiteMapItem { Name = "Tabs", Controller = "Tabs", Action = "Index", Parent = awesome, Keywords = "tab strip"});
            items.Add(new SiteMapItem { Name = "Notification", Controller = "Notification", Action = "Index", Parent = awesome, Keywords = "notify popup"});
            items.Add(new SiteMapItem { Name = "Form", Controller = "FormDemo", Action = "Index", Parent = awesome });
            items.Add(new SiteMapItem { Name = "Pager", Controller = "PagerDemo", Action = "Index", Parent = awesome });
            items.Add(new SiteMapItem { Name = "Form Inputs", Controller = "FormInput", Action = "Index", Parent = awesome, Keywords = "ocheckbox ochk otoggle button"});

            var ajaxlist = new SiteMapItem { Name = "AjaxList", Collapsed = true };

            items.Add(ajaxlist);
            items.Add(new SiteMapItem { Name = "Quick View", Controller = "AjaxListDemo", Action = "Index", Parent = ajaxlist });
            items.Add(new SiteMapItem { Name = "Custom Item Template", Controller = "AjaxListDemo", Action = "CustomItemTemplate", Parent = ajaxlist });
            items.Add(new SiteMapItem { Name = "Client Side API", Controller = "AjaxListDemo", Action = "ClientSideApi", Parent = ajaxlist });

            // use grid with infinite scroll more button instead
            //items.Add(new SiteMapItem { Name = "Table Layout", Controller = "AjaxListDemo", Action = "TableLayout", Parent = ajaxlist });
            //items.Add(new SiteMapItem { Name = "AjaxList Crud Demo", Controller = "AjaxListDemo", Action = "Crud", Parent = ajaxlist });

            var generic = new SiteMapItem { Name = "Generic" };

            items.Add(generic);
            items.Add(new SiteMapItem { Name = "Unobtrusive validation", Controller = "Unobtrusive", Action = "Index", Parent = generic });
            items.Add(new SiteMapItem { Name = "Disabled", Controller = "Disabled", Action = "Index", Parent = generic, Keywords = "enabled" });
            items.Add(new SiteMapItem { Name = "Rtl Demo", Controller = "RtlDemo", Action = "Index", Parent = generic });
            items.Add(new SiteMapItem { Name = "Grid RTL Support", Controller = "GridDemo", Action = "RTLSupport", Parent = generic });
            items.Add(new SiteMapItem { Name = "List Binding", Controller = "ListBinding", Action = "Index", Parent = generic, Keywords = "Phil Haack multi inline"});
            items.Add(new SiteMapItem { Name = "Attributes Demo", Controller = "AttributesDemo", Action = "Index", Parent = generic });
            items.Add(new SiteMapItem { Name = "Error Handling", Controller = "ErrorHandlingDemo", Action = "Index", Parent = generic });
            items.Add(new SiteMapItem { Name = "Client data demo", Controller = "ClientDataLoadingDemo", Action = "Index", Parent = generic });
            items.Add(new SiteMapItem { Name = "Multilevel cascading", Controller = "MultipleLevelCascadingDemo", Action = "Index", Parent = generic });

            var misc = new SiteMapItem { Name = "Misc" };
            items.Add(misc);
            items.Add(new SiteMapItem { Name = "Gantt chart and grid", Controller = "GanttChart", Action = "Index", Parent = misc, Keywords = "google"});
            items.Add(new SiteMapItem { Name = "Pie chart and grid", Controller = "GridChart", Action = "Pie", Parent = misc, Keywords = "google"});
            items.Add(new SiteMapItem { Name = "Wizard Demo", Controller = "WizardDemo", Action = "Index", Parent = misc });
            items.Add(new SiteMapItem { Name = "CRUD inside Lookup", Controller = "CrudInLookup", Action = "Index", Parent = misc });

            items.Add(new SiteMapItem { Name = "Grid export to excel/pdf/txt", Controller = "GridExportToExcelDemo", Action = "Index", Parent = misc, Keywords = "csv"});

            items.Add(new SiteMapItem { Name = "Cascading Grids", Controller = "CascadingGridDemo", Action = "Index", Parent = misc });

            //Items.Add(new SiteMapItem{Name = "Grid maintain selection", Controller = "GridMaintainSelectionDemo", Action = "Index", Parent = misc});
            items.Add(new SiteMapItem { Name = "Grid Sorting", Controller = "GridDemo", Action = "Sorting", Parent = misc });
            items.Add(new SiteMapItem { Name = "Grid selection", Controller = "GridDemo", Action = "Selection", Parent = misc });
            items.Add(new SiteMapItem { Name = "Grid choose columns", Controller = "GridChooseColumnsDemo", Action = "Index", Parent = misc });
            //Items.Add(new SiteMapItem{Title = "Grid show hide columns", Controller = "GridShowHideColumnsDemo", Action = "Index", Parent = misc});
            //items.Add(new SiteMapItem { Name = "Angular Demo", Controller = "AngularDemo", Action = "Index", Parent = misc });
            items.Add(new SiteMapItem { Name = "Knockout.js Demo", Controller = "KnockoutDemo", Action = "Index", Parent = misc });
            items.Add(new SiteMapItem { Name = "Grid Hide columns api", Controller = "GridShowHideColumnsApiDemo", Action = "Index", Parent = misc, 
                Keywords = "show"});

            var more = new SiteMapItem { Name = "More", Collapsed = true };
            items.Add(more);
            items.Add(new SiteMapItem { Name = "Grid Columns Auto Width", Controller = "GridColumnsAutoWidth", Action = "Index",  Parent = more });
            items.Add(new SiteMapItem { Name = "Grid List Count Column", Controller = "GridWithListCountColumn", Action = "Index",  Parent = more, Keywords = "sort order by"});
            items.Add(new SiteMapItem { Name = "Grid Custom Pager", Controller = "CustomPagerGridDemo", Action = "Index",  Parent = more });
            items.Add(new SiteMapItem { Name = "Grid Custom Loading Animation", Controller = "GridNoRecordsFoundCustomLoadingDemo", Action = "Index", Parent = more });
            items.Add(new SiteMapItem { Name = "Grid Array DataSource", Controller = "GridArrayDataSource", Action = "Index", Parent = more });
            items.Add(new SiteMapItem { Name = "aweui", Controller = "AweUi", Action = "Index", Parent = more, Keywords = "react vue.js client angular" });

            items.Add(new SiteMapItem { Name = "About", Controller = "Home", Action = "About", Parent = more, NoMenu = true,
                Keywords = "learn documentation video tutorial price question forum buy cost license installation download" });
        }
    }
}