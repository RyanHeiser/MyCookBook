using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class CompositeCommand : CommandBase
    {
        private readonly IEnumerable<CommandBase> _commands;

        public CompositeCommand(params CommandBase[] commands)
        {
            _commands = commands;
        }

        public override void Execute(object? parameter)
        {
            foreach (CommandBase command in _commands)
            {
                command.Execute(parameter);
            }
        }
    }
}
