using System;
using System.Collections.Generic;
using Omu.AwesomeMvc;

namespace Omu.Awem.Helpers
{
    /// <summary>
    /// Grid mods configuration
    /// </summary>
    public class GridModCfg<T>
    {
        private readonly GridModInfo info = new GridModInfo();
        private readonly AweInfo awe;
        private readonly Grid<T> grid;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridAwe"></param>
        /// <param name="grid"></param>
        public GridModCfg(AweInfo gridAwe, Grid<T> grid)
        {
            awe = gridAwe;
            this.grid = grid;
        }

        /// <summary>
        /// automatically go to next/prev page when scrolling to the end/beginning of the page
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> InfiniteScroll(bool moreBtn = false)
        {
            info.InfiniteScroll = true;
            info.MoreBtn = moreBtn;
            return this;
        }

        /// <summary>
        /// add page info ( page 1 of 75 ) in the right bottom corner of the grid
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> PageInfo()
        {
            info.PageInfo = true;
            return this;
        }

        /// <summary>
        /// add common mods, PageInfo, PageSize, ColumnsSelector, AutoMiniPager, Loading
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> Main(bool colSel = true)
        {
            PageInfo();
            PageSize();
            AutoMiniPager();
            Loading();

            if (colSel)
            {
                ColumnsSelector();
            }

            return this;
        }

        /// <summary>
        /// Freeze grid columns
        /// </summary>
        /// <param name="left">left side</param>
        /// <param name="right">right side</param>
        /// <param name="minsw">minimum scrolling space required to remain after freezing columns (defaults to 200)</param>
        /// <returns></returns>
        public GridModCfg<T> Freeze(int left = 2, int right = 0, int minsw = 0)
        {
            info.FzLeft = left;
            info.FzRight = right;
            info.Minsw = minsw;
            return this;
        }

        /// <summary>
        /// automatically switch the pager to a more compact version on smaller screens, or when resizing the browser to smaller size
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> AutoMiniPager()
        {
            info.AutoMiniPager = true;
            return this;
        }

        /// <summary>
        /// use minipager
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> MiniPager()
        {
            info.MiniPager = true;
            return this;
        }

        /// <summary>
        /// add page size selector 
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> PageSize()
        {
            info.PageSize = true;
            return this;
        }

        /// <summary>
        /// add columns selector dropdown
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> ColumnsSelector()
        {
            info.ColumnsSelector = true;
            return this;
        }

        /// <summary>
        /// Autohide columns
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> ColumnsAutohide()
        {
            info.ColumnsAutohide = true;
            return this;
        }

        /// <summary>
        /// Autohide columns
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> KeyNav()
        {
            info.KeyNav = true;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> CustomRender(string modFunc)
        {
            grid.ContentStr = "<div class=\"awe-itc\"></div>";
            info.CustomRender = true;
            info.modFunc = modFunc;
            return this;
        }

        /// <summary>
        /// set Inline editing urls
        /// </summary>
        /// <param name="saveUrl">url used create and edit if editUrl is not specified</param>
        /// <param name="editUrl"></param>
        /// <param name="oneRow">inline edit one row at a time</param>
        /// <param name="reloadOnSave">reload the grid on save (disabled when RowClickEdit is true)</param>
        /// <param name="rowClickEdit">edit row on click</param>
        /// <param name="clientOpt">js object containing client options: save, edit { save: function }</param>
        /// <returns></returns>
        public GridModCfg<T> InlineEdit(string saveUrl = null, string editUrl = null, bool oneRow = false, bool reloadOnSave = false, bool rowClickEdit = false, string clientOpt = null)
        {
            info.InlineEdit = true;
            info.CreateUrl = saveUrl;
            info.EditUrl = editUrl;
            info.OneRow = oneRow;
            info.RelOnSav = reloadOnSave;
            info.RowClickEdit = rowClickEdit;
            info.ClientSave = clientOpt;

            if (rowClickEdit)
            {
                info.RelOnSav = false;
            }

            return this;
        }

        /// <summary>
        /// Batch save inline grid editing
        /// </summary>
        /// <param name="saveUrl"></param>
        /// <param name="rowClickEdit">edit on row click (default true)</param>
        /// <param name="clientOpt">js object containing client options: save, edit { save: function }</param>
        /// <returns></returns>
        public GridModCfg<T> BatchEdit(string saveUrl = null, bool rowClickEdit = true, string clientOpt = null)
        {
            info.InlineEdit = true;
            info.Batch = true;
            info.CreateUrl = saveUrl;
            info.RowClickEdit = rowClickEdit;
            info.ClientSave = clientOpt;

            return this;
        }

        /// <summary>
        /// Add filter row to the grid
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> FilterRow(string keyupsel = "", bool clearBtns = true)
        {
            info.FilterOpt = new GridFilterOpt
            {
                keyupsel = keyupsel, 
                clearBtns = clearBtns
            };

            return this;
        }

        /// <summary>
        /// set Loading animation
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> Loading(bool enabled = true, bool showEmptyMsg = true, string empMsg = null)
        {
            info.Loading = true;
            info.LdngDisb = !enabled;
            info.NoEmpMsg = !showEmptyMsg;
            info.EmptyMessage = empMsg;
            return this;
        }

        /// <summary>
        /// movable grid rows
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> MovableRows(params string[] gridIds)
        {
            info.MovableRows = true;
            info.GridIds = gridIds;
            return this;
        }

        /// <summary>
        /// movable grid rows
        /// </summary>
        /// <returns></returns>
        public GridModCfg<T> MovableRows(Action<MovableRowsCfg> o)
        {
            var cfg = new MovableRowsCfg(awe);
            o(cfg);

            info.MovableRows = true;
            info.GridIds = cfg.GetIds();
            return this;
        }

        /// <summary>
        /// set mod by string func name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GridModCfg<T> Custom(string name)
        {
            if (info.CustomMods == null)
            {
                info.CustomMods = new List<string> { name };
            }
            else
            {
                info.CustomMods.Add(name);
            }

            return this;
        }

        internal GridModInfo GetInfo()
        {
            return info;
        }
    }
}