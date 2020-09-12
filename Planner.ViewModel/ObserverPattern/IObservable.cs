using System;
using System.Collections.Generic;
using System.Text;

namespace Planner.ViewModel.ObserverPattern
{
    public interface IObservable
    {
        List<IObserver> Observers { get; }
        void AddObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers();
    }
}
