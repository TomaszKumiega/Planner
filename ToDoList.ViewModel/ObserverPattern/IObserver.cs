using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.ViewModel.ObserverPattern
{
    public interface IObserver
    {
        void Update();
    }
}
