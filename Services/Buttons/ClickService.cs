using Transliterator.Models;

namespace Transliterator.Services.Buttons
{
    public class ClickService
    {
        private readonly ButtonsSetupService _setup; // ссылка на объект

        private readonly ToolStripMenuItem engRusItem = new("Английский → Русский")
        {
            Checked = true
        };

        private readonly ToolStripMenuItem rusEngItem = new("Русский → Английский");

        public ClickService(ButtonsSetupService setup)
        {
            _setup = setup;
        }

        public ToolStripMenuItem ButtonEngRusCLick()
        {
            engRusItem.Click += (_, _) =>
            {
                _setup.CurrentMode = LayoutMode.EngToRus; // меняем реально поле
                engRusItem.Checked = true;
                rusEngItem.Checked = false;
            };

            return engRusItem;
        }

        public ToolStripMenuItem ButtonRusEngCLick()
        {
            rusEngItem.Click += (_, _) =>
            {
                _setup.CurrentMode = LayoutMode.RusToEng; // меняем реально поле
                engRusItem.Checked = false;
                rusEngItem.Checked = true;
            };
            return rusEngItem;
        }
    }
}