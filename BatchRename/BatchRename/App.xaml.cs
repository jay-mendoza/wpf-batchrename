// ······································································//
// <copyright file="App.xaml.cs" company="Jay Bautista Mendoza">         //
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.          //
//     THIS IS PART OF MY PERSONAL OPEN SOURCE WPF WINDOW TEMPLATE.      //
//     THIS IS NOT PRIVATE PROPERTY. FEEL FREE TO MODIFY OR USE IT.      //
// </copyright>                                                          //
// ······································································//

namespace SqlToXml
{
    using System.Configuration;
    using System.Windows;
    using JayWpf.Windows;

    /// <summary>Interaction logic for App XAML.</summary>
    public partial class App : Application
    {
        /// <summary>Startup event of the main App.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">StartupEventArgs 'e'.</param>
        private void App_Startup(object sender, StartupEventArgs e)
        {
            WpfWindow mainWindow = new WpfWindow("Views/MainPage.xaml");
            mainWindow.Title = "Batch Rename";
            mainWindow.IconText = "⎡⇛⎦";
            mainWindow.IconTextSize = 17;

            mainWindow.Show();
        }
    }
}
