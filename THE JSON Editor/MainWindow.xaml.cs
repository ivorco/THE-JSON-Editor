using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace THE_JSON_Editor
{
    public class Complex : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Complex()
        {
            Complexes = new ObservableCollection<Complex>();
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Complex> _complex = null;
        public ObservableCollection<Complex> Complexes
        {
            get => _complex;
            set
            {
                _complex = value;
                OnPropertyChanged();
            }
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<Complex> _complex = null;
        public ObservableCollection<Complex> Complexes
        {
            get => _complex;
            set
            {
                _complex = value;
                OnPropertyChanged();
            }
        }

        public ViewModel()
        {
            Complexes = new ObservableCollection<Complex>();
            Complexes.Add(new Complex() { Name = "TestComplexSub" });
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
