using System.Linq;
using System.Collections.ObjectModel;


namespace Lab4
{
    class ViewModel
    {
        public ObservableCollection<Films> Conteiner { get; set; }
        //public List<Animals> Conteiner { get; set; }


        public ViewModel()
        {
            Conteiner = new ObservableCollection<Films>();
            //Conteiner = new List<Animals>();

        }  
        
        public void LoadConteiner()
        {
            ListOfFilms listOfAnimals = Model.LoadMethod();
            for (int i = 0; i < listOfAnimals.FilmsCount(); i++)
            {
                Conteiner.Add(listOfAnimals[i]);
            }
            
        }

        public void SaveConteiner()
        {
            ListOfFilms listOfAnimals = new ListOfFilms();
            for (int i = 0; i < Conteiner.Count(); i++)
            {
                listOfAnimals.AddFilm(Conteiner[i]);
            }
            Model.SaveMethod(listOfAnimals);
        }


        public void Close()
        {
            Model.CloseMethod();
        }
    }
}
