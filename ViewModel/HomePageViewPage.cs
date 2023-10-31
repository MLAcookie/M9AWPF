using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAA1999WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAA1999WPF.ViewModel
{
    public class HomePageViewModel : ObservableObject
    {
        private readonly ConsoleBehavior consoleBehavior = new();
        public RelayCommand StartMAACommand { get; set; }
        public HomePageViewModel()
        {
            StartMAACommand = new RelayCommand(StartMAA);
        }
        public async void StartMAA()
        {
            ConfigManager.SaveConfig();
            await Task.Run(() => consoleBehavior.Start());
        }
    }
}
