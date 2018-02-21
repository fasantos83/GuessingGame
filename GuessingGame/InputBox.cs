using System.Drawing;
using System.Windows.Forms;

namespace GuessingGame {
    
    /*
     * Classe Auxiliar para gerar input dialog.
     * Retirada do exemplo de:
     * https://stackoverflow.com/questions/1147190/creating-an-inputbox-in-c-sharp-using-forms
     */
    public class InputBox {
        #region Interface
        public static string ShowDialog(string prompt, string title, string defaultValue = null, int? xPos = null, int? yPos = null) {
            InputBoxDialog form = new InputBoxDialog(prompt, title, defaultValue, xPos, yPos);
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.Cancel)
                return null;
            else
                return form.Value;
        }
        #endregion

        #region Auxiliary class
        private class InputBoxDialog : Form {
            public string Value { get { return _txtInput.Text; } }

            private Label _lblPrompt;
            private TextBox _txtInput;
            private Button _btnOk;

            #region Constructor
            public InputBoxDialog(string prompt, string title, string defaultValue = null, int? xPos = null, int? yPos = null) {
                if (xPos == null && yPos == null) {
                    StartPosition = FormStartPosition.CenterParent;
                } else {
                    StartPosition = FormStartPosition.Manual;

                    if (xPos == null) xPos = (Screen.PrimaryScreen.WorkingArea.Width - Width) >> 1;
                    if (yPos == null) yPos = (Screen.PrimaryScreen.WorkingArea.Height - Height) >> 1;

                    Location = new Point(xPos.Value, yPos.Value);
                }

                InitializeComponent();

                if (title == null) title = Application.ProductName;
                Text = title;

                _lblPrompt.Text = prompt;
                Graphics graphics = CreateGraphics();
                Size labelSize = graphics.MeasureString(prompt, _lblPrompt.Font).ToSize();
                int promptWidth = labelSize.Width < 400 ? 400 : labelSize.Width + 10;
                int promptHeight = labelSize.Height;
                _lblPrompt.Size = new Size(promptWidth, promptHeight);

                _txtInput.Location = new Point(12, 30 + promptHeight);
                _txtInput.Size = new Size(promptWidth, 21);
                _txtInput.Text = defaultValue;
                _txtInput.SelectAll();
                _txtInput.Focus();

                Height = 125 + promptHeight;
                Width = promptWidth + 50;

                _btnOk.Location = new Point(promptWidth - 90, 60 + promptHeight);
                _btnOk.Size = new Size(100, 26);

                return;
            }
            #endregion

            #region Methods
            protected void InitializeComponent() {
                _lblPrompt = new Label();
                _lblPrompt.Location = new Point(12, 15);
                _lblPrompt.TabIndex = 0;
                _lblPrompt.BackColor = Color.Transparent;

                _txtInput = new TextBox();
                _txtInput.Size = new Size(100, 20);
                _txtInput.TabIndex = 1;

                _btnOk = new Button();
                _btnOk.TabIndex = 2;
                _btnOk.Size = new Size(75, 26);
                _btnOk.Text = "&OK";
                _btnOk.DialogResult = DialogResult.OK;

                AcceptButton = _btnOk;

                Controls.Add(_lblPrompt);
                Controls.Add(_txtInput);
                Controls.Add(_btnOk);

                FormBorderStyle = FormBorderStyle.FixedDialog;
                AutoSize = true;
                MaximizeBox = false;
                MinimizeBox = false;

                return;
            }
            #endregion
        }
        #endregion
    }
}
