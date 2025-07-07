using Microsoft.Extensions.DependencyInjection;
using MyCookBook.WPF.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Test.ViewModels
{
    public class CreateRecipeViewModelTest
    {
        [Test]
        public async Task ExecuteSubmitCommand_WithValidInput_CreatesRecipe()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<CreateRecipeViewModel>();
        }
    }
}
