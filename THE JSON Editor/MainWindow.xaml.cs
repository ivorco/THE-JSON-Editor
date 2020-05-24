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
    public class ComplexValue : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ComplexValue()
        {
            ComplexValues = new ObservableCollection<ComplexValue>();
        }

        private void ComplexValue_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ComplexValues));
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

        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ComplexValue> _complexValues = null;
        public ObservableCollection<ComplexValue> ComplexValues
        {
            get => _complexValues;
            set
            {
                _complexValues = value;
                OnPropertyChanged();
            }
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContextChanged += DataContextLoaded;

            InitializeComponent();
        }

        private void DataContextLoaded(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = (ComplexValue)DataContext;
            var TestComplexSubFirst = new ComplexValue() { Name = "TestComplexSubFirst" };
            TestComplexSubFirst.ComplexValues.Add(new ComplexValue() { Name = "TestComplexSubSubFirst" });
            vm.ComplexValues.Add(TestComplexSubFirst);

            DataContextChanged -= DataContextLoaded;
        }
    }
}
