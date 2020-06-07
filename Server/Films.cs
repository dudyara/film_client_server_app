
namespace Server
{
    class Films
    {
        //конструктор без значений 
        public Films()
        {

        }
        public Films(string name, string director, string country, int year, int cost,int gain,bool oscared)
        {
            Name = name;
            Director = director;
            Country = country;
            Year = year;
            Cost = cost;
            Gain = gain;
            Oscared = oscared;
        }
        // поля объектов класса
        private int _id;
        private string _name;
        private string _director;
        private string _country;
        private int _year;
        private int _cost;
        private int _gain;
        private bool _oscared;

        //свойства полей
        public int Id { get; set; }
        public string Name { get; set; }
        public string Director { get; set; }
        public string Country { get; set; }
        public int Year { get; set; }
        public int Cost { get; set; }
        public int Gain { get; set; }
        public bool Oscared { get; set; }
    }
}
