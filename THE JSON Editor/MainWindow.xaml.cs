using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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

        private object _value;
        public object Value
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

    public class ComplexValueTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var window = Application.Current.MainWindow;
            var resName = string.Empty;

            if (item is ComplexValue comp)
            {
                if (comp.ComplexValues.Any())
                {
                    if (comp.Name == "ITEM" && comp.ComplexValues.All(compv => !compv.ComplexValues.Any()))
                        resName = "TableTemplate";
                    else
                        resName = "ComplexValueTemplate";
                }
                else
                    resName = "ValueTemplate";
            }

            return window.Resources[resName] as DataTemplate;
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
            vm.ComplexValues.Add(new ComplexValue { Name = "TestSimpleValueFirst", Value = "abc" });

            DataContextChanged -= DataContextLoaded;
        }

        private void TreeView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var file = files[0];
                var jsonText = File.ReadAllText(file);

                var jss = new JavaScriptSerializer();

                dynamic d = jss.DeserializeObject(jsonText);

                DataContext = GetComplexValueFromDynamic(d);
            }
        }

        private ComplexValue GetComplexValueFromDynamic(dynamic d)
        {
            var value = new ComplexValue();

            if (d is Array arr)
            {
                value.Name = "ROOT";

                var items = arr.OfType<dynamic>().Select(item => GetComplexValueFromDynamic(item));
                foreach (var item in items)
                {
                    value.ComplexValues.Add(item);
                    value.ComplexValues.Last().Name = "ITEM";
                }
            }
            else if (d is Dictionary<string, object> dic)
            {
                foreach (var dicItem in dic)
                {
                    value.ComplexValues.Add(GetComplexValueFromDynamic(dicItem.Value));
                    value.ComplexValues.Last().Name = dicItem.Key;
                }
            }
            else if (d is object obj)
                value.Value = obj;

            return value;
        }
    }
}
