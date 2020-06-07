using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ConteinerOfFilms
    {
        //хралинилище для объектов класса
        private List<Films> _films = new List<Films>();
        //индексатор для хранилища (можно обращаться по индексу)
        public Films this[int index]
        {
            get
            {
                return _films[index];
            }
            set
            {
                _films[index] = value;
            }
        }
        //методы для работы с хранилищем
        //метод добавления объекта класса в хранилище
        public void AddFilm(Films film)
        {
            _films.Add(film);
        }
        //метод удаления объекта класса из хралилища по индексу
        public void RemoveFilm(int index)
        {
            _films.RemoveAt(index);
        }
        // метод возвращения хранилища
        public List<Films> GetFilms()
        {
            return _films;
        }
        //очищение хралилища
        public void ClearFilms()
        {
            _films.Clear();
        }
        //возвращение объекта класса из хранилища по индексу
        public Films GetFilm(int index)
        {
            return _films[index];
        }
        //подсчёт количества объектов класса в хранилище
        public int FilmsCount()
        {
            return _films.Count();
        }
    }
}
