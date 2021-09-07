using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.ViewModels.Display
{
    public class MealDisplay : KeyContent
    {
        public MealDisplay(object key, string content, string url, int catId)
            : base(key, content)
        {
            this.url = url;
            this.catId = catId;
        }

        public MealDisplay(object key, string content, string url)
            : base(key, content)
        {
            this.url = url;
        }

        public string url { get; set; }

        public int catId { get; set; }
    }
}