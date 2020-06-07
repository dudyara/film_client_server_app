using System.Linq;
using System.Collections.ObjectModel;


namespace Lab4
{
    class ViewModel
    {
        public ObservableCollection<Films> Container { get; set; }


        public ViewModel()
        {
            Container = new ObservableCollection<Films>();

        }  
        
        public void LoadContainer()
        {
            ListOfFilms listOfFilms = Model.LoadMethod();
            for (int i = 0; i < listOfFilms.FilmsCount(); i++)
            {
                Container.Add(listOfFilms[i]);
            }
            
        }

        public void SaveContainer()
        {
            ListOfFilms listOfFilms = new ListOfFilms();
            for (int i = 0; i < Container.Count(); i++)
            {
                listOfFilms.AddFilm(Container[i]);
            }
            Model.SaveMethod(listOfFilms);
        }


        public void Close()
        {
            Model.CloseMethod();
        }
    }
}
