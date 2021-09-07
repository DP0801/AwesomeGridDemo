namespace AwesomeMvcDemo.Controllers.Demos.Grid
{
    public class BaseGridModel
    {
        public string search { get; set; }
        public string orderby { get; set; }
        public int? pagenumber { get; set; }
        public int pagesize { get; set; }
    }
}