using MyCookBook.Domain.Models;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores
{
    public class MoveCopyStore
    {
        private DomainObject? _current;
        public DomainObject? Current
        {
            get => _current;

            set
            {
                _current = value;
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
                    IsCopying = false;
                    OnMoveCopyStarted();
                }
            }
        }

        private bool _isCopying;
        public bool IsCopying
        {
            get => _isCopying;

            set
            {
                _isCopying = value;
                if (value)
                {
                    IsMoving = false;
                    OnMoveCopyStarted();
                }
            }
        }

        public event Action? MoveCopyStarted;

        public MoveCopyStore()
        {
            
        }

        private void OnMoveCopyStarted()
        {
            MoveCopyStarted?.Invoke();
        }
    }
}
