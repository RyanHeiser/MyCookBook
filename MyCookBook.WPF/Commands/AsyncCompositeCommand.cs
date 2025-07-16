using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.WPF.Commands
{
    public class AsyncCompositeCommand : AsyncCommandBase
    {
        private readonly IEnumerable<AsyncCommandBase> _commands;

        public AsyncCompositeCommand(params AsyncCommandBase[] commands)
        {
            _commands = commands;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            foreach (AsyncCommandBase command in _commands)
            {
                await command.ExecuteAsync(parameter);
            }
        }
    }
}
