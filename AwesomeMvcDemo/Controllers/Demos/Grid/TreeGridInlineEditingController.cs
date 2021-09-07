using System;
using System.Linq;
using System.Web.Mvc;
using AwesomeMvcDemo.Models;
using AwesomeMvcDemo.ViewModels.Input;
using Omu.Awem.Utils;
using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Controllers.Demos.Grid
{
    public class TreeGridInlineEditingController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /*begin*/
        public ActionResult Save(TreeNodeInput input)
        {
            if (!ModelState.IsValid) return Json(ModelState.GetErrorsInline()); ;

            var edit = input.Id.HasValue;
            var ent = edit ? Db.Get<TreeNode>(input.Id) : new TreeNode();
            
            ent.Name = input.Name;

            if (edit)
            {
                Db.Update(ent);
            }
            else
            {
                if (input.ParentId.HasValue)
                {
                    ent.Parent = Db.Get<TreeNode>(input.ParentId);
                }

                Db.Insert(ent);
            }

            return Json(new { Item = MapNode(ent) });
        }

        public ActionResult Delete(int id)
        {
            var node = Db.Get<TreeNode>(id);

            return PartialView(new DeleteConfirmInput
            {
                Id = id,
                Type = "node",
                Name = node.Name
            });
        }

        [HttpPost]
        public ActionResult Delete(DeleteConfirmInput input)
        {
            var node = Db.Get<TreeNode>(input.Id);
            DeleteNode(node);
            
            return Json(new { node.Id });
        }

        public ActionResult CrudTree(GridParams g)
        {
            var roots = Db.TreeNodes.Where(o => o.Parent == null);

            var gmb = new GridModelBuilder<TreeNode>(roots.AsQueryable(), g)
            {
                KeyProp = o => o.Id,
                GetItem = () => // used for grid api updateItem
                {
                    var id = Convert.ToInt32(g.Key);
                    return Db.TreeNodes.FirstOrDefault(o => o.Id == id);
                },
                Map = MapNode
            };

            gmb.GetChildren = (node, nodeLevel) =>
            {
                // non lazy version
                //var children = result.Where(o => o.Parent == node).ToArray();
                //return children.AsQueryable();

                var children = Db.TreeNodes.Where(o => o.Parent == node).AsQueryable();

                children = gmb.OrderBy(children);

                // set this node as lazy when it's above level 1 (relative), it has children, and this is not a lazy request already
                if (nodeLevel > 1 && children.Any() && g.Key == null) return Awe.LazyNode;

                return children;
            };

            return Json(gmb.Build());
        }

        private object MapNode(TreeNode node)
        {
            return new { node.Name, node.Id };
        }

        private void DeleteNode(TreeNode node)
        {
            var children = Db.TreeNodes.Where(o => o.Parent == node).ToList();
            Db.Delete<TreeNode>(node.Id);

            foreach (var child in children)
            {
                DeleteNode(child);
            }
        }
        /*end*/
    }
}