using System.Web;
using System.Web.Mvc;
using Omu.Awem.Helpers;
using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Helpers.Awesome
{
    public static class CrudHelpers
    {
        private static UrlHelper GetUrlHelper<T>(HtmlHelper<T> html)
        {
            return new UrlHelper(html.ViewContext.RequestContext);
        }

        /*beging*/
        /// <summary>
        /// initialize PopupForms for grid crud
        /// </summary>
        /// <param name="html"></param>
        /// <param name="gridId"></param>
        /// <param name="crudController">controller containing the crud actions</param>
        /// <param name="createPopupHeight">height of the create/edit popup</param>
        /// <param name="maxWidth"> max popup width</param>
        /// <param name="reload">reload grid after save/delete action success</param>
        /// <param name="area"></param>
        /// <param name="inlineContId">html id for the inline popup container</param>
        public static IHtmlString InitCrudPopupsForGrid<T>(
            this HtmlHelper<T> html,
            string gridId,
            string crudController,
            int createPopupHeight = 430,
            int maxWidth = 0,
            bool reload = false,
            string area = null,
            string inlineContId = null)
        {
            var url = GetUrlHelper(html);
            gridId = html.Awe().GetContextPrefix() + gridId;

            var refreshGrid = "refreshGrid";
            var format = "utils.{0}('" + gridId + "')";

            var createFunc = string.Format(format, reload ? refreshGrid : "itemCreated");
            var editFunc = string.Format(format, reload ? refreshGrid : "itemEdited");
            var delFunc = string.Format(format, reload ? refreshGrid : "itemDeleted");
            var delConfirmFunc = string.Format(format, "delConfirmLoad");

            var create = html.Awe()
                    .InitPopupForm()
                    .Name("create" + gridId)
                    .Group(gridId)
                    .Height(createPopupHeight)
                    .MaxWidth(maxWidth)
                    .Url(url.Action("Create", crudController, new { area }))
                    .Title("Create item")
                    .Modal()
                    .Success(createFunc);

            var edit = html.Awe()
                .InitPopupForm()
                .Name("edit" + gridId)
                .Group(gridId)
                .Height(createPopupHeight)
                .MaxWidth(maxWidth)
                .Url(url.Action("Edit", crudController, new { area }))
                .Title("Edit item")
                .Modal()
                .Success(editFunc);

            var delete = html.Awe()
                  .InitPopupForm()
                  .Name("delete" + gridId)
                  .Group(gridId)
                  .Url(url.Action("Delete", crudController, new { area }))
                  .Title("Delete item")
                  .Success(delFunc)
                  .OnLoad(delConfirmFunc) // calls grid.api.select and animates the row
                  .Height(200)
                  .Modal();

            if (inlineContId != null)
            {
                create.Mod(o => o.Inline(inlineContId).ShowHeader(false));
                edit.Mod(o => o.Inline(inlineContId).ShowHeader(false));
                delete.Mod(o => o.Inline(inlineContId).ShowHeader(false));
            }

            var result = create.ToString() + edit + delete;

            return new MvcHtmlString(result);
        }
        /*endg*/

        /// <summary>
        /// initialize PopupForms for grid nest crud
        /// </summary>
        /// <param name="html"></param>
        /// <param name="gridId"></param>
        /// <param name="crudController">controller containing the crud actions</param>
        /// <param name="reload">reload grid after save/delete action success</param>
        /// <param name="area"></param>
        public static IHtmlString InitCrudForGridNest<T>(
            this HtmlHelper<T> html,
            string gridId,
            string crudController,
            bool reload = false,
            string area = null)
        {
            var url = GetUrlHelper(html);
            gridId = html.Awe().GetContextPrefix() + gridId;

            var refreshGrid = "refreshGrid";
            var format = "utils.{0}('" + gridId + "')";

            var createFunc = string.Format(format, reload ? refreshGrid : "itemCreated");
            var editFunc = string.Format(format, reload ? refreshGrid : "itemEdited");
            var delFunc = string.Format(format, reload ? refreshGrid : "itemDeleted");

            var result =
                html.Awe()
                    .InitPopupForm()
                    .Name("create" + gridId)
                    .Group(gridId)
                    .Url(url.Action("Create", crudController, new { area }))
                    .Mod(o => o.Inline().ShowHeader(false))
                    .Success(createFunc)
                    .ToString()
                + html.Awe()
                      .InitPopupForm()
                      .Name("edit" + gridId)
                      .Group(gridId)
                      .Url(url.Action("Edit", crudController, new { area }))
                      .Mod(o => o.Inline().ShowHeader(false))
                      .Success(editFunc)
                + html.Awe()
                      .InitPopupForm()
                      .Name("delete" + gridId)
                      .Group(gridId)
                      .Url(url.Action("Delete", crudController, new { area }))
                      .Mod(o => o.Inline().ShowHeader(false))
                      .Success(delFunc);

            return new MvcHtmlString(result);
        }

        /*beginal*/
        public static IHtmlString InitCrudPopupsForAjaxList<T>(
           this HtmlHelper<T> html,
           string ajaxListId,
           string controller,
           string popupName)
        {
            var url = GetUrlHelper(html);

            var result =
                html.Awe()
                    .InitPopupForm()
                    .Name("create" + popupName)
                    .Url(url.Action("Create", controller))
                    .Height(430)
                    .Success("utils.itemCreatedAlTbl('" + ajaxListId + "')")
                    .Group(ajaxListId)
                    .Title("create item")
                    .ToString()

                + html.Awe()
                      .InitPopupForm()
                      .Name("edit" + popupName)
                      .Url(url.Action("Edit", controller))
                      .Height(430)
                      .Success("utils.itemEditedAl('" + ajaxListId + "')")
                      .Group(ajaxListId)
                      .Title("edit item")

                + html.Awe()
                      .InitPopupForm()
                      .Name("delete" + popupName)
                      .Url(url.Action("Delete", controller))
                      .Success("utils.itemDeletedAl('" + ajaxListId + "')")
                      .Group(ajaxListId)
                      .OkText("Yes")
                      .CancelText("No")
                      .Height(200)
                      .Title("delete item");

            return new MvcHtmlString(result);
        }
        /*endal*/

        /// <summary>
        /// initialize Delete PopupForms for grid
        /// </summary>
        /// <param name="html"></param>
        /// <param name="gridId"></param>
        /// <param name="crudController">controller containing the crud actions</param>
        /// <param name="action">delete action name</param>
        /// <param name="reload">reload grid after delete action success</param>
        /// <param name="area"></param>
        /// <param name="url">delete action url</param>
        public static IHtmlString InitDeletePopupForGrid<T>(
            this HtmlHelper<T> html,
            string gridId,
            string crudController = null,
            string action = "Delete",
            bool reload = false,
            string area = null,
            string url = null)
        {
            var urlh = GetUrlHelper(html);
            gridId = html.Awe().GetContextPrefix() + gridId;

            var utilf = "utils.{0}('" + gridId + "')";

            var delFunc = string.Format(utilf, reload ? "refreshGrid" : "itemDeleted");
            var delConfirmFunc = string.Format(utilf, "delConfirmLoad");

            var result =
                html.Awe()
                    .InitPopupForm()
                    .Name("delete" + gridId)
                    .Group(gridId)
                    .Url(url ?? urlh.Action(action, crudController, new { area }))
                    .Success(delFunc)
                    .OnLoad(delConfirmFunc) // delConfirmFunc calls grid.api.select and animates the row
                    .Height(200)
                    .Modal()
                    .ToString();

            return new MvcHtmlString(result);
        }
    }
}