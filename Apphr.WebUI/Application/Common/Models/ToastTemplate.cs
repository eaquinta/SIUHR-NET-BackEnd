namespace Apphr.Application.Common.Models
{
    public class ToastTemplate
    {
        public ToastTemplate(bool autohide = true)
        {
            this.Autohide = autohide;
            this.Delay = 3000;
            this.Type = "info";
        }

        public ToastTemplate(ToastTemplate template)
        {
            this.Image = template.Image;
            this.Title = template.Title;
            this.Subtitle = template.Subtitle;
            this.Message = template.Message;
            this.Autohide = template.Autohide;
            this.Delay = template.Delay;
        }

        public string Image { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Message { get; set; }
        public bool Autohide { get; set; }
        public int Delay { get; set; }
        public string Type { get; set; }
    }
}
