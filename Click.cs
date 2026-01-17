namespace Transliterator
{
    public class Click
    {
        private ButtonsSetup _setup; // ссылка на объект

        private ToolStripMenuItem engRusItem = new("Английский → Русский")
        {
            Checked = true
        };

        private ToolStripMenuItem rusEngItem = new("Русский → Английский");

        public Click(ButtonsSetup setup)
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