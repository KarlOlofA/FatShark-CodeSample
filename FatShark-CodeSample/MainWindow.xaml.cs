using FatShark_CodeSample.ViewModels;
using System.Data;
using System.Windows;

namespace FatShark_CodeSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataManagementViewModel _dataManagementViewModel = new();

        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }
        public string DataImportFilePath { get; set; }
        private SampleData[] _sampleData;

        public float PostalCodeSearchRadius = 75;
        public int TopEmailDomainsAmount = 5;
        
        public MainWindow()
        {
            DataContext = this;
            WindowHeight = (int)(SystemParameters.PrimaryScreenHeight*0.75);
            WindowWidth = (int)(SystemParameters.PrimaryScreenWidth*0.75);
            DataImportFilePath = "Select File Path";
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded += Window_Loaded;
        }

        private void SelectFilePathButton_OnClick(object sender, RoutedEventArgs e)
        {
            _dataManagementViewModel.SelectFilePath();
        }

        private void ImportButton_OnClick(object sender, RoutedEventArgs e)
        {
            _sampleData = _dataManagementViewModel.ImportSampleData();
            
            if (_sampleData.Length < 1)
            {
                return;
            }
            
            string?[] dataArray = new string[11];

            DataTable dt = new();
            
            for (int i = 0; i < _sampleData.Length; i++)
            {
                dataArray[0] = _sampleData[i].GetDataType(SampleType.FirstName);
                dataArray[1] = _sampleData[i].GetDataType(SampleType.LastName);
                dataArray[2] = _sampleData[i].GetDataType(SampleType.Company);
                dataArray[3] = _sampleData[i].GetDataType(SampleType.Address);
                dataArray[4] = _sampleData[i].GetDataType(SampleType.City);
                dataArray[5] = _sampleData[i].GetDataType(SampleType.County);
                dataArray[6] = _sampleData[i].GetDataType(SampleType.PostalCode);
                dataArray[7] = _sampleData[i].GetDataType(SampleType.Phone1);
                dataArray[8] = _sampleData[i].GetDataType(SampleType.Phone2);
                dataArray[9] = _sampleData[i].GetDataType(SampleType.Email);
                dataArray[10] = _sampleData[i].GetDataType(SampleType.Webpage);

                if (i == 0)
                {
                    foreach (string? headerRow in dataArray)
                    {
                        char[] strings = ['\"'];
                        dt.Columns.Add(headerRow?.Trim(strings).Replace('_', ' '), typeof(string));
                    }

                    continue;
                }

                dt.Rows.Add(dataArray);

                DataGridView.ItemsSource = new DataView(dt);
            }
            
            Console.WriteLine("Most Common Emails");
            foreach ((string key, int value) in _dataManagementViewModel.GetTopEmailDomains(TopEmailDomainsAmount))
            {
                Console.WriteLine("Email Domain: " + key + " | " + value);   
            }
        }

        private void ProcessPostCodesButton_OnClick(object sender, RoutedEventArgs e)
        {
            _dataManagementViewModel.FetchLocationData(PostalCodeSearchRadius);;
        }
    }
}