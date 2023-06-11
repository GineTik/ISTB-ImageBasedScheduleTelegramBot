using Telegram.Bot.Types.ReplyMarkups;

namespace ISTB.Framework.TelegramBotApplication.Builders
{
    public class InlineKeyboardBuilder
    {
        private readonly List<List<InlineKeyboardButton>> _buttons;
        private List<InlineKeyboardButton> _currentRow;

        public InlineKeyboardBuilder()
        {
            _buttons = new List<List<InlineKeyboardButton>>();
            _currentRow = new List<InlineKeyboardButton>();
        }

        public InlineKeyboardBuilder ButtonRange(IEnumerable<InlineKeyboardButton> buttons, int rowCount = 1)
        {
            var stack = new Queue<InlineKeyboardButton>(buttons);

            while (stack.Count > 0)
            {
                for (int i = 0; i < rowCount && stack.Count > 0; i++)
                {
                    Button(stack.Dequeue());
                }
                EndRow();
            }

            return this;
        }

        public InlineKeyboardBuilder CallbackButton(string text, string callback)
        {
            _currentRow.Add(InlineKeyboardButton.WithCallbackData(text, callback));
            return this;
        }

        public InlineKeyboardBuilder Button(InlineKeyboardButton button)
        {
            _currentRow.Add(button);
            return this;
        }

        public InlineKeyboardBuilder EndRow()
        {
            _buttons.Add(_currentRow);
            _currentRow = new List<InlineKeyboardButton>();
            return this;
        }

        public InlineKeyboardMarkup Build()
        {
            return new InlineKeyboardMarkup(_buttons);
        }
    }
}
