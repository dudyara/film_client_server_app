using System.Linq;
using System.Collections.ObjectModel;


namespace Lab4
{
    class ViewModel
    {
        public ObservableCollection<Films> Conteiner { get; set; }


        public ViewModel()
        {
            Conteiner = new ObservableCollection<Films>();

        }  
        
        public void LoadContainer()
        {
            ListOfFilms listOfFilms = Model.LoadMethod();
            for (int i = 0; i < listOfFilms.FilmsCount(); i++)
            {
                Conteiner.Add(listOfFilms[i]);
            }
            
        }

        public void SaveContainer()
        {
            ListOfFilms listOfFilms = new ListOfFilms();
            for (int i = 0; i < Conteiner.Count(); i++)
            {
                listOfFilms.AddFilm(Conteiner[i]);
            }
            Model.SaveMethod(listOfFilms);
        }


        public void Close()
        {
            Model.CloseMethod();
        }
    }
}
