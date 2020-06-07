using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ConteinerOfAnimals
    {
        //хралинилище для объектов класса
        private List<Films> _animals = new List<Films>();
        //индексатор для хранилища (можно обращаться по индексу)
        public Films this[int index]
        {
            get
            {
                return _animals[index];
            }
            set
            {
                _animals[index] = value;
            }
        }
        //методы для работы с хранилищем
        //метод добавления объекта класса в хранилище
        public void AddAnimal(Films animal)
        {
            _animals.Add(animal);
        }
        //метод удаления объекта класса из хралилища по индексу
        public void RemoveAnimal(int index)
        {
            _animals.RemoveAt(index);
        }
        // метод возвращения хранилища
        public List<Films> GetAnimals()
        {
            return _animals;
        }
        //очищение хралилища
        public void ClearAnimals()
        {
            _animals.Clear();
        }
        //возвращение объекта класса из хранилища по индексу
        public Films GetAnimal(int index)
        {
            return _animals[index];
        }
        //подсчёт количества объектов класса в хранилище
        public int AnimalsCount()
        {
            return _animals.Count();
        }
    }
}
