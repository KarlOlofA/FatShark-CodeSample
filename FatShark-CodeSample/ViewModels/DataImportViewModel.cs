using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FatShark_CodeSample.ViewModels
{
    public class DataManagementViewModel
    {
        #region Variables

        private string _apiKey = "https://api.postcodes.io/postcodes/";
        private CoordinateContainer _collectedCoords = new CoordinateContainer();
        private string _filePath = "";
        
        private SampleData[] Data { get; set; }
        private string[] AllEmails { get; set; }
        private string?[] AllWebPages { get; set; }
        private string?[] AllCities { get; set; }
        private string?[] AllCompanies { get; set; }
        private string?[] AllCounties { get; set; }
        private string?[] AllPostalCodes { get; set; }

        private HttpClient _client = new HttpClient();

        #endregion // Variables

        #region Construct

        public DataManagementViewModel()
        {
            _collectedCoords.Coordinates = [];
        }

        #endregion // Construct

        #region Public Methods

        public SampleData[] ImportSampleData()
        {
            return ImportCsvData();
        }
        
        public void SelectFilePath()
        {
            BrowseFilePath();
        }

        public async void FetchLocationData(float searchRadius)
        {
            if (AllPostalCodes == null || AllPostalCodes.Length <= 0)
            {
                return;
            }
            
            await SetCollectedCoordinates();

            if (_collectedCoords.Coordinates.Count > 0)
            {
                CoordinateCluster cluster = GetLargestCoordinateCluster(searchRadius);
                Console.WriteLine("--> Cluster Master: " + cluster.OriginCoordinate.Postcode + " | " + cluster.Coordinates.Count + " <--");
                foreach (var clusterCoordinate in cluster.Coordinates)
                {
                    Console.WriteLine("Cluster Child: " + clusterCoordinate.Postcode);
                }
            }
        }

        public Dictionary<string, int> GetTopEmailDomains(int amount)
        {
            Dictionary<string, int> mostCommonDomains = new Dictionary<string, int>();
            for (int i = 1; i < AllEmails.Length; i++)
            {
                string domainKey = AllEmails[i].Split("@")[^1];
                if (mostCommonDomains.ContainsKey(domainKey))
                {
                    mostCommonDomains[domainKey]++;
                    continue;
                }
                
                mostCommonDomains.Add(domainKey, 1);
            }
            
            Dictionary<string, int> returnDictionary = mostCommonDomains.OrderByDescending(pair => pair.Value).Take(amount).ToDictionary(pair => pair.Key, pair => pair.Value);
            
            return returnDictionary;
        }

        #endregion // Public Methods

        #region Private Methods
        
        private SampleData[] ImportCsvData()
        {
            if (String.IsNullOrEmpty(_filePath))
            {
                return [];
            }
            
            List<SampleData> dataList = [];
            List<string?> emails = [];
            List<string?> webpages = [];
            List<string?> companies = [];
            List<string?> counties = [];
            List<string?> postalCodes = [];
            List<string?> city = [];

            using (StreamReader streamReader = new StreamReader(_filePath))
            {
                while (!streamReader.EndOfStream)
                {
                    string?[] dataArray = SplitWithQuotes(streamReader.ReadLine());

                    SampleData data = new SampleData();
                    data.SetDataType(SampleType.FirstName, dataArray[0]);
                    data.SetDataType(SampleType.LastName, dataArray[1]);
                    data.SetDataType(SampleType.Company, dataArray[2]);
                    data.SetDataType(SampleType.Address, dataArray[3]);
                    data.SetDataType(SampleType.City, dataArray[4]);
                    data.SetDataType(SampleType.County, dataArray[5]);
                    data.SetDataType(SampleType.PostalCode, dataArray[6]);
                    data.SetDataType(SampleType.Phone1, dataArray[7]);
                    data.SetDataType(SampleType.Phone2, dataArray[8]);
                    data.SetDataType(SampleType.Email, dataArray[9]);
                    data.SetDataType(SampleType.Webpage, dataArray[10]);

                    emails.Add(data.GetDataType(SampleType.Email));
                    webpages.Add(data.GetDataType(SampleType.Webpage));
                    counties.Add(data.GetDataType(SampleType.County));
                    postalCodes.Add(data.GetDataType(SampleType.PostalCode));
                    city.Add(data.GetDataType(SampleType.City));
                    companies.Add(data.GetDataType(SampleType.Company));
                    
                    dataList.Add(data);
                }
                AllEmails = emails.ToArray();
                AllWebPages = webpages.ToArray();
                AllCounties = counties.ToArray();
                AllPostalCodes = postalCodes.ToArray();
                AllCompanies = companies.ToArray();
                AllCities = city.ToArray();
                
            }
            return dataList.ToArray();
        }

        void BrowseFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            
            if (String.IsNullOrEmpty(openFileDialog.FileName))
            {
                return;
            }

            _filePath = openFileDialog.FileName;
        }
        
        static string?[] SplitWithQuotes(string data)
        {
            List<string> segments = [];
            string currentSegment = "";
            bool inQuotes = false;

            foreach (char character in data)
            {
                if (character == '"')
                {
                    inQuotes = !inQuotes; // Toggle quote state
                }
                else if (character == ',' && !inQuotes)
                {
                    segments.Add(currentSegment.Trim()); // Add segment, remove whitespace
                    currentSegment = "";
                }
                else
                {
                    currentSegment += character;
                }
            }
            segments.Add(currentSegment.Trim()); // Add the last segment
            return segments.ToArray();
        }

        // Gets distance from latitude / longitude
        private double GetDistanceBetweenCoordinates(Coordinate latLong1, Coordinate latLong2)
        {
            double dTheta = DegreeToRadian(latLong2.Latitude - latLong1.Latitude) / 2.0;
            double dPhi = DegreeToRadian(latLong2.Longitude - latLong1.Longitude) / 2.0;

            double sinTheta = Math.Sin(dTheta);
            double sinPhi = Math.Sin(dPhi);

            double a = Math.Pow(sinTheta, 2) + Math.Cos(DegreeToRadian(latLong1.Latitude)) * Math.Cos(DegreeToRadian(latLong2.Latitude)) * Math.Pow(sinPhi, 2);
            double c = 2 * Math.Asin(Math.Sqrt(a));

            return c * 6371;
        }

        private static double DegreeToRadian(double degree)
        {
            return degree * (Math.PI / 180.0);
        }
        
        private async Task GetPostcodesAsync(List<string> postcodes, string filter = null)
        {
            const int itemsToFetch = 98;
            Dictionary<string, string[]> queryParameters = new Dictionary<string, string[]>
            {
                { "postcodes", postcodes[..itemsToFetch].ToArray()},
            };
            
            string url = "https://api.postcodes.io/postcodes/";
            if (!string.IsNullOrEmpty(filter))
            {
                url += $"?filter={filter}";
            }

            var json = JsonConvert.SerializeObject(queryParameters);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            
            var responseString = await response.Content.ReadAsStringAsync();
            
            for (int i = 0; i < itemsToFetch; i++)
            {
                var data  = JObject.Parse(responseString)["result"]?[i]?["result"];
                
                if (data == null || data.Type == JTokenType.Null) continue;
                
                var coordinate = new Coordinate
                {
                    Postcode = data["postcode"].ToString(),
                    Longitude = data["longitude"]?.ToObject<double>() ?? 0.0,
                    Latitude = data["latitude"]?.ToObject<double>() ?? 0.0,
                };
                
                _collectedCoords.Coordinates.Add(coordinate);
            }
        }

        private async Task SetCollectedCoordinates()
        {
            List<string> tempContainer = AllPostalCodes.ToList();
            tempContainer.RemoveAt(0);

            await GetPostcodesAsync(tempContainer, "postcode,longitude,latitude");
        }

        private CoordinateCluster GetLargestCoordinateCluster(float searchRadius)
        {
            var currentCluster = new CoordinateCluster();
            List<Coordinate> tempCoordinates = [];

            for (int i = 0; i < _collectedCoords.Coordinates.Count; i++)
            {
                tempCoordinates.Clear();
                for (int j = 0; j < _collectedCoords.Coordinates.Count; j++)
                {
                    if (_collectedCoords.Coordinates[i] == _collectedCoords.Coordinates[j])
                    {
                        continue;
                    }

                    if (GetDistanceBetweenCoordinates(_collectedCoords.Coordinates[i], _collectedCoords.Coordinates[j]) < searchRadius)
                    {
                        tempCoordinates.Add(_collectedCoords.Coordinates[j]);
                    }

                }

                if (tempCoordinates.Count > currentCluster.Coordinates.Count)
                {
                    currentCluster.OriginCoordinate = _collectedCoords.Coordinates[i];
                    currentCluster.Coordinates = [..tempCoordinates];
                }
            }

            return currentCluster;
        }
        
        #endregion // Private Methods
    }
}
