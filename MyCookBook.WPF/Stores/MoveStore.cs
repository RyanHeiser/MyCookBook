using MyCookBook.Domain.Models;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores
{
    public class MoveStore
    {
        private ChildDomainObject? _current;
        public ChildDomainObject? Current
        {
            get => _current;

            set
            {
                _current = value;
                OnMoveUpdated();
            }
        }

        private bool _isMoving;
        public bool IsMoving
        {
            get => _isMoving;

            set
            {
                _isMoving = value;
                if (value)
                {
                    OnMoveStarted();
                }
                OnMoveUpdated();
            }
        }

        public event Action? MoveUpdated;
        public event Action? MoveStarted;

        public MoveStore()
        {
            
        }

        private void OnMoveUpdated()
        {
            MoveUpdated?.Invoke();
        }


        private void OnMoveStarted()
        {
            MoveStarted?.Invoke();
        }
    }
}
