using System.ComponentModel;
using FatShark_CodeSample.ViewModels;
using System.Data;
using System.Windows;

namespace FatShark_CodeSample
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private DataManagementViewModel _dataManagementViewModel = new();

        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }
        public string DataImportFilePath { get; set; }

        private string _masterClusterName = "Master Cluster: ";
        private SampleData[] _sampleData;
        private float _masterClusterSearchRadius = 75;
        private int _topEmailDomainsAmount = 5;
        
        public MainWindow()
        {
            WindowHeight = (int)(SystemParameters.PrimaryScreenHeight*0.75);
            WindowWidth = (int)(SystemParameters.PrimaryScreenWidth*0.75);;
            DataImportFilePath = "File Path: ";
            InitializeComponent();
            DataContext = this;
            MasterClusterTb.DataContext = this;
            DataImportFilePathTb.DataContext = this;
        }
        public string FilePath  
        {  
            get { return DataImportFilePath; }  
            set  
            {  
                if (value != DataImportFilePath)  
                {  
                    DataImportFilePath = value;  
                    OnPropertyChanged("DataImportFilePath");  
                }  
            }  
        }  
        public string MasterClusterName  
        {  
            get { return _masterClusterName; }  
            set  
            {  
                if (value != _masterClusterName)  
                {  
                    _masterClusterName = value;  
                    OnPropertyChanged("MasterClusterName");  
                }  
            }  
        }  
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)  
        {  
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  
        }  

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded += Window_Loaded;
        }

        private void SelectFilePathButton_OnClick(object sender, RoutedEventArgs e)
        {
            FilePath = "File Path: " + _dataManagementViewModel.SelectFilePath();
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
            foreach ((string key, int value) in _dataManagementViewModel.GetTopEmailDomains(_topEmailDomainsAmount))
            {
                Console.WriteLine("Email Domain: " + key + " | " + value);   
            }
        }

        private async void ProcessPostCodesButton_OnClick(object sender, RoutedEventArgs e)
        {
            await _dataManagementViewModel.FetchLocationData(_masterClusterSearchRadius);;

            CoordinateCluster cluster = _dataManagementViewModel.GetLargestCluster(_masterClusterSearchRadius);

            if (cluster.Coordinates.Count <= 0)
            {
                return;
            }
            
            DataTable dt = new();

            MasterClusterName = "Master Cluster: " + cluster.OriginCoordinate.Postcode;
            dt.Columns.Add("Postal codes in master cluster");
            dt.Columns.Add("Inhabitants of master cluster");
            if (cluster.Inhabitants.Count > cluster.Coordinates.Count)
            {
                for (int i = 0; i < cluster.Inhabitants.Count; i++)
                {
                    var postCode = "";
                    if (cluster.Coordinates.Count > i && !string.IsNullOrEmpty(cluster.Coordinates[i].Postcode))
                    {
                        postCode = cluster.Coordinates[i].Postcode;
                    }
                    
                    string[] tempElement = { postCode,  cluster.Inhabitants[i]};
                    dt.Rows.Add(tempElement);
                }
            }
            else
            {
                for (int i = 0; i < cluster.Coordinates.Count; i++)
                {
                    var inhabitant = "";
                    Console.WriteLine(cluster.Inhabitants.Count + " | " + i);
                    if (cluster.Inhabitants.Count > i && !string.IsNullOrEmpty(cluster.Inhabitants[i]))
                    {
                        inhabitant = cluster.Inhabitants[i];
                    }
                    
                    string[] tempElement = { cluster.Coordinates[i].Postcode,  inhabitant};
                    dt.Rows.Add(tempElement);
                }
            }
            
            PostCodeGridView.ItemsSource = new DataView(dt);
        }
    }
}