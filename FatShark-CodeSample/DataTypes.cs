using System.Numerics;

namespace FatShark_CodeSample
{
    public class SampleData
    {
        private string? FirstName { get; set; }
        private string? LastName { get; set; }
        private string? Company { get; set; }
        private string? Address { get; set; }
        private string? City { get; set; }
        private string? County { get; set; }
        private string? PostalCode { get; set; }
        private string? Phone1 { get; set; }
        private string? Phone2 { get; set; }
        private string? Email { get; set; }
        private string? Webpage { get; set; }

        public string? GetDataType(SampleType sampleType) 
        {
            return sampleType switch
            {
                SampleType.FirstName => FirstName,
                SampleType.LastName => LastName,
                SampleType.Company => Company,
                SampleType.Address => Address,
                SampleType.City => City,
                SampleType.County => County,
                SampleType.PostalCode => PostalCode,
                SampleType.Phone1 => Phone1,
                SampleType.Phone2 => Phone2,
                SampleType.Email => Email,
                SampleType.Webpage => Webpage,
                _ => "No Value",
            };
        }
        public void SetDataType(SampleType sampleType, string? value = "")
        {

            switch (sampleType)
            {
                case SampleType.FirstName:
                    FirstName = value; 
                    break;
                case SampleType.LastName:
                    LastName = value;
                    break;
                case SampleType.Company:
                    Company = value;
                    break;
                case SampleType.Address:
                    Address = value;
                    break;
                case SampleType.City:
                    City = value;
                    break;
                case SampleType.County:
                    County = value;
                    break;
                case SampleType.PostalCode:
                    PostalCode = value;
                    break;
                case SampleType.Phone1:
                    Phone1 = value;
                    break;
                case SampleType.Phone2:
                    Phone2 = value;
                    break;
                case SampleType.Email:
                    Email = value;
                    break;
                case SampleType.Webpage:
                    Webpage = value;
                    break;
            }
        }
    }

    public enum SampleType
    {
        FirstName,
        LastName, 
        Company, 
        Address, 
        City, 
        County, 
        PostalCode, 
        Phone1, 
        Phone2,
        Email,
        Webpage,
    }


    public class Coordinate()
    {
        public string Postcode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
    
    public class CoordinateContainer()
    {
        public List<Coordinate> Coordinates { get; set; }
    }
    
    public class CoordinateCluster{

        public CoordinateCluster()
        {
            Coordinates = [];
            Inhabitants = [];
        }
        public Coordinate OriginCoordinate { get; set; }
        public List<Coordinate> Coordinates { get; set; }
        public List<string> Inhabitants { get; set; }
    }
}
