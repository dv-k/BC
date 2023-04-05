using BlackCleaner.WPF.Views;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BlackCleaner.WPF
{

        /// <summary>
        /// Interaction logic for App.xaml
        /// </summary>
        public partial class App
        {

            /// <inheritdoc />
            protected override void RegisterTypes(IContainerRegistry containerRegistry)
            {
           
            }

            /// <inheritdoc />
            protected override Window CreateShell()
            {
                return Container.Resolve<Shell>();
            }
    }
    
}
