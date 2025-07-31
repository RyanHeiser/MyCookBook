using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class RemoveImageCommand : CommandBase
    {
        private readonly CreateRecipeViewModel _viewModel;

        public RemoveImageCommand(CreateRecipeViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        /// <summary>
        /// Checks if the command can be executed.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>True if the command can be executed.</returns>
        public override bool CanExecute(object? parameter)
        {
            return _viewModel.HasImage;
        }

        /// <summary>
        /// Removes the image currently saved in memory on CreateRecipeView.
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object? parameter)
        {
            _viewModel.RawImageData = null;
            _viewModel.RawThumbnailData = null;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.HasImage))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
