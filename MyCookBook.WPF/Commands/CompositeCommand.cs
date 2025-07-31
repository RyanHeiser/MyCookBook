using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.WPF.Commands
{
    public class CompositeCommand : CommandBase
    {
        private readonly IEnumerable<ICommand> _commands;

        public CompositeCommand(params ICommand[] commands)
        {
            _commands = commands;
        }

        /// <summary>
        /// Executes multiple commands sequentially.
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object? parameter)
        {
            foreach (ICommand command in _commands)
            {
                command.Execute(parameter);
            }
        }
    }
}
