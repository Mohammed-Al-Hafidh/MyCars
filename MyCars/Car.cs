namespace MyCars
{
    public class Car
    {
        public string MakeModel { get; set; }
        public double EngineSize { get; set; }
        public string FuelType { get; set; }
        public Car(string makeModel,double engineSize, string fuelType)
        {
            this.MakeModel = makeModel;
            this.EngineSize = engineSize;
            this.FuelType = fuelType;
        }
        public override string ToString()
        {
            return string.Format("{0};{1};{2}", this.MakeModel, this.EngineSize, this.FuelType);
        }
        public string ToDataString()
        {
            return string.Format("{0};{1};{2}", this.MakeModel, this.EngineSize, this.FuelType);
        }
        public string ToCSVString()
        {
            return string.Format("{0},{1},{2}", this.MakeModel, this.EngineSize, this.FuelType);
        }


    }
}
