using MyData;
using System.Windows;
using System.Windows.Input;

namespace Mydata
{
     
    public class NotifyIconViewModel
    {
         
        public ICommand ShowWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                     
                    CommandAction = () =>
                    {
                        //Application.Current.MainWindow = new MainWindow();
                        Application.Current.MainWindow.Show();
                    }
                };
            }
        }

        public ICommand HideWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => Application.Current.MainWindow.Hide(),
                    CanExecuteFunc = () => Application.Current.MainWindow != null
                };
            }
        }

        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand {CommandAction = () => Application.Current.Shutdown()};
            }
        }
    }
}
